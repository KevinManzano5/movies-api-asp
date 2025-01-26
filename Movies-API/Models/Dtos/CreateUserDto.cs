﻿using System.ComponentModel.DataAnnotations;

namespace Movies_API.Models.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
