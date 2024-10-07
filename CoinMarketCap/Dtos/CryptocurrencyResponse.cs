using CoinMarketCap.Models;
using System.Text.Json.Serialization;

namespace CoinMarketCap.Dtos
{
    public class CryptocurrencyResponse
    {
        [JsonPropertyName("data")]
        public List<CryptocurrencyData> Data { get; set; }
    }
}
