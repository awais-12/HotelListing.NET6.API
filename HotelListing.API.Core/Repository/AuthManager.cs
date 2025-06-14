﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.User;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HotelListing.API.Core.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _user;
        private const string _loginProvider = "HotelListing";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration,ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

 
        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
             _user = await _userManager.FindByNameAsync(username);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {

            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (_user == null || !validPassword)
            {
                _logger.LogWarning($"Loging user not found {loginDto.Email} and {nameof(Login)}");
                return null;
            }

            var token = await GenerateToken();

            _logger.LogInformation($"Login user with email {loginDto.Email} and the token is {token}");
            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token,
                RefreshToken = await CreateRefreshToken()

            };

        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }
            return result.Errors;
        }

        private async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email),
                new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email ,_user.Email),
                new Claim("uid",_user.Id),



            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                 issuer: _configuration["JwtSettings:Issuer"],
                 audience: _configuration["JwtSettings:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddMinutes(
                     Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])
                 ),
                 signingCredentials: credentials
                 );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

