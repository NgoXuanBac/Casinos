using System.Security.Cryptography;
using System.Text;
using Identity.Application.Interfaces.Security;

namespace Identity.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var salt = Guid.NewGuid().ToByteArray();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(salt.Concat(passwordBytes).ToArray());
            return Convert.ToBase64String(salt.Concat(hash).ToArray());
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = hashBytes.Take(16).ToArray();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var actualHash = SHA256.HashData(salt.Concat(passwordBytes).ToArray());
            return actualHash.SequenceEqual(hashBytes.Skip(16));
        }
    }
}