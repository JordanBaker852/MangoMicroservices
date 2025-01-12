using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface IProductService : IDisposable
    {
        Task<ResponseDTO?> GetProductsAsync();
        Task<ResponseDTO?> GetProductByIdAsync(int productId);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductAsync(int productId);
    }
}
