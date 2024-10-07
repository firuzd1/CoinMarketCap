namespace CoinMarketCap.Dtos
{
    public class PaginationViewResponse<T>
    {
        public int CurrentPage { get; set; }
        public int CountPage { get; set; }
        public List<T> Enttities { get; set; }
    }
}
