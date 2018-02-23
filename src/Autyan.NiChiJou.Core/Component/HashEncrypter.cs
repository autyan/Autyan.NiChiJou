using System;
using System.Security.Cryptography;
using System.Text;

namespace Autyan.NiChiJou.Core.Component
{
    public static class HashEncrypter
    {
        public static string Sha256Encrypt(string phrase, string salt = null)
        {
            if (salt == null)
            {
                salt = string.Empty;
            }
            var saltAndPwd = string.Concat(phrase, salt);
            var encoder = new UTF8Encoding();
            using (var sha256Hasher = new SHA256Managed())
            {
                var hashedDataBytes = sha256Hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
                return Convert.ToBase64String(hashedDataBytes);
            }
        }

        public static byte[] Hmacsha256Encrypt(byte[] source)
        {
            using (var hmac = new HMACSHA256())
            {
                return hmac.ComputeHash(source);
            }
        }

        public static string Hmacsha256EncryptToBase64(byte[] source)
        {
            var hash = Hmacsha256Encrypt(source);
            return Convert.ToBase64String(hash);
        }

        public static byte[] Hmacsha256Encrypt(byte[] secret, byte[] source)
        {
            using (var hmac = new HMACSHA256(secret))
            {
                return hmac.ComputeHash(source);
            }
        }

        public static string Hmacsha256EncryptToBase64(byte[] secret, byte[] source)
        {
            var hash = Hmacsha256Encrypt(secret, source);
            return Convert.ToBase64String(hash);
        }

        public static string Md5EncryptToBase64(string phrase)
        {
            var md5Bytes = Md5Encrypt(phrase);
            return Convert.ToBase64String(md5Bytes);
        }

        public static byte[] Md5Encrypt(string phrase)
        {
            var md5Hash = MD5.Create();
            return md5Hash.ComputeHash(Encoding.UTF8.GetBytes(phrase));
        }
    }
}
