using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                ResponseDTO responseDTO = await _authService.LoginAsync(loginRequestDTO);

                if (responseDTO == null || !responseDTO.IsSuccess)
                {
                    ModelState.AddModelError("CustomError", responseDTO.Message);
                    TempData["error"] = responseDTO.Message;
                    return View(loginRequestDTO);
                }

                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(responseDTO.Result.ToString());

                await SignInUser(loginResponseDTO);

                _tokenProvider.SetToken(loginResponseDTO.Token);

                TempData["success"] = "Login Successful";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured with the login attempt: {ex.Message}";
                return View(loginRequestDTO);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var userRoles = new List<SelectListItem>() {
                new SelectListItem(StaticDetails.ROLE_ADMIN, StaticDetails.ROLE_ADMIN),
                new SelectListItem(StaticDetails.ROLE_CUSTOMER, StaticDetails.ROLE_CUSTOMER)
            };

            ViewBag.UserRoles = userRoles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                ResponseDTO emailTakenResponse = await _authService.EmailAlreadyTakenAsync(registrationRequestDTO.Email);

                if (emailTakenResponse.IsSuccess && (bool)emailTakenResponse.Result)
                {
                    TempData["error"] = "Email is already taken";
                    return RedirectToAction(nameof(Register));
                }

                ResponseDTO result = await _authService.RegitserAsync(registrationRequestDTO);

                if (result == null || !result.IsSuccess)
                {
                    TempData["error"] = $"Unable to register new user: {result.Message}";
                    return RedirectToAction(nameof(Register));
                }

                if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                {
                    registrationRequestDTO.Role = StaticDetails.ROLE_CUSTOMER;
                }

                ResponseDTO assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);

                if (assignRole == null || !assignRole.IsSuccess)
                {
                    TempData["error"] = $"Unable to assign user role: {result.Message}";
                    return RedirectToAction(nameof(Register));
                }

                TempData["success"] = "Registration Successful";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex) 
            { 
                TempData["error"] = $"An error occured with the registration: {ex.Message}";
                return RedirectToAction(nameof(Register));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            TempData["success"] = "Logout Successful";

            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(claim => claim.Type == StaticDetails.JWT_ROLE_CLAIM_KEY).Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }
    }
}
