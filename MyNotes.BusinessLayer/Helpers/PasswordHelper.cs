using System;
using System.Security.Cryptography;

namespace MyNotes.BusinessLayer.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Salt üret
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(32);

            // salt.hash formatında sakla
            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] computedHash = pbkdf2.GetBytes(32);

            return SlowEquals(hash, computedHash);
        }

        // FixedTimeEquals yerine kendi güvenli karşılaştırmamız
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);

            return diff == 0;
        }
    }
}

