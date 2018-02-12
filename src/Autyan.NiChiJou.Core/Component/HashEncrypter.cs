﻿using System;
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

        public static string Md5Encrypt(string phrase)
        {
            var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(phrase));
            return Convert.ToBase64String(data);
        }
    }
}
