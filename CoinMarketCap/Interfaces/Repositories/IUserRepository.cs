using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models;
using Dapper;
using System.Data;

namespace CoinMarketCap.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public ValueTask<int> CreateUserAsync(UserModel user, CancellationToken token = default);
        public Task<UserModel?> GetUserByLoginAndPasswordAsync(string login, string password, CancellationToken token = default);
        public Task<int> UpdateUserAsync(UserModel user, CancellationToken token = default);
        public Task<int> ChangeLanguageAsync(Lang lang, int userId, CancellationToken token);
    }
}
