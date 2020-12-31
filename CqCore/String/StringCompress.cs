using System;
using System.IO;
using System.IO.Compression;
using System.Text;

public static class StringCompress
{
    static byte[] tempBuffer=new byte[2048];

    /// <summary>
    /// 超过这个尺寸的数据会被压缩
    /// </summary>
    const int compressSize = 420;

    /// <summary>
    /// 解压时缓冲区以多大倍数的空间来接收数据
    /// </summary>
    const int uncompressScale = 50;

    static void CheckIncrease(int len)
    {
        var targetLen = tempBuffer.Length;
        while (targetLen < len) targetLen *= 2;
        if(targetLen!= tempBuffer.Length)
        {
            tempBuffer = new byte[targetLen];
        }
    }
    /// <summary>
    /// 自动判定内容大小选择性压缩,返回首位表示数据是否有压缩<para/>
    /// 主要应用于网络通信时
    /// </summary>
    public static byte[] CheckCompress(string content)
    {
        lock(tempBuffer)
        {
            var realData = Encoding.UTF8.GetBytes(content);
            if (realData.Length < compressSize)
            {
                var result = new byte[realData.Length + 1];
                Array.Copy(realData, 0, result, 1, realData.Length);
                return result;
            }
            else
            {
                CheckIncrease(realData.Length);
                var len = Zip.Compress(realData, 0, tempBuffer, 0, realData.Length);

                var result = new byte[len + 1];
                result[0] = 1;
                Array.Copy(tempBuffer, 0, result, 1, len);
                return result;
            }
        }
    }

    /// <summary>
    /// 根据首位表示数据是否有压缩,来选择性处理数据<para/>
    /// 主要应用于网络通信时
    /// </summary>
    public static string CheckUnCompress(byte[] netData)
    {
        lock (tempBuffer)
        {
            if (netData[0] == 0)
            {
                return Encoding.UTF8.GetString(netData, 1, netData.Length - 1);
            }
            else
            {
                CheckIncrease(netData.Length * uncompressScale);
                var len = Zip.Decompress(netData, 1, tempBuffer, 0, netData.Length - 1);

                return Encoding.UTF8.GetString(tempBuffer, 0, len);
            }
        }
    }

    /// <summary>  
    /// 字符串压缩  
    /// </summary>  
    public static byte[] Compress(byte[] data)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            return buffer;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>  
    /// 字符串解压缩  
    /// </summary>  
    public static byte[] Decompress(byte[] data)
    {
        try
        {
            MemoryStream ms = new MemoryStream(data);
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
            MemoryStream msreader = new MemoryStream();
            byte[] buffer = new byte[0x1000];
            while (true)
            {
                int reader = zip.Read(buffer, 0, buffer.Length);
                if (reader <= 0)
                {
                    break;
                }
                msreader.Write(buffer, 0, reader);
            }
            zip.Close();
            ms.Close();
            msreader.Position = 0;
            buffer = msreader.ToArray();
            msreader.Close();
            return buffer;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string CompressString(string str)
    {
        byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
        byte[] compressAfterByte = Compress(compressBeforeByte);
        return compressAfterByte.ToBase64();
    }

    public static string DecompressString(string str)
    {
        string compressString = "";
        //byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);    
        byte[] compressBeforeByte = str.ToBase64();
        byte[] compressAfterByte = Decompress(compressBeforeByte);
        compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
        return compressString;
    }
    public static string ZipStr(string value)
    {
        //Transform string into byte[]    
        byte[] byteArray = new byte[value.Length];
        int indexBA = 0;
        foreach (char item in value.ToCharArray())
        {
            byteArray[indexBA++] = (byte)item;
        }

        //Prepare for compress  
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
            System.IO.Compression.CompressionMode.Compress);

        //Compress  
        sw.Write(byteArray, 0, byteArray.Length);
        //Close, DO NOT FLUSH cause bytes will go missing...  
        sw.Close();

        //Transform byte[] zip data to string  
        byteArray = ms.ToArray();
        System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
        foreach (byte item in byteArray)
        {
            sB.Append((char)item);
        }
        ms.Close();
        sw.Dispose();
        ms.Dispose();
        return sB.ToString();
    }

    public static string UnZipStr(string value)
    {
        //Transform string into byte[]  
        byte[] byteArray = new byte[value.Length];
        int indexBA = 0;
        foreach (char item in value.ToCharArray())
        {
            byteArray[indexBA++] = (byte)item;
        }

        //Prepare for decompress  
        System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
        System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
            System.IO.Compression.CompressionMode.Decompress);

        //Reset variable to collect uncompressed result  
        byteArray = new byte[byteArray.Length];

        //Decompress  
        int rByte = sr.Read(byteArray, 0, byteArray.Length);

        //Transform byte[] unzip data to string  
        System.Text.StringBuilder sB = new System.Text.StringBuilder(rByte);
        //Read the number of bytes GZipStream red and do not a for each bytes in  
        //resultByteArray;  
        for (int i = 0; i < rByte; i++)
        {
            sB.Append((char)byteArray[i]);
        }
        sr.Close();
        ms.Close();
        sr.Dispose();
        ms.Dispose();
        return sB.ToString();
    }
}
