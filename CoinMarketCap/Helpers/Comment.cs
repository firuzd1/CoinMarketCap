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




//{
//    "LNG": {
//        "Success": "бомуваффакият.",
//    "Error": "Хатогӣ.",
//    "PleaseTryLater": "Лутфан, баъдтар бори дигар кӯшиш кунед.",

//    "InvalidUserId": "ID корбар эътибор надорад.",
//    "CoinNotFound": "Валюта ёфт нашуд.",
//    "CoinDataNotFound": "Маълумот оид ба асъор ёфт нашуд.",
//    "TransactionFailed": "Амалиёт муваффақ нашуд.",
//    "CoinNotSelected": "ягон асъор интихоб нашудааст.",
//    "AmountNotConfirmed": "шумораи тангаҳо интихоб нашудааст.",


//    "FailedRecordAtDb": "Илова кардани сабт ба пойгоҳи додаҳо муяссар нашуд.",

//    "InvalidLoginOrPassword": "Логин ё рамзи нодуруст.",

//    "InvalidLogin": "Логин бояд танҳо ҳарфҳо ва рақамҳои англисӣ дошта бошад, бидуни фосила ё аломатҳои махсус.",
//    "InvalidPassword": "Рамз холӣ буда наметавонад ва бояд ҳадди аққал 5 аломат дошта бошад.",
//    "InvalidRegisLogin": "Логин холӣ буда наметавонад ва набояд аз 35 аломат зиёд бошад.",
//    "InvalidRegisPassword": "Парол бояд ҳадди аққал 6 аломат ва ҳадди аксар 10 аломат дошта бошад."
//    }
//}



//{
//    "LNG": {
//        "Success": "Успешно.",
//    "Error": "Ошибка.",
//    "PleaseTryLater": "Повторите позже.",

//    "InvalidUserId": "ID пользователя не валиден.",
//    "CoinNotFound": "Не удалось найти валюту.",
//    "CoinDataNotFound": "Данные по валюте не найдены.",
//    "TransactionFailed": "Транзакция не выполнена.",
//    "CoinNotSelected": "вылюта не выбрана.",
//    "AmountNotConfirmed": "не выбрана количество монет.",


//    "FailedRecordAtDb": "Не удалось добавить запись в базу данных.",

//    "InvalidLoginOrPassword": "Неверный логин или пароль.",

//    "InvalidLogin": "Логин должен содержать только буквы английского алфавита и цифры, без пробелов и специальных символов.",
//    "InvalidPassword": "Пароль не может быть пустым и должен содержать не менее 5 символов.",
//    "InvalidRegisLogin": "Логин не сожет быть пустым и не превышать 35 символов.",
//    "InvalidRegisPassword": "Пароль должен содержать минимум 6 символов и максимум 10."
//    }
//}
