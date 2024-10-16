namespace CoinMarketCap.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public string CoinSymbol { get; set; }
        public decimal Amount { get; set; }
        public decimal TimeOfPurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int UserId { get; set; }
        public int CryptoMetadataId { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
