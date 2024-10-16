using CoinMarketCap.Models;

namespace CoinMarketCap.Dtos
{
    public class BalanceResponse
    {
        public string Balance { get; set; }
        public PaginationViewResponse<TransactionModel>? Pagination { get; set; }
    }
}
