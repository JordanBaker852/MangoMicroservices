using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;

        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetCartByUserId(Guid userId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.GET,
                Url = $"{StaticDetails.ShoppingCartAPIBase}/api/shoppingcart/GetCart/{userId}"
            });
        }

        public async Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = cartDTO,
                Url = $"{StaticDetails.ShoppingCartAPIBase}/api/shoppingcart/CartUpsert"
            });
        }

        public async Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = cartDetailsId,
                Url = $"{StaticDetails.ShoppingCartAPIBase}/api/shoppingcart/RemoveCart"
            });
        }

        public async Task<ResponseDTO?> ApplyCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = couponDto,
                Url = $"{StaticDetails.ShoppingCartAPIBase}/api/shoppingcart/ApplyCoupon"
            });
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
