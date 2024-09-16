using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Core.Helpers
{
    public static class HashingHelper
    {
        public static void CreateHashPassword(string password, out byte[] passowordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be empty or white space");
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passowordHash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
        }
        public static Boolean VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be empty or white space");
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected) ", nameof(storedHash));
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (64 bytes expected) ", nameof(storedSalt));
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
            return !storedHash.Where((t, i) => computedHash[i] != t).Any();
        }
    }
}
