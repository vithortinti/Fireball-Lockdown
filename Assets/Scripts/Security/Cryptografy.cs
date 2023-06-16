using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class Cryptografy
{
    private static string _key = "1f5s1fsfs##fds4s@16663@fsdfsas77a21fsçççç";

    public static string Encrypt(string s)
    {
        using (var md5 = new MD5CryptoServiceProvider())
        {
            using (var tdcsp = new TripleDESCryptoServiceProvider())
            {
                tdcsp.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(_key));
                tdcsp.Mode = CipherMode.ECB;
                tdcsp.Padding = PaddingMode.PKCS7;

                using (var encrypt = tdcsp.CreateEncryptor())
                {
                    byte[] stringBytes = Encoding.UTF8.GetBytes(s);
                    byte[] bytes = encrypt.TransformFinalBlock(stringBytes, 0, stringBytes.Length);
                    return Convert.ToBase64String(bytes, 0, bytes.Length);
                }
            }
        }
    }

    public static string Decrypt(string s)
    {
        using (var md5 = new MD5CryptoServiceProvider())
        {
            using (var tdcsp = new TripleDESCryptoServiceProvider())
            {
                tdcsp.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(_key));
                tdcsp.Mode = CipherMode.ECB;
                tdcsp.Padding = PaddingMode.PKCS7;

                using (var decrypt = tdcsp.CreateDecryptor())
                {
                    byte[] stringBytes = Convert.FromBase64String(s);
                    byte[] bytes = decrypt.TransformFinalBlock(stringBytes, 0, stringBytes.Length);
                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }
    }
}
