using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using FluentResults;

namespace API.Interfaces
{
    public interface ICartRepository
    {
        Task<Result<CartResponse>> GetCartAsync(string userId);
        Task<Result<CartResponse>> CreateCartAsync(string userId);
        Task<Result> UpdateCartAsync(CartDetailRequest model, string userId);
        Task<Result> ApplyCouponAsync(CartHeaderRequest model, string userId);
        Task<Result> RemoveFromCartAsync(int cartDetailsId, bool removeAll, string userId);
        Task<bool> CartExistsAsync(string userId);
    }
}
