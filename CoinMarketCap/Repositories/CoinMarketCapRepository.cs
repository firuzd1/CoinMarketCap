using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces;
using CoinMarketCap.Models;
using Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoinMarketCap.Repositories
{
    public class CoinMarketCapRepository
    {
        private readonly IDbConnectionFactory _db;
        public CoinMarketCapRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<int> CryptocurrencyQuoteAddAsync(CryptocurrencyResponse response,
     CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            var sql = @"
                    INSERT INTO cryptocurrency 
                    (name, symbol, slug, num_market_pairs, date_added, max_supply, circulating_supply, total_supply, cmc_rank,
                     price, volume_24h, volume_change_24h, percent_change_1h, percent_change_24h, percent_change_7d, percent_change_30d, percent_change_60d, percent_change_90d, market_cap, market_cap_dominance, fully_diluted_market_cap)
                    VALUES 
                    (@Name, @Symbol, @Slug, @NumMarketPairs, @DateAdded, @MaxSupply, @CirculatingSupply, @TotalSupply, @CmcRank,
                     @Price, @Volume24h, @VolumeChange24h, @PercentChange1h, @PercentChange24h, @PercentChange7d, @PercentChange30d, @PercentChange60d, @PercentChange90d, @MarketCap, @MarketCapDominance, @FullyDilutedMarketCap)";

            foreach (var crypto in response.Data)
            {
                var quoteUSD = crypto.Quote?.USD;
                if (quoteUSD == null)
                {
                    throw new NullReferenceException($"Quote.USD for {crypto.Name} is null.");
                }

                var parameters = new
                {
                    crypto.Name,
                    crypto.Symbol,
                    crypto.Slug,
                    crypto.NumMarketPairs,
                    dateAdded = string.IsNullOrEmpty(crypto.DateAdded) ? (DateTime?)null : DateTime.Parse(crypto.DateAdded),
                    crypto.MaxSupply,
                    crypto.CirculatingSupply,
                    crypto.TotalSupply,
                    crypto.CmcRank,
                    Price = quoteUSD.Price,
                    Volume24h = quoteUSD.Volume24h,
                    VolumeChange24h = quoteUSD.VolumeChange24h,
                    PercentChange1h = quoteUSD.PercentChange1h,
                    PercentChange24h = quoteUSD.PercentChange24h,
                    PercentChange7d = quoteUSD.PercentChange7d,
                    PercentChange30d = quoteUSD.PercentChange30d,
                    PercentChange60d = quoteUSD.PercentChange60d,
                    PercentChange90d = quoteUSD.PercentChange90d,
                    MarketCap = quoteUSD.MarketCap,
                    MarketCapDominance = quoteUSD.MarketCapDominance,
                    FullyDilutedMarketCap = quoteUSD.FullyDilutedMarketCap
                };

                // Выполняем запрос для каждой записи
                await conn.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
            }

            return response.Data.Count; // Возвращаем количество добавленных записей
        }

        public async Task<int> ClearCryptocurrencyTable(CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            var deleteQuery = "DELETE FROM cryptocurrency_json;";
            return await conn.ExecuteAsync(deleteQuery);
        }
    } 
}
