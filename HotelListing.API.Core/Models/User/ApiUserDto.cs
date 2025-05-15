using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.User
{
	public class ApiUserDto: LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

