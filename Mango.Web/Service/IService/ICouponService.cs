using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface ICouponService : IDisposable
    {
        Task<ResponseDTO?> GetCouponAsync(string couponCode);
        Task<ResponseDTO?> GetAllCouponsAsync();
        Task<ResponseDTO?> GetAllCouponByIdAsync(int couponId);
        Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDto);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDto);
        Task<ResponseDTO?> DeleteCouponAsync(int couponId);
    }
}
