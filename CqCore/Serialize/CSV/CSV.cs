using ParserCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// CSV数据格式的序列化
/// </summary>
public static class CSV
{
    /// <summary>
    /// 序列化表格数据
    /// </summary>
    /// <returns></returns>
    public static List<List<string>> Deserialize(string content)
    {
        var cp=new BaseParser<char>(content.ToList());

        List<string> rowStrs=new List<string>();

        List<List<string>> data = new List<List<string>>();

        while (!cp.IsEnd())
        {
            switch(cp.Value)
            {
                case '"':
                    {
                        StringWriter sw = new StringWriter();
                        cp.Next();
                        while (cp.Value != '"')
                        {
                            if (cp.Value == '\\')
                            {
                                cp.Next();
                                sw.Write(cp.Value);
                                cp.Next();
                            }
                            else
                            {
                                sw.Write(cp.Value);
                                cp.Next();
                            }
                        }
                        cp.Next();
                        rowStrs.Add(sw.ToString());
                    }
                    break;
                case ',':
                    cp.Next();
                    break;
                case '\n':
                    {
                        cp.Next();
                        data.Add(rowStrs);
                        rowStrs = new List<string>();
                        break;
                    }
                case '\r':
                    {
                        cp.Next();
                        if (cp.Value == '\n') cp.Next();
                        data.Add(rowStrs);
                        rowStrs = new List<string>();
                        break;
                    }
                default:
                    {
                        StringWriter sw = new StringWriter();
                        while (!cp.IsEnd() && cp.Value != ',' && cp.Value!='\r' && cp.Value!='\n')
                        {
                            sw.Write(cp.Value);
                            cp.Next();
                        }
                        rowStrs.Add(sw.ToString());
                    }
                    break;
            }
        }
        
        return data;
    }

    /// <summary>
    /// 用确定的行数据对象类型来序列化表格数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public static List<T> TryDeserialize<T>(string content) where T:new()
    {
        content = content.Trim();
        content = content.Replace("\r", "");
        var strList=content.Split('\n');
        var type = typeof(T);
        var list = new List<T>();
        var heads = strList[0].Split(',');
        //变量名中有空格,替换为_才能对应到现有的数据结构成员
        for (int i=0;i<heads.Length;i++)
        {
            heads[i] = heads[i].Replace(" ", "_");
        }
        for(int i=1;i<strList.Length;i++)
        {
            var itemList = strList[i].Split(',');
            var item = new T();
            for(int j=0;j<heads.Length;j++)
            {
                var value = itemList[j];
                if(value!="null" && value!="None")
                {
                    AssemblyUtil.SetMemberValue(item, heads[j], value, true);
                }
            }
            list.Add(item);
        }
        return list;
    }
}
