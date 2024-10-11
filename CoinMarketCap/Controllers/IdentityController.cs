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

            //// Проверка учетных данных (пример проверки)
            //if (userLogin.Login == "test" && userLogin.Password == "password")
            //{
            //    // Генерация токена
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new[] { new Claim("id", "1") }),
            //        Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //        Issuer = _configuration["Jwt:Issuer"],
            //        Audience = _configuration["Jwt:Audience"]
            //    };
            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    var tokenString = tokenHandler.WriteToken(token);

            //    return Ok(new { Token = tokenString });
            //}

            //return Unauthorized();
        }
    }
}
