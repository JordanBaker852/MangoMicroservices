﻿using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDTO> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var finalResponse = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if (finalResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(finalResponse.Result.ToString());
            }

            return new CouponDTO();
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}