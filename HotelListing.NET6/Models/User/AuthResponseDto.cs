﻿using System;
namespace HotelListing.NET6.Models.User
{
	public class AuthResponseDto
	{
		public string UserId { get; set; }
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}

