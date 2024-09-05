using CCMS.Common.Helpers;

namespace CCMS.FE.UI.Extensions
{
    public static class EncryptionAndDecryption
    {
        private const string Key = "Rte2bR+DY77y9aEuAPenVVCLkPSN55rTGyA+swfhykA=";
        private const string KeyIVBase64 = "MwND80W9V8urVLhK+iRKzQ==";

        public static string Encrypt(string input)
        {
            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var encryptedText = symmetricEncryptDecrypt.Encrypt(input, KeyIVBase64, Key);
            return encryptedText;
        }

        public static string Decrypt(string encryptedText)
        {
            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var decryptedText = symmetricEncryptDecrypt.Decrypt(encryptedText, KeyIVBase64, Key);
            return decryptedText;
        }
    }

}
