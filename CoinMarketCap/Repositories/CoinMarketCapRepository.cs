using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces;
using CoinMarketCap.Models;
using Dapper;
using Npgsql;
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

            var deleteQuery = @"DELETE FROM public.cryptocurrency;
                                ALTER SEQUENCE cryptocurrency_id_seq RESTART WITH 1;";
            return await conn.ExecuteAsync(deleteQuery);
        }
        public async Task<bool> HasDataAsync(CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            var sql = "SELECT EXISTS(SELECT 1 FROM cryptocurrency);";

            return await conn.ExecuteScalarAsync<bool>(new CommandDefinition(sql, cancellationToken: token));
        }

        public async Task<List<CryptocurrencyData>> GetAllCryptocurrenciesAsync(int page, int itemofPage, CancellationToken token = default)
        {
            using IDbConnection? conn = await _db.CreateConnectionAsync(token);

            var sql = @"
            SELECT 
                name AS Name, 
                symbol AS Symbol, 
                slug AS Slug, 
                num_market_pairs AS NumMarketPairs, 
                date_added AS DateAdded, 
                max_supply AS MaxSupply, 
                circulating_supply AS CirculatingSupply, 
                total_supply AS TotalSupply, 
                cmc_rank AS CmcRank,
                price AS Price, 
                volume_24h AS Volume24h, 
                volume_change_24h AS VolumeChange24h, 
                percent_change_1h AS PercentChange1h, 
                percent_change_24h AS PercentChange24h, 
                percent_change_7d AS PercentChange7d, 
                percent_change_30d AS PercentChange30d, 
                percent_change_60d AS PercentChange60d, 
                percent_change_90d AS PercentChange90d, 
                market_cap AS MarketCap, 
                market_cap_dominance AS MarketCapDominance, 
                fully_diluted_market_cap AS FullyDilutedMarketCap
            FROM cryptocurrency
                     OFFSET @Page LIMIT @ItemsOfPage";

            var cryptocurrencies = await conn.QueryAsync<CryptocurrencyData, QuoteUSD, CryptocurrencyData>(
                new CommandDefinition(sql, new { Page = page, ItemsOfPage = itemofPage }, cancellationToken: token), 
                (crypto, quote) =>
                {
                    crypto.Quote = new Quote { USD = quote };
                    return crypto;
                },
                splitOn: "Price"
            );

            return cryptocurrencies.ToList();
        }

        public async Task<int> AddCryptoMetadataAsync(CryptocurrencyMetaDataResponse cryptocurrencyResponse, CancellationToken token = default)
        {
            int res = 0;
            foreach (var item in cryptocurrencyResponse.Data)
            {
                var cryptocurrency = item.Value;

                const string sql = @"
            INSERT INTO crypto_metadata (coin_market_cap_id, name, symbol, category, description, slug, logo, subreddit, date_added, infinite_supply, platform_name, platform_slug)
            VALUES (@CoinMarketCapId, @Name, @Symbol, @Category, @Description, @Slug, @Logo, @Subreddit, @DateAdded, @InfiniteSupply, @PlatformName, @PlatformSlug);";

                using IDbConnection? conn = await _db.CreateConnectionAsync(token);
                {
                    res = await conn.ExecuteAsync(sql, new
                    {
                        CoinMarketCapId = cryptocurrency.CoinMarketCapId.ToString(),
                        Name = cryptocurrency.Name,
                        Symbol = cryptocurrency.Symbol,
                        Category = cryptocurrency.Category,
                        Description = cryptocurrency.Description,
                        Slug = cryptocurrency.Slug,
                        Logo = cryptocurrency.Logo,
                        Subreddit = cryptocurrency.Subreddit,
                        DateAdded = cryptocurrency.DateAdded,
                        InfiniteSupply = cryptocurrency.InfiniteSupply,
                        PlatformName = cryptocurrency.Platform?.Name,
                        PlatformSlug = cryptocurrency.Platform?.Slug
                    });
                }
            }
            return res;
        }
    }
}
