using CqCore;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

/// <summary>
/// 变量/类导出标识
/// </summary>
public class ExportAttribute : PropertyAttribute
{
    /// <summary>
    /// 导出变量的先后顺序
    /// </summary>
    public int priority;

    public string name;
    public string start;
    public string end;
    public string middle;
    
    /// <summary>
    /// 用于在修饰List变量中,确定成员之间是否包含分隔符
    /// 在包含分隔符时会用%[%和%]%来识别
    /// </summary>
    public bool hasSeparator;


    public enum Style
    {
        /// <summary>
        /// 替换转译符name 成对应变量
        /// </summary>
        One,

        /// <summary>
        /// 替换以start开始,end结束的内容
        /// </summary>
        Start_End,

        /// <summary>
        /// true:保留以start开始,middle结束的内容
        /// false:保留以middle开始,end结束的内容
        /// </summary>
        Start_Middle_End,


    }
    public Style style;

    public bool serialize;

    /// <summary>
    /// 替换转译符name 成对应变量
    /// </summary>
    public ExportAttribute(string name = null,int priority=0,bool serialize=false)
    {
        this.name = name;
        this.serialize = serialize;
        this.priority = priority;
        style = Style.One;
    }

    /// <summary>
    /// 替换以start开始,end结束的内容
    /// </summary>
    public ExportAttribute(string start,string end, int priority = 0,bool hasSeparator=false)
    {
        this.start = start;
        this.end = end;
        this.priority = priority;
        this.hasSeparator = hasSeparator;
        style = Style.Start_End;
    }

    /// <summary>
    /// true:保留以start开始,middle结束的内容
    /// false:保留以middle开始,end结束的内容
    /// </summary>
    public ExportAttribute(string start,string middle, string end, int priority = 0)
    {
        this.start = start;
        this.end = end;
        this.middle = middle;
        this.priority = priority;
        style = Style.Start_Middle_End;
    }

    public string Replace(string content,Func<object,string> func0=null, Func<string, string> func1=null)
    {
        var sign= (name == null) ? "%" + Info.Name + "%" : name;
        switch (style)
        {
            case Style.One:
                string str=null;
                if (func0 != null) str = func0(Target);
                else
                {
                    if (serialize)
                    {
                        str = Torsion.Serialize(Target);
                    }
                    else str = Target.ToString();
                }
                content = content.Replace(sign, str);
                break;
            case Style.Start_End:
                content = StringUtil.ReplaceSubStr(content, start, end, func1);
                break;
            case Style.Start_Middle_End:
                content = StringUtil.ReplaceSubStrByBool(content, (bool)Target, start, middle, end);
                break;
            default:
                break;
        }
        return content;
    }
    /// <summary>
    /// 先导出优先级高的
    /// 同优先级先导出基类成员
    /// </summary>
    public static string InjectData(string content, object obj)
    {
        if (obj == null) return content;
        if (obj is IExport && (obj as IExport).CanExport == false) return "";
        if (obj is ICustomSerialize) return (obj as ICustomSerialize).InjectData(content);
        return InjectNormalData(content, obj);
    }
    /// <summary>
    /// 先导出优先级高的
    /// 同优先级先导出基类成员
    /// </summary>
    public static string InjectNormalData(string content,object obj)
    {
        var list = AssemblyUtil.GetMemberAttributesInObject<ExportAttribute>(obj);
        
        ListUtil.Sort(list, x => x.priority * 2 + (x.Info.DeclaringType != x.Info.ReflectedType ? 0 : 1));
        
        foreach (var attr in list)
        {
            var newValue = attr.Target;
            if (newValue != null)
            {
                if (newValue is TreeNode)
                {
                    content = attr.Replace(content, null, x =>
                    {
                        string str = "";
                        (newValue as TreeNode).PreorderTraversalLeaf((it) =>
                        {
                            str = str + InjectData(x, it.nodeObj);
                        });
                        return str;
                    });
                }
                else if (newValue is IList)
                {
                    content = attr.Replace(content, null, x =>
                    {
                        string str = "";
                        int index = 0;
                        foreach (var it in newValue as IList)
                        {
                            str = str + InjectData(x, it);
                            if(attr.hasSeparator) str = StringUtil.ReplaceSubStr(str, "%[%", "%]%", y => (index != (newValue as IList).Count - 1)?y:"");
                            index++;
                        };
                        return str;
                    });
                }
                else if (newValue is bool)
                {
                    content = attr.Replace(content, x => x.ToString().ToLower(), x =>
                    {
                        if ((bool)newValue) return x;
                        else return "";
                    });
                }
                else if (newValue is IExpression)
                {
                    content = attr.Replace(content, x => (newValue as IExpression).ExecContent.ToString());
                }
                else if (newValue is EventExp)
                {
                    content = attr.Replace(content, x => (newValue as EventExp).ExecContent.ToString());
                }
                else
                {
                    content = attr.Replace(content);
                }
            }
            else
            {
                content = attr.Replace(content, x => "");
            }
        }
        return content;
    }
}

