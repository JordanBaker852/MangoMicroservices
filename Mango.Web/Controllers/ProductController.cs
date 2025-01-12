using Mango.Web.Models.DTO;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> Create()
        {
            var productCategories = new List<SelectListItem>() {
                new SelectListItem(StaticDetails.CATEGORY_APPETIZER, StaticDetails.CATEGORY_APPETIZER),
                new SelectListItem(StaticDetails.CATEGORY_ENTREE, StaticDetails.CATEGORY_ENTREE),
                new SelectListItem(StaticDetails.CATEGORY_DESERT, StaticDetails.CATEGORY_DESERT)
            };

            ViewBag.ProductCategories = productCategories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

            var productCategories = new List<SelectListItem>() {
                new SelectListItem(StaticDetails.CATEGORY_APPETIZER, StaticDetails.CATEGORY_APPETIZER),
                new SelectListItem(StaticDetails.CATEGORY_ENTREE, StaticDetails.CATEGORY_ENTREE),
                new SelectListItem(StaticDetails.CATEGORY_DESERT, StaticDetails.CATEGORY_DESERT)
            };

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                ViewBag.ProductCategories = productCategories;
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.UpdateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(int productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            ResponseDTO? response = await _productService.DeleteProductAsync(productId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = $"Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }
    }
}
