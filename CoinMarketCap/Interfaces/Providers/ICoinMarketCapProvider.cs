using System.Net.Http;

namespace CoinMarketCap.Interfaces.Providers
{
    public interface ICoinMarketCapProvider
    {
        public Task<string> GetCryptocurrencyQuoteAsync(CancellationToken token = default);

        public Task<string> GetMetadataAsync(string symbol, CancellationToken token = default);
    }
}
