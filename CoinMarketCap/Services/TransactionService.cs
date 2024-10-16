using CoinMarketCap.Dtos;
using CoinMarketCap.Mappers;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoinMarketCap.Models.Enums;

namespace CoinMarketCap.Services
{
    public class TransactionService
    {
        private TransactionRepository _transactionRepository;
        private CoinMarketCapRepository _coinMarketCapRepository;
        private readonly int pageSize = 10;

        public TransactionService(TransactionRepository transactionRepository, CoinMarketCapRepository coinMarketCapRepository)
        {
            _transactionRepository = transactionRepository;
            _coinMarketCapRepository = coinMarketCapRepository;
        }

        public async Task<ApiResponse> TransactionSimulationAsync(CoinSymbols coinSymbols, TransactionDto transactionDto, int userId, CancellationToken token = default)
        {
            ApiResponse apiResponse = new();

            apiResponse.Code = ApiErrorCodes.FailedCode;

            if(userId <= 0) 
            {
                apiResponse.Comment = "id пользователя не валидно.";
                return apiResponse;
            }

            CryptocurrencyMetaData? metaData = await _coinMarketCapRepository.GetMetaDataFromDbAsync(coinSymbols.ToString());
            
            if (metaData == null) 
            {
                apiResponse.Comment = "Не удалось найти валюту";
                return apiResponse;
            }

            decimal cryptocurrencyPrice = await _transactionRepository.GetCurrentExchangeRateBySymbolAsync(metaData.Symbol);

            if (cryptocurrencyPrice <= 0)
            {
                apiResponse.Comment = "Данные по валюте не найдены.";
                return apiResponse;
            }


            TransactionModel transactionModel = transactionDto.TransactionDtoToModel(userId, metaData.DbId);

            transactionModel.TimeOfPurchasePrice = transactionModel.Amount * cryptocurrencyPrice;

            int result = await _transactionRepository.TransactionSimulationAsync(transactionModel, token);

            if(result <= 0)
            {
                apiResponse.Comment = "транзакция не выполнена.";
                return apiResponse;
            }

            apiResponse.Code = ApiErrorCodes.SuccessCode;
            apiResponse.Comment = "успешно";
            return apiResponse;
        }

        public async Task<BalanceResponse> CheckBalanceAsync(int userId, string? search, int page, CancellationToken token = default)
        {
            PaginationViewResponse<TransactionModel>? paginationViewResponse = new();
            decimal balance = 0;

            int limit = pageSize;
            int skip = page == 1 ? 0 : (page - 1) * limit;

            List<TransactionModel>? transactions = await _transactionRepository.CheckBalanceAsync(userId, search, skip, limit, token);

            foreach (TransactionModel transaction in transactions)
            {
                balance += transaction.CurrentPrice;
            }

            int transactionCount = await _transactionRepository.GetCTransactionCountAsync(search, userId, token);

            paginationViewResponse.Enttities = transactions;
            paginationViewResponse.CountPage = (int)Math.Ceiling(transactionCount / (decimal)pageSize);
            paginationViewResponse.CurrentPage = page;

            return(new BalanceResponse
            {
                Pagination = paginationViewResponse,
                Balance = "$"+balance.ToString(),
            });
        }
    }
}
