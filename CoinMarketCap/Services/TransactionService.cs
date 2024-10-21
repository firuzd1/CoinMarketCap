using CoinMarketCap.Dtos;
using CoinMarketCap.Mappers;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Helpers;

namespace CoinMarketCap.Services
{
    public class TransactionService
    {
        private TransactionRepository _transactionRepository;
        private CoinMarketCapRepository _coinMarketCapRepository;
        private readonly Comment _comment;
        private readonly int pageSize = 10;

        public TransactionService(TransactionRepository transactionRepository, CoinMarketCapRepository coinMarketCapRepository, Comment comment)
        {
            _transactionRepository = transactionRepository;
            _coinMarketCapRepository = coinMarketCapRepository;
            _comment = comment;
        }

        public async Task<ApiResponse> TransactionSimulationAsync(CoinSymbols? coinSymbols, TransactionDto transactionDto, int userId, CancellationToken token = default)
        {
            ApiResponse apiResponse = new();

            apiResponse.Code = ApiErrorCodes.FailedCode;

            if(userId <= 0) 
            {
                apiResponse.Comment = _comment.InvalidUserId;
                return apiResponse;
            }

            if(coinSymbols == null)
            {
                apiResponse.Comment = _comment.CoinNotSelected;
                return apiResponse;
            }

            if(transactionDto.Amount <= 0)
            {
                apiResponse.Comment = _comment.AmountNotConfirmed;
                return apiResponse;
            }

            CryptocurrencyMetaData? metaData = await _coinMarketCapRepository.GetMetaDataFromDbAsync(coinSymbols.ToString());
            
            if (metaData == null) 
            {
                apiResponse.Comment = _comment.CoinNotFound;
                return apiResponse;
            }

            decimal cryptocurrencyPrice = await _transactionRepository.GetCurrentExchangeRateBySymbolAsync(metaData.Symbol);

            if (cryptocurrencyPrice <= 0)
            {
                apiResponse.Comment = _comment.CoinDataNotFound;
                return apiResponse;
            }


            TransactionModel transactionModel = transactionDto.TransactionDtoToModel(userId, metaData.DbId);

            transactionModel.TimeOfPurchasePrice = transactionModel.Amount * cryptocurrencyPrice;

            int result = await _transactionRepository.TransactionSimulationAsync(transactionModel, token);

            if(result <= 0)
            {
                apiResponse.Comment = _comment.TransactionFailed;
                return apiResponse;
            }

            apiResponse.Code = ApiErrorCodes.SuccessCode;
            apiResponse.Comment = _comment.Success;
            return apiResponse;
        }

        public async Task<BalanceResponse> CheckBalanceAsync(int userId, string? search, int page, CancellationToken token = default)
        {
            PaginationViewResponse<TransactionModel>? paginationViewResponse = new();
            decimal balance = 0;

            int limit = pageSize;
            int skip = page == 1 ? 0 : (page - 1) * limit;

            List<TransactionModel>? transactions = await _transactionRepository.GetTransactionAsync(userId, search, skip, limit, token);

            balance = await _transactionRepository.CheckBalanceAsync(userId, token);

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
