using System.Text.RegularExpressions;

namespace CoinMarketCap.Helpers;

public class ErrorExtractor
{
    public static string ExtractErrorMessage(string input)
    {
        string pattern = @": (.*?) Severity";

        Match match = Regex.Match(input, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        return string.Empty;
    }
}