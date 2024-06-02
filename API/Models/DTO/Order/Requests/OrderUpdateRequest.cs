using static API.Utility.SD;

namespace API.Models.DTO.Order.Requests
{
    public record OrderUpdateRequest(int orderHeaderId, OrderStatus OrderStatus)
    {
        public OrderUpdateRequest() : this(default, default)
        {
            
        }
    };
}
