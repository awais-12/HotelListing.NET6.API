using System;
using AutoMapper;
using HotelListing.NET6.Contracts;
using HotelListing.NET6.Data;
using HotelListing.NET6.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.NET6.Repository
{
    public class AuthManager : IAuthManager
	{
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
		{
            _mapper = mapper;
            _userManager = userManager;
        }

   

        public async Task<IEnumerable<IdentityError>> UserRegister(ApiUserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}

