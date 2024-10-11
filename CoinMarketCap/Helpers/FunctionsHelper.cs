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

        public string GenerateUniqueLoginId()
        {
            Random random = new Random();
            int min = 100000;
            int max = 999999;

            int randomNumber = random.Next(min, max + 1);

            return randomNumber.ToString();
        }

        public string ComputeMd5Hash()
        {
            string hashText = "GrEen" + DateTime.Now.ToString("yyyyddMMHH-mmssfff") + "PoWer";
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(hashText);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
