using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = registrationRequestDTO,
                Url = $"{StaticDetails.AuthAPIBase}/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = loginRequestDTO,
                Url = $"{StaticDetails.AuthAPIBase}/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegitserAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = registrationRequestDTO,
                Url = $"{StaticDetails.AuthAPIBase}/api/auth/register"
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> EmailAlreadyTakenAsync(string email)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = email,
                Url = $"{StaticDetails.AuthAPIBase}/api/auth/EmailTaken"
            }, withBearer: false);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
