using System;
using HotelListing.NET6.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.NET6.Contracts
{
	public interface IAuthManager
	{
		Task<IEnumerable<IdentityError>> UserRegister(ApiUserDto userDto);
	}
}

