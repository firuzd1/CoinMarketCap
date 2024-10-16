using CoinMarketCap.Interfaces;
using CoinMarketCap.Models;
using Dapper;
using System.Data;

namespace CoinMarketCap.Repositories
{
    public class UserRepository
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

            const string query = @"INSERT INTO public.user (login, password) 
                                    VALUES (@Login, @Password) RETURNING id ";

            insertedUserId = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, user, cancellationToken: token));
            
            return insertedUserId;
        }

        public async Task<UserModel?> GetUserByLoginAndPasswordAsync(string login, string password, CancellationToken token = default)
        {
            UserModel? user = null;

            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"SELECT id AS Id, login as Login, password AS Password
                            FROM ""user""
                            WHERE login = @Login AND password = @Password";

            user = await conn.QueryFirstOrDefaultAsync<UserModel>(new CommandDefinition(query, new { Login = login, Password = password }, cancellationToken: token));
      

            return user;
        }
    }
}
