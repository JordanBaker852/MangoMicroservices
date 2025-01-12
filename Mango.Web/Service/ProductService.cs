using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                HttpRequest = StaticDetails.HttpRequestType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product"
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                HttpRequest = StaticDetails.HttpRequestType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/{productId}"
            });
        }

        public async Task<ResponseDTO?> AddProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                HttpRequest = StaticDetails.HttpRequestType.POST,
                Url = $"{StaticDetails.ProductAPIBase}/api/product",
                Data = productDTO
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                HttpRequest = StaticDetails.HttpRequestType.DELETE,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/{productId}"
            });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                HttpRequest = StaticDetails.HttpRequestType.PUT,
                Url = $"{StaticDetails.ProductAPIBase}/api/product",
                Data = productDTO
            });
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
