using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface IAuthService : IDisposable
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO?> RegitserAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDTO?> EmailAlreadyTakenAsync(string email);
    }
}
