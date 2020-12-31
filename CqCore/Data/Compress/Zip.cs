using CQ.IO.Compression;
using System;
using System.IO;
using System.Text;

public static class Zip
{
    /// <summary>
    /// 压缩字符串
    /// maxCompressionRatio用于确定压缩缓冲区的大小.
    /// </summary>
    public static string Compress(string input)
    {
        return Convert.ToBase64String(Compress(Encoding.UTF8.GetBytes(input)));
    }

    /// <summary>
    /// 解压缩字符串
    /// maxCompressionRatio最大压缩率,用于确定解压缓冲区的大小.
    /// </summary>
    public static string Decompress(string input)
    {
        return Encoding.UTF8.GetString(Decompress(Convert.FromBase64String(input)));
    }

    /// <summary>
    /// GZip压缩
    /// </summary>
    public static byte[] Compress(byte[] rawData)
    {
        MemoryStream ms = new MemoryStream();
        GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
        compressedzipStream.Write(rawData, 0, rawData.Length);
        compressedzipStream.Close();
        return ms.ToArray();
    }

    /// <summary>
    /// ZIP解压
    /// </summary>
    public static byte[] Decompress(byte[] zippedData)
    {
        MemoryStream ms = new MemoryStream(zippedData);
        GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
        MemoryStream outBuffer = new MemoryStream();
        byte[] block = new byte[1024];
        while (true)
        {
            int bytesRead = compressedzipStream.Read(block, 0, block.Length);
            if (bytesRead <= 0)
                break;
            else
                outBuffer.Write(block, 0, bytesRead);
        }
        compressedzipStream.Close();
        return outBuffer.ToArray();
    }

    /// <summary>
    /// 压缩字节数组
    /// </summary>
    public static int Compress(byte[] inputBytes, int start, byte[] buffer, int bufferStart, int size)
    {
        var x = ArrayUtil.SplitByteArray(inputBytes, start, start+size -1);
        var y = Compress(x);
        var z = Decompress(y);
        try
        {
            using (MemoryStream outStream = new MemoryStream(buffer))
            {
                outStream.Position = bufferStart;
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    zipStream.Write(inputBytes, start, size);
                    zipStream.Close(); //很重要，必须关闭，否则无法正确解压
                    return (int)outStream.Position;
                }
            }
        }
        catch (Exception )
        {
            return 0;
        }
    }

    /// <summary>
    /// 解压缩字节数组
    /// </summary>
    public static int Decompress(byte[] inputBytes,int start,byte[] buffer, int bufferStart, int size)
    {
        var x = ArrayUtil.SplitByteArray(inputBytes, start, start + size - 1);
        var y = Decompress(x);
        using (MemoryStream stream = new MemoryStream())
        {
            using (GZipStream gZipStream = new GZipStream(new MemoryStream(inputBytes, start, size), CompressionMode.Decompress))
            {
                int len = gZipStream.Read(buffer, bufferStart, buffer.Length- bufferStart);
                gZipStream.Close();
                return len;
            }
        }
    }

    
}
