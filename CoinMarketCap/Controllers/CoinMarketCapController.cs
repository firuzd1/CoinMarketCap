using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
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
       private CoinMarketCapService _service;

        public CoinMarketCapController(CoinMarketCapService service)
        {
            _service = service;
        }

        [HttpGet("update-cryptocurrency-database")]
        public async Task<ApiResponse> UpdateCryptocurrencyQuoteBaseAsync(CancellationToken token = default) 
            => await _service.UpdateCryptocurrencyQuoteBaseAsync(token);
    }
}
