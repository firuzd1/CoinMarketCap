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
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Interfaces.Repositories;

namespace CoinMarketCap.Services
{
    public class IdentityService : IIdentityService
    {
        private IUserRepository _userRepository;
        private FunctionsHelper _functionsHelper;
        private readonly IOptions<JwtSettingsModel> _options;
        private UserValidator _userValidator;
        private readonly Comment _comment;

        public IdentityService(
        IUserRepository userRepository,
            IOptions<JwtSettingsModel> options,
            FunctionsHelper functionsHelper,
            UserValidator userValidator,
            Comment comment)
        {
            _userRepository = userRepository;
            _options = options;
            _functionsHelper = functionsHelper;
            _userValidator = userValidator;
            _comment = comment;
        }

        public async Task<ApiResponse> GenerationToken(UserLoginDto loginDto, CancellationToken cancellationToken = default)
        {
            ApiResponse _response = new();
            _response.Params = new List<Param>();
           

            JwtSettingsModel jwtSettings = new JwtSettingsModel
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                TokenLifeTime = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES"))
            };
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

            ApiResponse validationResponse = _userValidator.ValidateLoginModel(loginDto, _comment);


            if (validationResponse.Code == ApiErrorCodes.FailedCode)
            {
                return validationResponse;
            }

            loginDto.Password = _functionsHelper.GetSHA1String(loginDto.Password);


            UserModel? user = await _userRepository.GetUserByLoginAndPasswordAsync(loginDto.Login, loginDto.Password, cancellationToken);


            if (user is null)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.InvalidLoginOrPassword;
                return _response;
            }

                // Генерация токена
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new ("UserId", user.Id.ToString()),
                new ("Login", user.Login),
                new("Lang", user.Lang.ToString())
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
            _response.Comment = _comment.Success;
            _response.Params.Add(new Param { Name = "Token", Value = jwtToken });

            return _response;


        }
    }
}
