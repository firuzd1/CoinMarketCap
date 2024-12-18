using CoinMarketCap.Interfaces.Providers;
using log4net;

namespace CoinMarketCap.Providers
{
    public class CoinMarketCapProvider : ICoinMarketCapProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private ILog _log;

        public CoinMarketCapProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _log = LogManager.GetLogger(typeof(CoinMarketCapProvider));

            // Получаем ключ из переменных окружения
            _apiKey = Environment.GetEnvironmentVariable("COINMARKETCAP_API_KEY")
                      ?? throw new ArgumentNullException("API key not found in environment variables");
        }

        public async Task<string?> GetCryptocurrencyQuoteAsync(CancellationToken token = default)
        {
            string? content = null;
            try
            {
                var requestUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("X-CMC_PRO_API_KEY", _apiKey);

                var response = await _httpClient.SendAsync(request, token);
                response.EnsureSuccessStatusCode();

                content = await response.Content.ReadAsStringAsync(token);
                
            }
            catch (Exception ex)
            {
                _log.Error("CoinMarketCapProvider.GetCryptocurrencyQuoteAsync exception is: " + ex);
            }

            return content;
        }

        public async Task<string> GetMetadataAsync(string symbol, CancellationToken token = default)
        {
            string? response1 = null;
            try
            {
                _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _apiKey);
                var endpoint = $"https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?symbol={symbol}";

                var response = await _httpClient.GetAsync(endpoint, token);
                response.EnsureSuccessStatusCode();

                response1 = await response.Content.ReadAsStringAsync(token);
            }
            catch(Exception ex)
            {
                _log.Error("CoinMarketCapProvider.GetMetadataAsync exception is: " + ex);
            }
            return response1;
        }
    }
}
