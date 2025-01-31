using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<ProductDTO>? list = new();

                ResponseDTO? response = await _productService.GetProductsAsync();

                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
                }
                else
                {
                    TempData["error"] = response?.Message;
                }

                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occured: {ex.Message}");
                return Error();
            }
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            try
            {
                ProductDTO? product = new();

                ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

                if (response != null && response.IsSuccess)
                {
                    product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                }
                else
                {
                    TempData["error"] = response?.Message;
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occured: {ex.Message}");
                return Error();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductDTO productDto)
        {
            try
            {
                var cartDTO = new CartDTO()
                {
                    CartHeader = new CartHeaderDTO()
                    {
                        UserId = User.Claims.Where(user => user.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                    }
                };

                var cartDetailsDTO = new CartDetailsDTO()
                {
                    Quantity = productDto.Quantity,
                    ProductId = productDto.Id
                };

                var cartDetailsDTOs = new List<CartDetailsDTO>() 
                {
                    cartDetailsDTO
                };

                cartDTO.CartDetails = cartDetailsDTOs;

                ResponseDTO? response = await _shoppingCartService.UpsertCartAsync(cartDTO);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Item added to cart successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                    return RedirectToAction(nameof(ProductDetails), productDto.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occured: {ex.Message}");
                return Error();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
