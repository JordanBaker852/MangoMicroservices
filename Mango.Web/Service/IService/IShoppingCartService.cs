using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService : IDisposable
    {
        Task<ResponseDTO?> GetCartByUserId(Guid userId);
        Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO);
        Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDTO?> ApplyCouponAsync(CouponDTO couponDto);
    }
}
