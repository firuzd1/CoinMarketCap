using System.Text.Json.Serialization;

namespace CoinMarketCap.Models
{
    public class CryptocurrencyData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("num_market_pairs")]
        public int? NumMarketPairs { get; set; }

        [JsonPropertyName("date_added")]
        public string DateAdded { get; set; }

        [JsonPropertyName("max_supply")]
        public decimal? MaxSupply { get; set; }

        [JsonPropertyName("circulating_supply")]
        public decimal? CirculatingSupply { get; set; }

        [JsonPropertyName("total_supply")]
        public decimal? TotalSupply { get; set; }

        [JsonPropertyName("cmc_rank")]
        public int? CmcRank { get; set; }

        [JsonPropertyName("quote")]
        public Quote Quote { get; set; }
    }
}
