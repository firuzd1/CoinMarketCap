using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using Dapper;
using System.Data;

namespace CoinMarketCap.Interfaces.Repositories
{
    public interface ICoinMarketCapRepository
    {
        public Task<int> CryptocurrencyQuoteAddAsync(CryptocurrencyResponse response, CancellationToken token = default);
        public Task<List<CryptocurrencyData>> GetAllCryptocurrenciesAsync(int page, int itemofPage, string? search, CancellationToken token = default);
        public Task<int> AddCryptoMetadataAsync(CryptocurrencyMetaDataResponse cryptocurrencyResponse, CancellationToken token = default);
        public Task<CryptocurrencyMetaData?> GetMetaDataFromDbAsync(string symbol, CancellationToken token = default);
    }
}
