using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/users")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        [Authorize]
        public async Task<ApiResponse> CreateUserAsync(Lang lang, [FromBody] UserDto user, CancellationToken token = default)
            => await _userService.CreateUserAsync(lang, user, token);

        [HttpPut("update-user")]
        [Authorize]
        public async Task<ApiResponse> UpdateUserAsync(Lang lang, UserDto userDto, CancellationToken token = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue("UserId"));
            return await _userService.UpdateUserAsync(userId, lang, userDto, token);
        }

        [HttpPut("update-language")]
        [Authorize]
        public async Task<ApiResponse> ChangeLanguageAsync(Lang lang, CancellationToken token = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue("UserId"));
            return await _userService.ChangeLanguageAsync(userId, lang, token);
        }
    }
}
