using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;

namespace CoinMarketCap.Models
{
    public class ApiResponse
    {
        public ApiErrorCodes Code { get; set; }
        public string Comment { get; set; }
        public List<Param> Params { get; set; }
    }
}
