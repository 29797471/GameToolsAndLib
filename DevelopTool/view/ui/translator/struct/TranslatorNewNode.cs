using CqCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
/// <summary>
/// 转译全局变量
/// </summary>
[Editor("")]
[System.Serializable]
public class TranslatorNewNode : NotifyObject
{
    public override string ToString()
    {
        return Id.ToString();
    }

    /// <summary>
    /// 在Excel中的第几行
    /// </summary>
    
    [Priority(5)]
    [TextBox("Excel行"),IsEnabled(false),Width(80)]
    public int ExcelRow
    {
        get
        {
            return excelRow;
        }
        set
        {
            excelRow = value;
        }
    }
    int excelRow;

    [Priority(1)]
    [TextBox("ID"),MinWidth(100)]
    [Export("%Id%")]
    public int Id
    {
        get { return mId; }
        set { mId = value; Update("Id"); }
    }
    public int mId;

    [Export("%StyleIndex%")]
    public int StyleIndex
    {
        get { return mStyleIndex; }
        set { mStyleIndex = value; Update("StyleIndex"); }
    }
    public int mStyleIndex;


    public string Desc
    {
        get { return mDesc; }
        set { mDesc = value; Update("Desc"); }
    }
    public string mDesc;

    [Priority(3)]
    [TextBox("转译格式"), AcceptsReturn(true), TextWrapping(TextWrapping.Wrap), Width(300)]
    [Export("%TranFormat%")]
    public string TranFormat
    {
        get { if (mTranFormat == null) mTranFormat = ""; return mTranFormat; }
        set { mTranFormat = value; Update("TranFormat"); }
    }
    public string mTranFormat;

    [Export("%TranFormatX%")]
    public string TranFormatX
    {
        get
        {
            return TranFormat.Replace("\"","\\\"");
        }
    }

    [Export("%Content%")]
    public string Content
    {
        get
        {
            return "[=[" + RegexUtil.Replace(TranFormat, Pattern, m => "]=]..Convert."+ m.Groups["type"].Value + "(args," +
            (int.Parse(m.Groups["i"].Value) + 1) + ")..[=[") + "]=]";
        }
    }

    /// <summary>
    /// 捕获正则
    /// </summary>
    public string Pattern
    {
        get
        {
            return "%(?<type>[a-zA-Z_]+?)(?<i>\\d)%";
        }
    }
}
