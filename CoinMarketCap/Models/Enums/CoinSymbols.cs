using System.Text.Json.Serialization;

namespace CoinMarketCap.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CoinSymbols
    {
        BTC,
        ETH,
        USDT,
        BNB,
        SOL,
        USDC,
        XRP
    }
}
