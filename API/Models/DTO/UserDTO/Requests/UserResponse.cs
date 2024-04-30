using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.UserDTO.Requests
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}
