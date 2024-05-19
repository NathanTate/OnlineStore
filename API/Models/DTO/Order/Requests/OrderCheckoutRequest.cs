using API.Models.DTO.Address.Requests;
using API.Models.DTO.Cart.CartResponses;

namespace API.Models.DTO.Order.Requests
{
    public class OrderCheckoutRequest
    {
        public CartResponse CartResponse { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressRequest Address { get; set; }
    }
}
