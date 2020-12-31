using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
public class XmlUtil
{

    #region 反序列化
    /// <summary>
    /// 反序列化
    /// </summary>
    public static T Deserialize<T>(string xml)
    {
        using (StringReader sr = new StringReader(xml))
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return (T)xmldes.Deserialize(sr);
        }
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    public static object Deserialize(Type type, Stream stream)
    {
        XmlSerializer xmldes = new XmlSerializer(type);
        return xmldes.Deserialize(stream);
    }
    #endregion
    #region 序列化
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static string Serializer( object obj)
    {
        MemoryStream Stream = new MemoryStream();
        //创建序列化对象
        XmlSerializer xml = new XmlSerializer(obj.GetType());
        try
        {
            //序列化对象
            xml.Serialize(Stream, obj);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        Stream.Position = 0;
        StreamReader sr = new StreamReader(Stream);
        string str = sr.ReadToEnd();
        return str;
    }
    #endregion


    #region 获取对应XML节点的值
    /// <summary>
    /// 摘要:获取对应XML节点的值
    /// </summary>
    /// <param name="stringRoot">XML节点的标记</param>
    /// <param name="xmlPath">XML内容</param>
    /// <returns>返回获取对应XML节点的值</returns>
    public static string XmlAnalysis(string stringRoot, string xmlPath)
    {
        if (stringRoot.Equals("") == false)
        {
            XmlDocument XmlLoad = new XmlDocument();
            XmlLoad.LoadXml(xmlPath);
            return XmlLoad.DocumentElement.SelectSingleNode(stringRoot).InnerXml.Trim();
        }
        return "";
    }
    #endregion
}