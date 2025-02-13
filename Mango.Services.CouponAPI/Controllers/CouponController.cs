﻿using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Mango.Services.CouponAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public CouponController(AppDbContext db, IMapper mapper)
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
        [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
        public ResponseDTO Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon result = _mapper.Map<Coupon>(couponDTO);
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
        [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
        public ResponseDTO Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon result = _db.Coupons.FirstOrDefault(coupon => coupon.Id == couponDTO.Id);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                }
                else
                {
                    //detach current coupon state to avoid tracking conflicts
                    _db.Entry(result).State = EntityState.Detached;

                    result = _mapper.Map<Coupon>(couponDTO);
                    _db.Coupons.Update(result);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<CouponDTO>(result);
                }
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
                Coupon result = _db.Coupons.FirstOrDefault(coupon => coupon.Id == id);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon ID: {id} not found";
                }
                else
                {
                    _db.Coupons.Remove(result);
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
    }
}
