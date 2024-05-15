using API.Helpers;
using API.Helpers.OrderParameters;
using API.Models.DTO.Cart.CartResponses;
using API.Models.DTO.Order;
using API.Models.DTO.Order.Requests;
using FluentResults;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<PagedList<OrderHeaderDto>> GetOrdersAsync(OrderParams orderParams, string userId);
        Task<Result<OrderHeaderDto>> GetOrderAsync(int orderHeaderId, string userId);
        Task<Result<string>> CheckoutAsync(CartResponse cart);
        Task<Result> VerifyStripeSessionAsync(int orderHeaderId);
        Task<Result> UpdateOrderAsync(OrderUpdateRequest model);
    }
}
