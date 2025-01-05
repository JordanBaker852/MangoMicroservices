using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator : IDisposable
    {
        string GenerateToken(ApplicationUser applicationsUser);
    }
}
