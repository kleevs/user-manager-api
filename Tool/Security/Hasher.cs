using System.Security.Cryptography;
using System.Text;

namespace Tool
{
    public class Hasher
    {
        private SHA256 _SHA256;
        public Hasher()
        {
            _SHA256 = SHA256.Create();
        }

        public string Compute(string text)
        {
            return System.Convert.ToBase64String(_SHA256.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
