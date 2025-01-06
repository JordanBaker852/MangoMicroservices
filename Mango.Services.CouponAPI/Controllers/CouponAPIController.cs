using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Mango.Services.CouponAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
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
                IEnumerable<Coupon> result = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(result);
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
                Coupon result = _db.Coupons.FirstOrDefault(coupon => coupon.Id == id);
                _response.Result = _mapper.Map<Coupon>(result);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Coupon result = _db.Coupons.FirstOrDefault(coupon => coupon.Code.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<Coupon>(result);
                _response.IsSuccess = result != null;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon result = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(result);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(result);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon result = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(result);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(result);
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
                Coupon result = _db.Coupons.FirstOrDefault(coupon => coupon.Id == id);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon ID: {id} not found";
                    return _response;
                }

                _db.Coupons.Remove(result);
                _db.SaveChanges();
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
