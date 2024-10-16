using CoinMarketCap.Models;
using System.Net;
using log4net;
using FluentValidation;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Helpers;
using Newtonsoft.Json;

namespace CoinMarketCap.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly ILog _log;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
            _log = LogManager.GetLogger(typeof(GlobalExceptionHandlingMiddleware));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse
                {
                    Code = ApiErrorCodes.FailedCode,
                    Comment = ErrorExtractor.ExtractErrorMessage(ex.Message)
                }));
            }
            catch (Exception ex)
            {
                _log.Error("GlobalExceptionHandlingMiddleware.InvokeAsync exception occured :" + ex);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }

}
