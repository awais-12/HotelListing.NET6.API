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

        public async Task<bool> Login(LoginDto loginDto)
        {
            bool isValidUser = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                    isValidUser = false;
                var validPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                isValidUser = validPassword;
            }
            catch (Exception ex)
            {

            }

            return isValidUser;
        }

        public async Task<IEnumerable<IdentityError>> UserRegister(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result.Errors;
        }
    }
}

