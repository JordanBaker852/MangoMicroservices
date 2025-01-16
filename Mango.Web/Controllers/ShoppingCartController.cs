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

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            CartDTO cartDTO = await LoadCartDTOForUser();

            return View(cartDTO);
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

            return JsonConvert.DeserializeObject<CartDTO>(response.Result.ToString());
        }
    }
}
