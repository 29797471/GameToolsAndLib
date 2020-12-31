using CqCore;
using DevelopTool;
using System;
using System.IO;
using System.Linq;

/// <summary>
/// 将表格数据格式转化为可以Torsion序列化的格式
/// 主要用于纯数据和符号表达式 的反序列化
/// 通常使用在表格数据上 类型只有int,uint,float,string
/// 形如1:10,3:20,4:50
/// </summary>
public class TableDataFormat : Singleton<TableDataFormat>
{
    /// <summary>
    /// 将Json格式转化为lua格式
    /// </summary>
    public string JsonToLua(string tableContent, CustomerStructItem item)
    {
        if (tableContent == null) return null;
        return tableContent.Replace("[", "{").Replace("]", "}");//.Replace(":", "=").Replace("\"", "");
    }

    /// <summary>
    /// 将表格数据格式转化为Lua格式
    /// </summary>
    public string ToLua(string tableContent, CustomerStructItem item)
    {
        tableContent = tableContent.Replace("\n", "");
        return ToLuaBase(tableContent, item);
    }
    
    /// <summary>
    /// 将表格数据格式转化为可以Torsion序列化的格式
    /// </summary>
    public string ToLuaBase(string tableContent, CustomerStructItem item, bool canBeList = true)
    {
        if(item.IsList && canBeList)
        {
            return ToLuaList(tableContent, item);
        }
        else if (item.IsBaseType())
        {
            switch (item.Type)
            {
                case "string":
                    return "\"" + tableContent + "\"";
                case "bool":
                    return (tableContent == "1") ? "true" : "false";
                default:
                    return (tableContent == "") ? "0" : tableContent;
            }
        }
        else
        {
            return ToLuaObject(tableContent, ExcelStructModel.instance.CustomStructList.ToList().Find(x => x.Name == item.Type));
        }
    }
    string[] SplitWithDepth(string content)
    {
        foreach(var chr in ExcelStructModel.instance.setting.Separates)
        {
            if(content.Contains(chr))
            {
                return content.Split(chr);
            }
        }
        return new string[] { content};
    }
    string ToLuaList(string content, CustomerStructItem item)
    {
        if (content == "") return "{}";
        var ary = SplitWithDepth(content);

        for (int i = 0; i < ary.Length; i++)
        {
            ary[i] = ToLuaBase(ary[i], item, false);
        }
        return "{" + string.Join(",", ary) + "}";
    }
    
    string ToLuaObject(string tableContent, CustomStruct item)
    {
        var ary = SplitWithDepth(tableContent);

        var sw = new StringWriter();
        var i = 0;
        for (; i < item.CustomerList.Count-1 && i < ary.Length-1; i++)
        {
            var it = item.CustomerList[i];
            sw.Write(string.Format("{0}={1},", it.Variable, ToLuaBase(ary[i], it)));
        }
        {
            var it = item.CustomerList[i];
            sw.Write(string.Format("{0}={1}", it.Variable, ToLuaBase(ary[i], it)));
        }
        return "{" + sw.ToString() + "}";
    }


}
