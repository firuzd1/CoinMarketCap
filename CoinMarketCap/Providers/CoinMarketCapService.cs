namespace CoinMarketCap.Providers
{
    public class CoinMarketCapService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CoinMarketCapService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Получаем ключ из переменных окружения
            _apiKey = Environment.GetEnvironmentVariable("COINMARKETCAP_API_KEY")
                      ?? throw new ArgumentNullException("API key not found in environment variables");
        }

        public async Task<string> GetCryptocurrencyQuoteAsync(CancellationToken token = default)
        {
            string key = _apiKey;
            return key;
        }
    }
}
