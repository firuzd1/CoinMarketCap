using CoinMarketCap.Models.Enums;

namespace CoinMarketCap.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Lang Lang { get; set; }
    }
}
