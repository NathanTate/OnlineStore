using API.Interfaces;
using API.Models.Coupon;
using API.Models.DTO.Coupon;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.CouponRepositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<CouponDto>> CreateCouponAsync(CouponDto model)
        {
            var coupon = _mapper.Map<Coupon>(model);
            coupon.CouponCode = coupon.CouponCode.ToLower();

            if(_dbContext.Coupons.Any(c => c.CouponCode == coupon.CouponCode)) 
            {
                return Result.Fail("Coupon Already Exists");
            }

            var options = new Stripe.CouponCreateOptions
            {
                Id = model.CouponCode.ToLower(),
                Name = model.CouponCode.ToLower(),
                Currency = "USD",
                AmountOff = (long)model.DiscountAmount * 100
            };
            var service = new Stripe.CouponService();
            Stripe.Coupon response = await service.CreateAsync(options);

            coupon.CouponStripeId = response.Id;

            await _dbContext.Coupons.AddAsync(coupon);

            return Result.Ok(_mapper.Map<CouponDto>(coupon));
        }

        public async Task<IEnumerable<CouponDto>> GetCouponsAsync()
        {
            return _mapper.Map<IEnumerable<CouponDto>>(await _dbContext.Coupons.ToListAsync());
        }

        public async Task<Result<CouponDto>> GetCouponAsync(string code)
        {
            var coupon = await _dbContext.Coupons.FindAsync(code.ToLower());

            if (coupon == null)
            {
                return Result.Fail("Coupon doesn't exist");
            }

            return Result.Ok(_mapper.Map<CouponDto>(coupon));
        }

        public async Task<Result> UpdateCouponAsync(CouponDto model)
        {
            var coupon = await _dbContext.Coupons.FindAsync(model.CouponCode.ToLower());

            if(coupon == null)
            {
                return Result.Fail("Coupon doesn't exist");
            }

            _mapper.Map(model, coupon);

            return Result.Ok();
        }

        public async Task<Result> DeleteCouponAsync(string code)
        {
            var coupon = await _dbContext.Coupons.FindAsync(code.ToLower());

            if(coupon == null)
            {
                return Result.Fail("Coupon doesn't exist");
            }

            var service = new Stripe.CouponService();
            await service.DeleteAsync(coupon.CouponStripeId);

            _dbContext.Coupons.Remove(coupon);

            return Result.Ok();
        }
    }
}
