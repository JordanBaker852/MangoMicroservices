using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDTO _response;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> product = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(product => product.Id == id);

                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product ID: {id} not found";

                    return _response;
                }

                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(product => product.Id == id);

                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product ID: {id} not found";
                }
                else
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
        public ResponseDTO Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product result = _mapper.Map<Product>(productDTO);

                _db.Products.Add(result);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(result);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
        public ResponseDTO Put([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(product => product.Id == productDTO.Id);

                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product not found";
                }
                else
                {
                    //detach current product state to avoid tracking conflicts
                    _db.Entry(product).State = EntityState.Detached;

                    product = _mapper.Map<Product>(productDTO);
                    _db.Products.Update(product);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<ProductDTO>(product);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
