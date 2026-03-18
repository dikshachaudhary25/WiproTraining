using System;
using System.Security.Cryptography;
using System.Text;

namespace SecureUserApp
{
    public class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public User(string username, string password)
        {
            Username = username;
            HashedPassword = HashPassword(password);
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
        public bool Authenticate(string password)
        {
            string hashedInput = HashPassword(password);
            return HashedPassword == hashedInput;
        }
    }
}