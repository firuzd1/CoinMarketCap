using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class CryptocurrencyMetaDataResponse
{
    [JsonPropertyName("data")]
    public Dictionary<string, CryptocurrencyMetaData> Data { get; set; }
}

public class CryptocurrencyMetaData
{
    public int DbId { get; set; }

    [JsonPropertyName("id")]
    public int CoinMarketCapId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }

    [JsonPropertyName("subreddit")]
    public string Subreddit { get; set; }

    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; set; }

    [JsonPropertyName("infinite_supply")]
    public bool InfiniteSupply { get; set; }

    [JsonPropertyName("platform")]
    public PlatformData Platform { get; set; }
}

public class PlatformData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}
