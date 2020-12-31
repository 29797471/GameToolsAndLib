using System;
using System.Collections.Generic;

/// <summary>
/// 点击列表头的回调函数 (proName:funName)
/// </summary>
public class ListViewHeaderClickAttribute : Attribute
{
    public Dictionary< string,string> dic;
    public object Target;

    public ListViewHeaderClickAttribute(params string[] names) 
    {
        dic = new Dictionary<string, string>();
        foreach (var it in names)
        {
            var item = it.Split(':');
            dic[item[0]] = item[1];
        }
    }
    
    public void OnClick(string proName)
    {
        if (dic.ContainsKey(proName))
        {
            AssemblyUtil.InvokeMethod(Target, dic[proName]);
        }
    }
}

