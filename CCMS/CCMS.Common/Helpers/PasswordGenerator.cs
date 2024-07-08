using System.Security.Cryptography;
using System.Text;

namespace CCMS.Common.Helpers
{
    public static class PasswordGenerator
    {
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DigitChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()_-+=<>?{}[]|";
        private const string AllValidChars = LowercaseChars + UppercaseChars + DigitChars + SpecialChars;
        private static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static string GeneratePassword()
        {
            int maxLength = 10;
            StringBuilder password = new StringBuilder(maxLength);

            // Ensure the password contains at least one character from each required category
            password.Append(GetRandomChar(LowercaseChars));
            password.Append(GetRandomChar(UppercaseChars));
            password.Append(GetRandomChar(DigitChars));
            password.Append(GetRandomChar(SpecialChars));

            // Fill the rest of the password length with random characters from all valid characters
            for (int i = 4; i < maxLength; i++)
            {
                password.Append(GetRandomChar(AllValidChars));
            }

            // Shuffle the characters in the password to ensure they are not in a predictable pattern
            return ShufflePassword(password.ToString());
        }

        private static char GetRandomChar(string validChars)
        {
            byte[] randomData = new byte[1];
            rngCsp.GetBytes(randomData);
            int randomIndex = randomData[0] % validChars.Length;
            return validChars[randomIndex];
        }

        private static string ShufflePassword(string password)
        {
            char[] array = password.ToCharArray();
            byte[] randomData = new byte[array.Length];
            rngCsp.GetBytes(randomData);
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = randomData[i] % (i + 1);
                char temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
            return new string(array);
        }
    }
}
