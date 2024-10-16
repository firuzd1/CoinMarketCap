using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private IdentityService _identityService;

        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        public async Task<ApiResponse> GetTokenAsync(Lang lang, [FromBody] UserLoginDto userLogin, CancellationToken token = default)
        {
            ApiResponse _response = new();
            _response = await _identityService.GenerationToken(lang, userLogin);
            return _response;

        }
    }
}
