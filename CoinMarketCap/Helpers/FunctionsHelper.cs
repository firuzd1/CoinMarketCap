using System.Security.Cryptography;
using System.Text;

namespace CoinMarketCap.Helpers
{
    public class FunctionsHelper
    {
        public string GetSHA1String(string input)
        {
            string sign = "";
            using (SHA1 md5Hash = SHA1.Create())
            {
                sign = GetSHA1Hash(md5Hash, input);
            }
            return sign;
        }

        public string GetSHA1Hash(SHA1 sha1Hash, string input)
        {
            byte[] data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
