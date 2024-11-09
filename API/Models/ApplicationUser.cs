using API.Models.ProductModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class ApplicationUser : IdentityUser
{

    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(64)]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<Review> UserReviews { get; set; } = new List<Review>();
}
