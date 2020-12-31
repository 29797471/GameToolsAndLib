
using System.Globalization;
using System;
//这个是使用DES的基础
using System.Security.Cryptography;
//这个是处理文字编码的前提
using System.Text;
//以“流”的形式处理文字，也是微软DES算法要求的
using System.IO;

/// <summary>
/// 各种加密和解密方式
/// </summary>
public class Cryptographic
{
    //默认密钥向量 
    private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    
    /// <summary>
    /// 2次偏移加密算法
    /// </summary>
    /// <param name="inputByteArray"></param>
    /// <param name="keyAry"></param>
    /// <returns></returns>
    public static byte[] CQEncrypt(byte[] inputByteArray,byte[] keyAry)
    {
        byte[] encryptBytes = new byte[inputByteArray.Length];
        for (int i = 0; i < inputByteArray.Length; i++)
        {
            encryptBytes[i] = (byte)(inputByteArray[i] + keyAry[i]);
        }
        return encryptBytes;
    }
    /// <summary>
    /// 2次偏移解密算法
    /// </summary>
    /// <param name="inputByteArray"></param>
    /// <param name="keyAry"></param>
    /// <returns></returns>
    public static byte[] CQDecrypt(byte[] inputByteArray, byte[] keyAry)
    {
        byte[] encryptBytes = new byte[inputByteArray.Length];
        for(int i=0;i<inputByteArray.Length;i++)
        {
            encryptBytes[i] = (byte)(inputByteArray[i] -keyAry[i]);
        }
        return encryptBytes;
    }

    /// <summary>
    /// AES加密算法
    /// </summary>
    /// <param name="inputByteArray">明文字符串</param>
    /// <param name="strKey">密钥</param>
    /// <returns>返回加密后的密文字节数组</returns>
    public static byte[] AESEncrypt(byte[] inputByteArray, string strKey)
    {
        //分组加密算法
        SymmetricAlgorithm des = Rijndael.Create();

        //当需要加密字符串时,用该条代码
        //byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组	

        //设置密钥及密钥向量
        des.Key = Encoding.UTF8.GetBytes(strKey);
        des.IV = _key1;
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
        cs.Close();
        ms.Close();
        return cipherBytes;
    }

    /// <summary>
    /// AES解密算法
    /// </summary>
    /// <param name="cipherText">密文字节数组</param>
    /// <param name="strKey">密钥</param>
    /// <returns>返回解密后的字符串</returns>
    public static byte[] AESDecrypt(byte[] cipherText, string strKey)
    {
        SymmetricAlgorithm des = Rijndael.Create();
        des.Key = Encoding.UTF8.GetBytes(strKey);
        des.IV = _key1;
        byte[] decryptBytes = new byte[cipherText.Length];
        MemoryStream ms = new MemoryStream(cipherText);
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
        cs.Read(decryptBytes, 0, decryptBytes.Length);
        cs.Close();
        ms.Close();
        return decryptBytes;
    }

    /// <summary>
    /// 加密字节流
    /// </summary>
    /// <param name="inData">原始字节流</param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] EncryptDES(byte[] inData, string key, string iv)
    {
        try
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(iv);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();//Convert.ToBase64String(ms.ToArray()
                }
                catch
                {
                }
            }
        }
        catch { }

        return inData;
    }
    /// <summary>
    /// 解密字节流
    /// </summary>
    /// <param name="encryptedData">加密的字节流</param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] DecryptDES(byte[] encryptedData, string key, string iv)
    {
        byte[] btKey = Encoding.UTF8.GetBytes(key);

        byte[] btIV = Encoding.UTF8.GetBytes(iv);

        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        using (MemoryStream ms = new MemoryStream())
        {
            //byte[] inData = Convert.FromBase64String(encryptedString);
            try
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedData, 0, encryptedData.Length);

                    cs.FlushFinalBlock();
                }

                return ms.ToArray();//Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return encryptedData;
            }
        }
    }
    public static string EncryptDESString(string sInputString, string sKey, string sIV)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(sInputString);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

            ICryptoTransform desencrypt = DES.CreateEncryptor();

            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);

            return BitConverter.ToString(result);
        }
        catch { }

        return "转换出错！";
    }
    public static string DecryptDESString(string sInputString, string sKey, string sIV)
    {
        try
        {
            string[] sInput = sInputString.Split("-".ToCharArray());

            byte[] data = new byte[sInput.Length];

            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

            ICryptoTransform desencrypt = DES.CreateDecryptor();

            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);

            return Encoding.UTF8.GetString(result);
        }
        catch { }

        return "解密出错！";
    }
    /// <summary>
    /// DES加密方法
    /// </summary>
    /// <param name="strPlain">明文</param>
    /// <param name="strDESKey">密钥</param>
    /// <param name="strDESIV">向量</param>
    /// <returns>密文</returns>
    public static string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
    {
        //把密钥转换成字节数组
        byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
        //把向量转换成字节数组
        byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
        //声明1个新的DES对象
        DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
        //开辟一块内存流
        MemoryStream msEncrypt = new MemoryStream();
        //把内存流对象包装成加密流对象
        CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Write);
        //把加密流对象包装成写入流对象
        StreamWriter swEncrypt = new StreamWriter(csEncrypt);
        //写入流对象写入明文
        swEncrypt.WriteLine(strPlain);
        //写入流关闭
        swEncrypt.Close();
        //加密流关闭
        csEncrypt.Close();
        //把内存流转换成字节数组，内存流现在已经是密文了
        byte[] bytesCipher = msEncrypt.ToArray();
        //内存流关闭
        msEncrypt.Close();
        //把密文字节数组转换为字符串，并返回
        return UnicodeEncoding.Unicode.GetString(bytesCipher);
    }
    /// <summary>
    /// DES解密方法
    /// </summary>
    /// <param name="strCipher">密文</param>
    /// <param name="strDESKey">密钥</param>
    /// <param name="strDESIV">向量</param>
    /// <returns>明文</returns>
    public static string DESDecrypt(string strCipher, string strDESKey, string strDESIV)
    {
        //把密钥转换成字节数组
        byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
        //把向量转换成字节数组
        byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
        //把密文转换成字节数组
        byte[] bytesCipher = UnicodeEncoding.Unicode.GetBytes(strCipher);
        //声明1个新的DES对象
        DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
        //开辟一块内存流，并存放密文字节数组
        MemoryStream msDecrypt = new MemoryStream(bytesCipher);
        //把内存流对象包装成解密流对象
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, desDecrypt.CreateDecryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Read);
        //把解密流对象包装成读出流对象
        StreamReader srDecrypt = new StreamReader(csDecrypt);
        //明文=读出流的读出内容
        string strPlainText = srDecrypt.ReadLine();
        //读出流关闭
        srDecrypt.Close();
        //解密流关闭
        csDecrypt.Close();
        //内存流关闭
        msDecrypt.Close();
        //返回明文
        return strPlainText;
    }
}
