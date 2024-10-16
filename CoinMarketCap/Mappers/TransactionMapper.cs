using CoinMarketCap.Dtos;
using CoinMarketCap.Models;

namespace CoinMarketCap.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionModel TransactionDtoToModel(this TransactionDto transactionDto, int userId, int metaDateId )
        {
            return new TransactionModel
            {
                Amount = transactionDto.Amount,
                UserId = userId,
                CryptoMetadataId = metaDateId

            };
        }
    }
}
