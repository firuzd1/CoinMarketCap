using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models;
using CoinMarketCap.Models.SettingsModels;
using CoinMarketCap.Repositories;
using CoinMarketCap.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PowerBankSystem.Models;
using PowerBankSystem.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoinMarketCap.Services
{
    public class IdentityService
    {
        private UserRepository _userRepository;
        private FunctionsHelper _functionsHelper;
        private readonly IOptions<JwtSettingsModel> _options;
        private UserValidator _userValidator;

        public IdentityService(
        UserRepository userRepository,
            IOptions<JwtSettingsModel> options,
            FunctionsHelper functionsHelper,
            UserValidator userValidator)
        {
            _userRepository = userRepository;
            _options = options;
            _functionsHelper = functionsHelper;
            _userValidator = userValidator;
        }

        public async Task<ApiResponse> GenerationToken(UserLoginDto loginDto, CancellationToken cancellationToken = default)
        {
            ApiResponse _response = new();
            _response.Params = new List<Param>();
            Comment comment = new();

            JwtSettingsModel jwtSettings = new JwtSettingsModel
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                TokenLifeTime = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES"))
            };
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

            ApiResponse validationResponse = _userValidator.ValidateLoginModel(loginDto, comment);


            if (validationResponse.Code == ApiErrorCodes.FailedCode)
            {
                return validationResponse;
            }

            loginDto.Password = _functionsHelper.GetSHA1String(loginDto.Password);


            UserModel? user = await _userRepository.GetUserByLoginAndPasswordAsync(loginDto.Login, loginDto.Password, cancellationToken);


            if (user is null)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = "InvalidLoginOrPassword";
                return _response;
            }

                // Генерация токена
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new ("UserId", user.Id.ToString()),
                new ("Login", user.Login),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.TokenLifeTime)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = "Успешно.";
            _response.Params.Add(new Param { Name = "Token", Value = jwtToken });

            return _response;


        }
    }
}
