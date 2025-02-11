using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICouponService _couponService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, ICouponService couponService)
        {
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            CartDTO cartDTO = await LoadCartDTOForUser();

            return View(cartDTO);
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (userId == null)
            {
                return null;
            }

            ResponseDTO? response = await _shoppingCartService.RemoveFromCartAsync(cartDetailsId);

            if (response == null || !response.IsSuccess)
            {
                return View();
            }

            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            if (string.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
            {
                TempData["error"] = "Coupon required";
                return RedirectToAction(nameof(Index));
            }

            ResponseDTO? response = await _shoppingCartService.ApplyCouponAsync(cartDTO);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response.Message;
            }
            else
            {
                TempData["success"] = "Cart updated successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            cartDTO.CartHeader.CouponCode = string.Empty;

            ResponseDTO? response = await _shoppingCartService.ApplyCouponAsync(cartDTO);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response.Message;
            }
            else
            {
                TempData["success"] = "Cart updated successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<CartDTO> LoadCartDTOForUser()
        {
            var userId = User.Claims.Where(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (userId == null)
            {
                return null;
            }

            ResponseDTO? response = await _shoppingCartService.GetCartByUserId(new Guid(userId));

            if (response == null || !response.IsSuccess)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<CartDTO>(response.Result?.ToString());
        }
    }
}
