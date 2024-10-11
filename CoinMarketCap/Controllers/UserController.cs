using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/users")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        public async Task<ApiResponse> CreateUserAsync([FromBody] UserDto user)
            => await _userService.CreateUserAsync(user);
    }
}
