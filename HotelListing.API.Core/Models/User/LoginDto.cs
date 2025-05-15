using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.User
{
	public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(12, ErrorMessage = "your password limit {2} to {1} character", MinimumLength = 6)]
        public string Password { get; set; }

    }
}

