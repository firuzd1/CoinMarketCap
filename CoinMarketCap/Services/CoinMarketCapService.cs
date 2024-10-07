using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Providers;
using CoinMarketCap.Repositories;
using PowerBankSystem.Models.Enums;
using System.Text.Json;

namespace CoinMarketCap.Services
{
    public class CoinMarketCapService
    {
        private CoinMarketCapProvider _provider;
        private CoinMarketCapRepository _repository;
        public CoinMarketCapService(CoinMarketCapProvider provider, CoinMarketCapRepository repository)
        {
            _repository = repository;
            _provider = provider;
        }

        public async Task<ApiResponse> UpdateCryptocurrencyQuoteBaseAsync(CancellationToken token = default)
        {
            ApiResponse _response = new();
            _response.Code = ApiErrorCodes.FailedCode;

            string stringResponse = await _provider.GetCryptocurrencyQuoteAsync(token);

            CryptocurrencyResponse? response = JsonSerializer.Deserialize<CryptocurrencyResponse>(stringResponse);
            
            int result = await _repository.CryptocurrencyQuoteAddAsync(response, token);
            
            if(result <= 0) 
            {
                _response.Comment = "не удалось добавить запись в базу";
                return _response;
            }

            _response.Code = ApiErrorCodes.SuccessCode;
            _response.Comment = "success";
            return _response;
        }
    }
}
