using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
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

        public IActionResult Logout()
        {
            return View();
        }
    }
}
