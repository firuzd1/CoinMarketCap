using CoinMarketCap.Models;
using Dapper;
using System.Data;

namespace CoinMarketCap.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        public Task<int> TransactionSimulationAsync(TransactionModel transaction, CancellationToken token = default);
        public Task<decimal> GetCurrentExchangeRateBySymbolAsync(string symbol, CancellationToken token = default);
        public Task<List<TransactionModel>> GetTransactionAsync(int userId, string? search, int skip, int limit, CancellationToken token = default);
        public Task<int> GetCTransactionCountAsync(string? search, int userId, CancellationToken token);
        public  Task<decimal> CheckBalanceAsync(int userId, CancellationToken token = default);
    }
}
