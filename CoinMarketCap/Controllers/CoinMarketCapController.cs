using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/CoinMarketCap")]
    public class CoinMarketCapController : ControllerBase
    {
       private ICoinMarketCapService _service;

        public CoinMarketCapController(ICoinMarketCapService service)
        {
            _service = service;
        }

        [HttpGet("update-cryptocurrency-database")]
        public async Task<ApiResponse> UpdateCryptocurrencyQuoteBaseAsync(CancellationToken token = default) 
            => await _service.UpdateCryptocurrencyQuoteBaseAsync(token);

        [HttpGet("get-cryptocurrencies")]
        public async Task<List<CryptocurrencyData>> GetAllCryptocurrenciesAsync(string? search, int page = 1, CancellationToken token = default)
            => await _service.GetAllCryptocurrenciesAsync(page, search, token);

        [HttpGet("get-metadata")]
        [Authorize]
        public async Task<CryptocurrencyMetaData?> GetMetadataAsync(CoinSymbols symbol, CancellationToken token = default)
            => await _service.GetMetadataAsync(symbol, token);
    }
}
