using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ResponseDTO GetById(int id)
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
        public ResponseDTO Delete(int id)
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

                _db.Products.Remove(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(product);
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
