using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.UserDTO.Requests
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [Length(6, 32)]
        public string Password { get; set; }
    }
}
