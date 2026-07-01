using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.HashTools;

public enum EncodeType : byte
{
    None,
    SHA1,
    SHA256,
    MD5,
}

public class StringHashTool
{
    public static string Signature()
    {
        byte[] array = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(array);
    }

    public static string Encode(string pwd, EncodeType encodeType, string sign)
    {
        if (encodeType == EncodeType.None) { return pwd; }

        byte[] pwd_bytes = Encoding.UTF8.GetBytes(pwd);
        byte[] sign_bytes = Convert.FromBase64String(sign);

        byte[] inArray = new byte[pwd_bytes.Length + sign_bytes.Length];

        pwd_bytes.CopyTo(inArray, 0);
        sign_bytes.CopyTo(inArray, pwd_bytes.Length);

        var hashAlgorithm = HashAlgorithm.Create(encodeType.ToString());
        if (hashAlgorithm != null)
        {
            byte[] outArray = hashAlgorithm.ComputeHash(inArray);
            return Convert.ToBase64String(outArray);
        }
        else { return pwd; }
    }

    public static string Encode(string str, EncodeType encodeType)
    {
        if (encodeType == EncodeType.None) { return str; }

        byte[] inArray = Encoding.UTF8.GetBytes(str);

        var hashAlgorithm = HashAlgorithm.Create(encodeType.ToString());
        if (hashAlgorithm != null)
        {
            byte[] outArray = hashAlgorithm.ComputeHash(inArray);
            return Convert.ToBase64String(outArray);
        }
        else { return str; }
    }
}