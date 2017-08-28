using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace PasswordKeeperDemo.Helpers
{
    public static class Crypto
    {
        /**/
        private const string HmacSha1KeyMaterial = "password";

        public static string EncryptHmacSha1(string input)
        {
            try
            {
                byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(HmacSha1KeyMaterial, Encoding.Unicode);
                byte[] data = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(input, Encoding.Unicode);
                var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha1);
                CryptographicHash hasher = algorithm.CreateHash(keyMaterial);
                hasher.Append(data);
                byte[] mac = hasher.GetValueAndReset();
                string macBase64 = Convert.ToBase64String(mac);

                return macBase64;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /**/
        private const string AesCbcPkcs7KeyMaterial = "password";

        public static string EncryptAesCbcPkcs7(string input, ref string ivKey)
        {
            try
            {
                byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(AesCbcPkcs7KeyMaterial, Encoding.Unicode);
                byte[] data = UTF8Encoding.UTF8.GetBytes(input); // Convert() Convert.FromBase64String(input); 
                var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
                var key = provider.CreateSymmetricKey(keyMaterial);

                // The IV may be null, but supplying a random IV increases security.
                // The IV is not a secret like the key is.
                // You can transmit the IV (w/o encryption) alongside the ciphertext.
                var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(provider.BlockLength);
                var _iv = Convert.ToBase64String(iv);
                ivKey = _iv;

                byte[] cipherText = WinRTCrypto.CryptographicEngine.Encrypt(key, data, iv);
                var _cipherText = Convert.ToBase64String(cipherText);

                return _cipherText;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string EncryptAesCbcPkcs7(string input, string ivKey)
        {
            try
            {
                byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(AesCbcPkcs7KeyMaterial, Encoding.Unicode);
                byte[] data = UTF8Encoding.UTF8.GetBytes(input); // Convert() Convert.FromBase64String(input); 
                var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
                var key = provider.CreateSymmetricKey(keyMaterial);

                // The IV may be null, but supplying a random IV increases security.
                // The IV is not a secret like the key is.
                // You can transmit the IV (w/o encryption) alongside the ciphertext.
                var iv = Convert.FromBase64String(ivKey);

                byte[] cipherText = WinRTCrypto.CryptographicEngine.Encrypt(key, data, iv);
                var _cipherText = Convert.ToBase64String(cipherText);

                return _cipherText;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string DecryptAesCbcPkcs7(string input, string ivKey)
        {
            try
            {
                byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(AesCbcPkcs7KeyMaterial, Encoding.Unicode);
                var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
                var key = provider.CreateSymmetricKey(keyMaterial);

                byte[] plainText2 = WinRTCrypto.CryptographicEngine.Decrypt(key, Convert.FromBase64String(input), Convert.FromBase64String(ivKey));
                var _plainText2 = UTF8Encoding.UTF8.GetString(plainText2, 0, plainText2.Length); // Convert.ToBase64String(plainText2);

                return _plainText2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
