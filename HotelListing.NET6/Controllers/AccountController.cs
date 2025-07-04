using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HotelListing.NET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _manager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager manager, ILogger<AccountController> logger)
        {
            _manager = manager;
            _logger = logger;
        }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UserRegister([FromBody] ApiUserDto userDto)
        {
            _logger.LogInformation($"Registration Attemp user {userDto.Email}");

            var errors = await _manager.Register(userDto);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();


        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UserLogin([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Registration Attemp user {loginDto.Email}");

            var authResponse = await _manager.Login(loginDto);
            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }


    


    [HttpPost]
    [Route("refreshToken")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
    {
        var authResponse = await _manager.VerifyRefreshToken(request);
        if (authResponse == null)
        {
            return Unauthorized();
        }

        return Ok(authResponse);
    }
}
}
