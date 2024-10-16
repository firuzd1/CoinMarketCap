using System.Text.Json.Serialization;

namespace CoinMarketCap.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Lang : short
{
    TJ = 1,
    RU = 2,
    EN = 3
}