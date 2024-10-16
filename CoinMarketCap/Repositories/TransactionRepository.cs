using CoinMarketCap.Interfaces;
using CoinMarketCap.Models;
using Dapper;
using System.Data;

namespace CoinMarketCap.Repositories
{
    public class TransactionRepository
    {
        private IDbConnectionFactory _db;

        public TransactionRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<int> TransactionSimulationAsync(TransactionModel transaction, CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            var query = @" INSERT INTO public.transactions (amount, time_of_purchase_price, user_id, crypto_metadata_id) 
                                VALUES (@Amount, @TimeOfPurchasePrice, @UserId, @CryptoMetadataId)";

             return await conn.ExecuteAsync(new CommandDefinition(query, transaction, cancellationToken : token));
        }

        public async Task<decimal> GetCurrentExchangeRateBySymbolAsync(string symbol, CancellationToken token = default)
        {
            using IDbConnection conn = await _db.CreateConnectionAsync(token);

            string query = @"SELECT  price
	                        FROM public.cryptocurrency 
                            WHERE symbol = @Symbol ORDER BY updated_date DESC LIMIT 1;";

            return await conn.QueryFirstOrDefaultAsync<decimal>(new CommandDefinition(query, new { Symbol = symbol }, cancellationToken: token));
        }

        public async Task<List<TransactionModel>> CheckBalanceAsync(int userId, string? search, int skip, int limit, CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"SELECT tr.id AS Id, tr.amount AS Amount, tr.time_of_purchase_price AS TimeOfPurchasePrice, md.symbol AS CoinSymbol, tr.created_date AS CreatedDate, 
	                            ROUND((SELECT price FROM public.cryptocurrency WHERE symbol = md.symbol ORDER BY id DESC LIMIT 1) * tr.amount, 4) AS CurrentPrice
	                            FROM public.transactions AS tr
                            JOIN 
	                            public.user AS u ON u.id =  tr.user_id
                            JOIN 
	                            public.crypto_metadata AS md ON md.id = tr.crypto_metadata_id
                            WHERE (@Search IS NULL OR md.symbol LIKE ('%' || @Search || '%')) 
                            AND u.id = @UserId ORDER BY tr.id DESC OFFSET @Offset LIMIT @Limit";

            IEnumerable<TransactionModel> res = await conn.QueryAsync<TransactionModel>(new CommandDefinition(query, new {UserId = userId,
                Search = search, Limit = limit, Offset = skip}, cancellationToken:token));
            return res.ToList();
        }

        public async Task<int> GetCTransactionCountAsync(string? search, int userId, CancellationToken token)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            string query = @"SELECT COUNT(*)
                                FROM public.transactions AS tr
                            JOIN 
                                public.user AS u ON u.id =  tr.user_id
                            JOIN 
                                public.crypto_metadata AS md ON md.id = tr.crypto_metadata_id
                            WHERE (@Search IS NULL OR md.symbol LIKE ('%' || @Search || '%')) 
                            AND u.id = @UserId";

            return await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition( query, new { UserId = userId, Search = search },cancellationToken: token));
        }
    }
}
