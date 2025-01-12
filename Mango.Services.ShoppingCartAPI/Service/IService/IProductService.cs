using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService : IDisposable
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
    }
}
