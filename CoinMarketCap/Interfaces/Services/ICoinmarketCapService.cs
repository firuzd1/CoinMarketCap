using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models;
using CoinMarketCap.Providers;
using CoinMarketCap.Repositories;

namespace CoinMarketCap.Interfaces.Services
{
    public interface ICoinMarketCapService
    {

        public Task<ApiResponse> UpdateCryptocurrencyQuoteBaseAsync(CancellationToken token = default);
        public Task<CryptocurrencyMetaData?> GetMetadataAsync(CoinSymbols symbol, CancellationToken token = default);
        public  Task<List<CryptocurrencyData>> GetAllCryptocurrenciesAsync(int page, string? search, CancellationToken token = default);
        
    }
}
