using System.Globalization;
using System.Security.Claims;
using log4net;

namespace CoinMarketCap.Middlewares;

public class LanguageMiddleware
{
    private readonly ILog _log;
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
        _log = LogManager.GetLogger(typeof(LanguageMiddleware));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var language = context.User.FindFirstValue("Lang");
        if (language is not null)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        }
        await _next(context);
     }
}