using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.NET6.Models.User
{
	public class ApiUserDto
	{
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(12,ErrorMessage ="your password limit {2} to {1} character",MinimumLength =6)]
        public string Password { get; set; }
    }
}

