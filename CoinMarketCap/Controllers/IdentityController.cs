using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
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
        public async Task<ApiResponse> GetTokenAsync([FromBody] UserLoginDto userLogin)
        {
            ApiResponse _response = new();
            _response = await _identityService.GenerationToken(userLogin);
            return _response;

        }
    }
}
