using CoinMarketCap.Dtos;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models;
using CoinMarketCap.Repositories;

namespace CoinMarketCap.Interfaces.Services
{
    public interface ITransactionService
    {
        public Task<ApiResponse> TransactionSimulationAsync(CoinSymbols? coinSymbols, TransactionDto transactionDto, int userId, CancellationToken token = default);
        public Task<BalanceResponse> CheckBalanceAsync(int userId, string? search, int page, CancellationToken token = default);
    }
}
