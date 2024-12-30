using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface IBaseService : IDisposable
    {
       Task<ResponseDTO?> SendAsync(RequestDTO requestDto);
    }
}
