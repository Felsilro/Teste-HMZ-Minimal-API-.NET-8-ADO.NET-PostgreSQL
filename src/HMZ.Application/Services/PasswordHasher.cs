using System.Security.Cryptography;

namespace HMZ.Application.Services;

public sealed class PasswordHasher
{
    public bool Verify(string password, string saltHex, string hashHex, int iterations)
    {
        var salt = Convert.FromHexString(saltHex);
        var computed = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(computed, Convert.FromHexString(hashHex));
    }
}
