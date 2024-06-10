using System.Security.Cryptography;

namespace CCMS.Common.Helpers
{
    public static class PasswordGenerator
    {
        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public static string GeneratePassword()
        {
            int maxLength = 10;
            char[] chars = new char[maxLength];
            byte[] randomData = new byte[maxLength];
            rngCsp.GetBytes(randomData);

            for (int i = 0; i < maxLength; i++)
            {
                // Ensure the generated character is within the valid character set
                int randomNumber = randomData[i] % ValidChars.Length;
                chars[i] = ValidChars[randomNumber];
            }

            return new string(chars);
        }
    }
}
