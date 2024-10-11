using CoinMarketCap.Interfaces;
using CoinMarketCap.Models;
using Dapper;

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

            using var conn = await _db.CreateConnectionAsync(token);

            const string query = @"INSERT INTO public.user (login, password) 
                                    VALUES (@Login, @Password) RETURNING id ";

            insertedUserId = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, user, cancellationToken: token));
            
            return insertedUserId;
        }
    }
}
