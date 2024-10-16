using CoinMarketCap.Models.Enums;

namespace CoinMarketCap.Helpers
{
    public class Comment
    {
        private readonly IConfiguration _configuration;

        public string Success = string.Empty;
        public string Error = string.Empty;
        public string PleaseTryLaiter = string.Empty;

        //Transactions
        public string InvalidUserId = string.Empty;
        public string CoinNotFound = string.Empty;
        public string CoinDataNotFound = string.Empty;
        public string TransactionFailed = string.Empty;
        public string CoinNotSelected = string.Empty;
        public string AmountNotConfirmed = string.Empty;

        //CoinMarketCapService
        public string FailedRecordToDb = string.Empty;

        //IdentityService
        public string InvalidLoginOrPassword = string.Empty;

        //UserValidator
        public string InvalidLogin = string.Empty;
        public string InvalidPassword = string.Empty;



        public Comment(Lang LNG = Lang.RU)
        {
            if (LNG == Lang.TJ)
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/tj.json").Build();

            else if(LNG == Lang.RU)
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/ru.json").Build();
            else
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/en.json").Build();


            Success = _configuration.GetSection("LNG")["Success"];
            Error = _configuration.GetSection("LNG")["Error"];
            PleaseTryLaiter = _configuration.GetSection("LNG")["PleaseTryLaiter"];

            InvalidUserId = _configuration.GetSection("LNG")["InvalidUserId"];
            CoinNotFound = _configuration.GetSection("LNG")["CoinNotFound"];
            CoinDataNotFound = _configuration.GetSection("LNG")["CoinDataNotFound"];
            TransactionFailed = _configuration.GetSection("LNG")["TransactionFailed"];
            CoinNotSelected = _configuration.GetSection("LNG")["CoinNotSelected"];
            AmountNotConfirmed = _configuration.GetSection("LNG")["AmountNotConfirmed"];

            FailedRecordToDb = _configuration.GetSection("LNG")["FailedRecordAtDb"];

            InvalidLoginOrPassword = _configuration.GetSection("LNG")["InvalidLoginOrPassword"];

            InvalidLogin = _configuration.GetSection("LNG")["InvalidLogin"];
            InvalidPassword = _configuration.GetSection("LNG")["InvalidPassword"];

        }
    }
}
