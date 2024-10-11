using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Providers;
using CoinMarketCap.Repositories;
using Microsoft.AspNetCore.Builder.Extensions;
using PowerBankSystem.Models.Enums;
using System.Text.Json;

namespace CoinMarketCap.Services
{
    public class CoinMarketCapService
    {
        private CoinMarketCapProvider _provider;
        private CoinMarketCapRepository _repository;
        private readonly int ItemsOfPage = 20;
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

        public async Task<CryptocurrencyMetaData?> GetMetadataAsync(CoinSymbols symbol, CancellationToken token = default)
        {
            ApiResponse _response = new();
            _response.Code = ApiErrorCodes.FailedCode;

            CryptocurrencyMetaData? metaData = await _repository.GetMetaDataFromDbAsync(symbol.ToString(), token);
            if(metaData == null)
            {
                string metadata = await _provider.GetMetadataAsync(symbol.ToString(), token);

                CryptocurrencyMetaDataResponse? response = JsonSerializer.Deserialize<CryptocurrencyMetaDataResponse>(metadata);
                int repResponse = await _repository.AddCryptoMetadataAsync(response, token);

                if (repResponse > 0)
                {
                   return await _repository.GetMetaDataFromDbAsync(symbol.ToString(), token);
                    
                }
            }
            return metaData;
        }

        public async Task<List<CryptocurrencyData>> GetAllCryptocurrenciesAsync(int page, CancellationToken token = default)
        {
            if (page > 1)
            {
                page = ((page - 1) * ItemsOfPage);
            }
            else
            {
                page = 0;
            }
            return await _repository.GetAllCryptocurrenciesAsync(page, ItemsOfPage, token);
        }
    }
}
