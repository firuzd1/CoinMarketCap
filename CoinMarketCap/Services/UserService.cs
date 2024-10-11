using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models;
using CoinMarketCap.Repositories;
using PowerBankSystem.Models.Enums;
using PowerBankSystem.Models;
using CoinMarketCap.Mappers;
using FluentValidation;

namespace CoinMarketCap.Services
{
    public class UserService
    {
        private UserRepository _repository;
        private FunctionsHelper _functionsHelper;
        private IValidator<UserDto> _userValidatorFl;

        public UserService(UserRepository repository, FunctionsHelper functionsHelper, IValidator<UserDto> userValidatorFl)
        {
            _repository = repository;
            _functionsHelper = functionsHelper;
            _userValidatorFl = userValidatorFl;
        }

        public async Task<ApiResponse> CreateUserAsync(UserDto userDto, CancellationToken token = default)
        {

            ApiResponse _response = new();
            _response.Params = new List<Param>();
            var validationResult = await _userValidatorFl.ValidateAsync(userDto, o =>
                o.IncludeRuleSets("Create").ThrowOnFailures(), token);

            var comment = new Comment();

            var user = userDto.UserDtoToUserModel();
            user.Password = _functionsHelper.GetSHA1String(user.Password);

            int userId = await _repository.CreateUserAsync(user, token);

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = "Успешно.";
            _response.Params.Add(new Param { Name = "UserId", Value = userId.ToString() });

            if (userId <= 0)
            {
                _response.Code = ApiErrorCodes.FailedCode;
                _response.Comment = "Попробуйте позже.";
            }
            return _response;
        }
    }
}
