using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models;
using CoinMarketCap.Validators;

namespace CoinMarketCap.Interfaces.Services
{
    public interface IUserService
    {
        public Task<ApiResponse> CreateUserAsync(Lang lang, UserDto userDto, CancellationToken token = default);

        public Task<ApiResponse> UpdateUserAsync(int userId, Lang lang, UserDto userDto, CancellationToken token = default);

        public Task<ApiResponse> ChangeLanguageAsync(int userId, Lang lang, CancellationToken token);
    }
}
