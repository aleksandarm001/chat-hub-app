using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            StringBuilder hashStringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashStringBuilder.Append(b.ToString("x2")); 
            }

            return hashStringBuilder.ToString();
        }
    }
    public static bool VerifyHash(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}