﻿using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.UserDTO.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
