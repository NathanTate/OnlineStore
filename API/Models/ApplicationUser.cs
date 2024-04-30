﻿using API.Models.Product;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class ApplicationUser : IdentityUser
{

    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<Review> UserReviews { get; set; } = new List<Review>();
}
