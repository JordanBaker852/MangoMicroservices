using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ShoppingCartController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_db.CartHeaders.First(header => header.UserId == userId))
                };

                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_db.CartDetails
                    .Where(detail => detail.CartHeaderId == cart.CartHeader.Id));

                foreach (var item in cart.CartDetails)
                {
                    cart.CartHeader.Total += (item.Quantity * item.Product.Price);
                }

                _response.Result = cart;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert([FromBody] CartDTO cartDTO)
        {
            try
            {
                var existingUserCardHeader = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(cartHeader => cartHeader.UserId == cartDTO.CartHeader.UserId);

                if (existingUserCardHeader == null)
                {
                    //create header and details
                    await CreateCartHeaderWithDetails(cartDTO);
                }
                else
                {
                    var existingCartDetails = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        cartDetail => cartDetail.ProductId == cartDTO.CartDetails.First().ProductId && 
                        cartDetail.CartHeaderId == existingUserCardHeader.Id);

                    if (existingCartDetails == null)
                    {
                        //create cart details
                        await AssignCartHeaderAndAddDetails(cartDTO, existingUserCardHeader);
                    }
                    else
                    {
                        //update quantity
                        await UpdateCartQuantity(cartDTO, existingCartDetails);
                    }
                }

                _response.Result = cartDTO;
            }
            catch (Exception ex) 
            { 
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(cart => cart.Id == cartDetailsId);

                int totalItemsInCart = _db.CartDetails.Where(cart => cart.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);

                if (totalItemsInCart == 1)
                {
                    CartHeader cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(cartHeader => cartHeader.Id == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeader);
                }

                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        private async Task CreateCartHeaderWithDetails(CartDTO cartDTO)
        {
            try
            {
                CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                _db.CartHeaders.Add(cartHeader);
                await _db.SaveChangesAsync();

                await AssignCartHeaderAndAddDetails(cartDTO, cartHeader);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task AssignCartHeaderAndAddDetails(CartDTO cartDTO, CartHeader cartHeader)
        {
            try
            {
                cartDTO.CartDetails.First().CartHeaderId = cartHeader.Id;

                await CreateCartDetails(cartDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task CreateCartDetails(CartDTO cartDTO)
        {
            try
            {
                CartDetails cardDetails = _mapper.Map<CartDetails>(cartDTO.CartDetails.First());
                _db.CartDetails.Add(cardDetails);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdateCartQuantity(CartDTO cartDTO, CartDetails cartDetails)
        {
            try
            {
                cartDTO.CartDetails.First().Quantity += cartDetails.Quantity;
                cartDTO.CartDetails.First().CartHeaderId = cartDetails.CartHeaderId;
                cartDTO.CartDetails.First().Id = cartDetails.Id;
                CartDetails cardDetails = _mapper.Map<CartDetails>(cartDTO.CartDetails.First());
                _db.CartDetails.Update(cardDetails);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}