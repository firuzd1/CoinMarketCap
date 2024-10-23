using CoinMarketCap.Interfaces;
using CoinMarketCap.Interfaces.Repositories;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using Dapper;
using System.Data;

namespace CoinMarketCap.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _db;
        public UserRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async ValueTask<int> CreateUserAsync(UserModel user, CancellationToken token = default)
        {
            int insertedUserId = 0;

            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            const string query = @"INSERT INTO public.user (login, password, lang) 
                                    VALUES (@Login, @Password, @Lang) RETURNING id ";

            insertedUserId = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, user, cancellationToken: token));
            
            return insertedUserId;
        }

        public async Task<UserModel?> GetUserByLoginAndPasswordAsync(string login, string password, CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"SELECT id AS Id, login as Login, password AS Password, lang AS Lang
                            FROM ""user""
                            WHERE login = @Login AND password = @Password";

            return await conn.QueryFirstOrDefaultAsync<UserModel>(new CommandDefinition(query, new { Login = login, Password = password }, cancellationToken: token));
        }

        public async Task<int> UpdateUserAsync(UserModel user, CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"UPDATE ""user""
                     SET login = @Login,
                         password = @Password,
                         lang = @Lang
                     WHERE id = @Id";

            return await conn.ExecuteAsync(new CommandDefinition(query, new
            {
                user.Login,
                user.Password,
                user.Lang,
                user.Id
            }, cancellationToken: token));
        }

        public async Task<int> ChangeLanguageAsync(Lang lang, int userId, CancellationToken token)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"UPDATE ""user"" SET lang = @Lang WHERE id = @Id";

            return await conn.ExecuteAsync(new CommandDefinition(query, new {Lang = lang, Id = userId}));
        }
    }
}
