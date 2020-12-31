using System.IO;
using ICCEmbedded.SharpZipLib.Zip.Compression;

class CompressCRC
{
    public const int _MAX_PACKAGE_ = 2048;
    /// <summary>
    /// 压缩
    /// </summary>
    public static byte[] compress(byte[] input)
    {
        // Create the compressor with highest level of compression   
        Deflater compressor = new Deflater();
        compressor.SetLevel(Deflater.BEST_COMPRESSION);

        // Give the compressor the data to compress   
        compressor.SetInput(input);
        compressor.Finish();

        /*  
         * Create an expandable byte array to hold the compressed data.  
         * You cannot use an array that's the same size as the orginal because  
         * there is no guarantee that the compressed data will be smaller than  
         * the uncompressed data.  
         */
        MemoryStream bos = new MemoryStream(input.Length);

        // Compress the data   
        byte[] buf = new byte[_MAX_PACKAGE_];
        while (!compressor.IsFinished)
        {
            int count = compressor.Deflate(buf);
            bos.Write(buf, 0, count);
        }

        // Get the compressed data   
        return bos.ToArray();
    }

    /// <summary>
    /// 解压
    /// </summary>
    public static byte[] unCompress(byte[] input)
    {
        Inflater decompressor = new Inflater();
        decompressor.SetInput(input);

        // Create an expandable byte array to hold the decompressed data   
        MemoryStream bos = new MemoryStream(input.Length);

        // Decompress the data   
        byte[] buf = new byte[_MAX_PACKAGE_];
        while (!decompressor.IsFinished)
        {
            int count = decompressor.Inflate(buf);
            bos.Write(buf, 0, count);
        }

        // Get the decompressed data   
        return bos.ToArray();
    }
}
