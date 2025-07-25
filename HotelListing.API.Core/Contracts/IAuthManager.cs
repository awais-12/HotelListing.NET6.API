﻿using System;
using HotelListing.API.Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.Contracts
{
	public interface IAuthManager
	{
		Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
		Task<AuthResponseDto> Login(LoginDto loginDto);
		Task<string> CreateRefreshToken();
		Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

	}
}

