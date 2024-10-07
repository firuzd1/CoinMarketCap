namespace CoinMarketCap.Providers
{
    public class CoinMarketCapProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CoinMarketCapProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Получаем ключ из переменных окружения
            _apiKey = Environment.GetEnvironmentVariable("COINMARKETCAP_API_KEY")
                      ?? throw new ArgumentNullException("API key not found in environment variables");
        }

        public async Task<string> GetCryptocurrencyQuoteAsync(CancellationToken token = default)
        {
            
            var requestUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-CMC_PRO_API_KEY", _apiKey);

            var response = await _httpClient.SendAsync(request, token);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token);
            return content;
        }
    }
}
