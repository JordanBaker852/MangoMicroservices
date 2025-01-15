using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService : IDisposable
    {
        Task<CouponDTO> GetCouponAsync(string couponCode);
    }
}
