﻿using System;
namespace HotelListing.API.Core.Models.User
{
	public class AuthResponseDto
	{
		public string UserId { get; set; }
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}

