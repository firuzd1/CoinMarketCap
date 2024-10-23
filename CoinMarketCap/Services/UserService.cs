using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models;
using CoinMarketCap.Repositories;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models;
using CoinMarketCap.Mappers;
using FluentValidation;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Interfaces.Repositories;

namespace CoinMarketCap.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private FunctionsHelper _functionsHelper;
        private readonly Comment _comment;
        private IValidator<UserDto> _userValidatorFl;

        public UserService(IUserRepository repository, FunctionsHelper functionsHelper, IValidator<UserDto> userValidatorFl, Comment comment)
        {
            _repository = repository;
            _functionsHelper = functionsHelper;
            _userValidatorFl = userValidatorFl;
            _comment = comment;
        }

        public async Task<ApiResponse> CreateUserAsync(Lang lang, UserDto userDto, CancellationToken token = default)
        {

            ApiResponse _response = new();
            _response.Params = new List<Param>();
            var validationResult = await _userValidatorFl.ValidateAsync(userDto, o =>
                o.IncludeRuleSets("Create").ThrowOnFailures(), token);

            var user = userDto.UserDtoToUserModel(lang);
            user.Password = _functionsHelper.GetSHA1String(user.Password);

            int userId = await _repository.CreateUserAsync(user, token);

            if (userId <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.PleaseTryLaiter;
                return _response;
            }

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = _comment.Success;
            _response.Params.Add(new Param { Name = "UserId", Value = userId.ToString() });

            return _response;
        }

        public async Task<ApiResponse> UpdateUserAsync(int userId, Lang lang, UserDto userDto, CancellationToken token = default)
        {
            ApiResponse _response = new();
            _response.Params = new List<Param>();

            if(userId <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.InvalidUserId;
                return _response;

            }

            var validationResult = await _userValidatorFl.ValidateAsync(userDto, o =>
                o.IncludeRuleSets("Create").ThrowOnFailures(), token);

            var user = userDto.UpdateUserDtoToUserModel(userId, lang);
            user.Password = _functionsHelper.GetSHA1String(user.Password);

            int reposResponse = await _repository.UpdateUserAsync(user, token);

            if (reposResponse <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.PleaseTryLaiter;
                return _response;
            }

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = _comment.Success;
            _response.Params.Add(new Param { Name = "UserId", Value = userId.ToString() });

            return _response;
        }

        public async Task<ApiResponse> ChangeLanguageAsync(int userId, Lang lang, CancellationToken token)
        {
            ApiResponse _response = new();
            _response.Params = new List<Param>();

            if (userId <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.InvalidUserId;
                return _response;
            }

            int reposResponese = await _repository.ChangeLanguageAsync(lang, userId, token);

            if (reposResponese <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = _comment.PleaseTryLaiter;
                return _response;
            }

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = _comment.Success;
            _response.Params.Add(new Param { Name = "UserId", Value = userId.ToString() });

            return _response;
        }
    }
}
