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
        public string InvalidRegisLogin = string.Empty;
        public string InvalidRegisPassword = string.Empty;



        public Comment()
        {
            string lang = Thread.CurrentThread.CurrentCulture.Name.ToUpperInvariant();

            if (string.IsNullOrEmpty(lang))
                lang = Lang.RU.ToString();

            lang = lang.Substring(0, 2);

            if (string.Equals(lang, Lang.RU.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/ru.json").Build();
            }
            else if (string.Equals(lang, Lang.TJ.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/tj.json").Build();
            }
            else if (string.Equals(lang, Lang.EN.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("Language/en.json").Build();
            }


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
            InvalidRegisLogin = _configuration.GetSection("LNG")["InvalidRegisLogin"];
            InvalidRegisPassword = _configuration.GetSection("LNG")["InvalidRegisPassword"];

        }
    }
}




//{
//    "LNG": {
//        "Success": "Succes.",
//    "Error": "Error.",
//    "PleaseTryLater": "PleaseTryLater.",

//    "InvalidUserId": "Invalid user id.",
//    "CoinNotFound": "Coin not found.",
//    "CoinDataNotFound": "Coin data not found.",
//    "TransactionFailed": "Transaction failed.",
//    "CoinNotSelected": "Coin not selected.",
//    "AmountNotConfirmed": "Amount not confirmed.",

//    "FailedRecordAtDb": "Failed to record at data base.",

//    "InvalidLoginOrPassword": "Invalid login or password.",

//    "InvalidLogin": "The login must contain only English letters and numbers, without spaces or special characters.",
//    "InvalidPassword": "The password cannot be empty and must contain at least 5 characters.",
//    "InvalidRegisLogin": "The login cannot be empty and cannot exceed 35 characters.",
//    "InvalidRegisPassword": "The password must contain a minimum of 6 characters and a maximum of 10."
  
//  }
//}
