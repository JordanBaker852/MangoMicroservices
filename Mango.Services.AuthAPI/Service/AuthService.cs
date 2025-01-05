using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;

            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password); 

                if (result.Succeeded)
                {
                    var returnUser = _db.ApplicationUsers.First(user => user.Email.ToLower() == registrationRequestDTO.Email.ToLower());

                    UserDTO userDTO = new()
                    {
                        Id = returnUser.Id,
                        Email = returnUser.Email,
                        Name = returnUser.Name,
                        PhoneNumber = returnUser.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex) 
            {
                return $"Error encountered: {ex.Message}";
            }
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(user => user.Email.ToLower() == loginRequestDTO.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = string.Empty
                };
            }

            //Generate JWT token
            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, userRoles);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDTO = new()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDTO;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(user => user.Email.ToLower() == email.ToLower());

            if (user == null) {
                return false;
            }

            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                //create role
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        public bool EmailTaken(string email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(user => user.Email.ToLower() == email.ToLower());

            return user != null;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
