namespace CoinMarketCap.Models.SettingsModels
{
    public class JwtSettingsModel
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenLifeTime { get; set; }
    }
}
