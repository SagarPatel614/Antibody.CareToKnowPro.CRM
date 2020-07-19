// Decompiled with JetBrains decompiler
// Type: IDCOM.CRM.WebApi.Core.Helpers.StringCipher
// Assembly: IDCOM.CRM.WebApi.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B1295F1-6140-463F-9B4C-79705EA7CA0C
// Assembly location: C:\Learning\Code\wwwroot\IDCOM.CRM.WebApi.Core.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Antibody.CareToKnowPro.CRM.Helpers
{
    public static class StringCipher
    {
        private const int Keysize = 128;
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] salt = StringCipher.Generate128BitsOfRandomEntropy();
            byte[] rgbIV = StringCipher.Generate128BitsOfRandomEntropy();
            byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000))
            {
                byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, rgbIV))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes1, 0, bytes1.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] array = ((IEnumerable<byte>)((IEnumerable<byte>)salt).Concat<byte>((IEnumerable<byte>)rgbIV).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)memoryStream.ToArray()).ToArray<byte>();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(array);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] numArray1 = Convert.FromBase64String(cipherText);
            byte[] array1 = ((IEnumerable<byte>)numArray1).Take<byte>(16).ToArray<byte>();
            byte[] array2 = ((IEnumerable<byte>)numArray1).Skip<byte>(16).Take<byte>(16).ToArray<byte>();
            byte[] array3 = ((IEnumerable<byte>)numArray1).Skip<byte>(32).Take<byte>(numArray1.Length - 32).ToArray<byte>();
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, array1, 1000))
            {
                byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(array3))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] numArray2 = new byte[array3.Length];
                                int count = cryptoStream.Read(numArray2, 0, numArray2.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(numArray2, 0, count);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate128BitsOfRandomEntropy()
        {
            byte[] data = new byte[16];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(data);
            return data;
        }
    }
}