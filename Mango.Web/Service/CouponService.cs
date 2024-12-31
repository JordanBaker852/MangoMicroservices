using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Data = couponDto,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupon"
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.DELETE,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupon/{couponId}"
            });
        }

        public async Task<ResponseDTO?> GetAllCouponByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDTO() { 
                HttpRequest = StaticDetails.HttpRequestType.GET, 
                Url = $"{StaticDetails.CouponAPIBase}/api/coupon/{couponId}" 
            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.GET,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.GET,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupon/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                HttpRequest = StaticDetails.HttpRequestType.PUT,
                Data = couponDto,
                Url = $"{StaticDetails.CouponAPIBase}/api"
            });
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
