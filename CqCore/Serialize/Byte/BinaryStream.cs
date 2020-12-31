using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BinaryStream
{
    public static byte[] Serialize<T>(T pkg) 
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, pkg);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
    public static byte[] SerializeBinary<T>(T request)      //结构体转化为字节流
    {

        BinaryFormatter serializer = new BinaryFormatter();

        MemoryStream memStream = new MemoryStream();

        serializer.Serialize(memStream, request);

        return memStream.GetBuffer();

    }
    public static T DeSerialize<T>(byte[] bs) 
    {
        using (MemoryStream ms = new MemoryStream(bs))
        {
            BinaryFormatter bf = new BinaryFormatter();
            T pkg = (T)bf.Deserialize(ms);
            return pkg;
        }
    }

    public static T DeserializeBinary<T>(byte[] buf)        //字节流转化为结构体
    {

        MemoryStream memStream = new MemoryStream(buf);

        memStream.Position = 0;

        BinaryFormatter deserializer = new BinaryFormatter();

        T newobj = (T)deserializer.Deserialize(memStream);

        memStream.Close();

        return newobj;
    }
}
