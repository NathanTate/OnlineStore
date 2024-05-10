using API.Models.DTO.Coupon;
using FluentResults;

namespace API.Interfaces
{
    public interface ICouponRepository
    {
        Task<Result<CouponDto>> CreateCouponAsync(CouponDto model);
        Task<Result<CouponDto>> GetCouponAsync(string code);
        Task<IEnumerable<CouponDto>> GetCouponsAsync();
        Task<Result> UpdateCouponAsync(CouponDto model);
        Task<Result> DeleteCouponAsync(string code);

    }
}
