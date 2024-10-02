using CoinMarketCap.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinMarketCap.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/WeatherForecast")]
    public class CoinMarketCapController : ControllerBase
    {
       private CoinMarketCapService _service;

        public CoinMarketCapController(CoinMarketCapService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<string> Get() 
        {
            return await _service.GetCryptocurrencyQuoteAsync();
        }
    }
}
