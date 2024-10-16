using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private TransactionService _transactionService;
        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("simulate_transaction")]
        [Authorize]
        public async Task<ApiResponse> TransactionSimulationAsync(Lang lang, CoinSymbols? coinSymbol, TransactionDto transactionDto, CancellationToken token = default)
        {
            int UserId = int.Parse(HttpContext.User.FindFirstValue("UserId"));

            return await _transactionService.TransactionSimulationAsync(coinSymbol, transactionDto, UserId, lang, token);
        }

        [HttpGet("check-account_balance")]
        public async Task<BalanceResponse> CheckBalanceAsync(string? search, int page = 1, CancellationToken token = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue("UserId"));
            return await _transactionService.CheckBalanceAsync(userId, search, page, token);
        }
    }
}
