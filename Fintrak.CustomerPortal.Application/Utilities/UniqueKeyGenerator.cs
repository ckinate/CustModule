using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Application.Utilities
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public static class UniqueKeyGenerator
    {
        public static string AlphaNumericRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var dictionaryLenght = 62;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string AlphaRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var dictionaryLenght = 52;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string AlphaNumericCapRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var dictionaryLenght = 36;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string AlphaCapRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var dictionaryLenght = 26;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string AlphaSmallRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "abcdefghijklmnopqrstuvwxyz";
            var dictionaryLenght = 26;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string NumericRNGCharacterMask(int minSize = 7, int maxSize = 8)
        {
            var dictionary = "1234567890";
            var dictionaryLenght = 10;

            return RunKeyGen(minSize, maxSize, dictionary, dictionaryLenght);
        }

        public static string RunKeyGen(int minSize, int maxSize, string dictionary, int dictionaryLenght)
        {
            char[] chars = new char[dictionaryLenght];
            chars = dictionary.ToCharArray();

            int size = maxSize;
            byte[] data = new byte[1];

            //RandomNumberGenerator crypto = new RandomNumberGenerator();
            var crypto = RandomNumberGenerator.Create();
            crypto.GetBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetBytes(data);

            StringBuilder result = new StringBuilder(size);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }

        public static string GetUniqueToken(int length, string chars1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", string chars2 = @"()`~!@#$%^&*-+=|\{}[]:;'<>,.?/_")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                string chars = chars1 + chars2;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                var finalResult = new string(result);

                return finalResult;
            }
        }

        public static int GetRandomInteger(int min = 0, int max = int.MaxValue)
        {
            if (max <= min)
            {
                throw new ArgumentOutOfRangeException("Maximum value must be greater than minimum value");
            }

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                uint scale = uint.MaxValue;
                while (scale == uint.MaxValue)
                {
                    // Get four random bytes.
                    byte[] fourBytes = new byte[4];
                    crypto.GetBytes(fourBytes);

                    // Convert that into an uint.
                    scale = BitConverter.ToUInt32(fourBytes, 0);
                }

                // Add min to the scaled difference between max and min.
                return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
            }
        }
    }
}