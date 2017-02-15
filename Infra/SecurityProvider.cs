using System;
using System.Security.Cryptography;
using System.Text;
using OwnAspNetCore.Services;

namespace OwnAspNetCore.Infra
{
    public class SecurityProvider : ISecurity
    {
        public string GenerateSalt()
        {
            var guid = Guid.NewGuid().ToString();
            return HashToString(guid);
        }

        public byte[] Hash(string source)
        {
            var sha512 = SHA512.Create();
            return sha512.ComputeHash(Encoding.UTF8.GetBytes(source));
        }

        public string HashToString(string source)
        {
            return HashToString(source, string.Empty);
        }

        public string HashToString(string source, string salt)
        {
            string target = source + salt;
            return Convert.ToBase64String(Hash(target));
        }

        public bool Validate(string hash, string password, string salt)
        {
            return hash == HashToString(password, salt);
        }
    }
}