using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using System.Text.RegularExpressions;

namespace CoinMarketCap.Validators
{
    public class UserValidator
    {
        private ApiResponse _response;
        private const string LoginPattern = @"^[a-zA-Z0-9]+$";
        private static readonly Regex _allowedLoginRegex = new Regex(LoginPattern, RegexOptions.Compiled);
        public UserValidator()
        {
            _response = new();
        }

        public ApiResponse ValidateLoginModel(UserLoginDto loginDto, Comment comment)
        {
            
            _response.Code = ApiErrorCodes.FailedCode;

            if (!_allowedLoginRegex.IsMatch(loginDto.Login))
            {
                _response.Comment = comment.InvalidLogin;
                return _response;
            }

            if (string.IsNullOrEmpty(loginDto.Password) || loginDto.Password.Length < 5)
            {
                _response.Comment = comment.InvalidPassword;
                return _response;
            }

            _response.Code = ApiErrorCodes.SuccessCode;
            return _response;
        }
    }
}
