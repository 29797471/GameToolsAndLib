using DevelopTool;
using System.Linq;
using WinCore;

/// <summary>
/// 编辑的值
/// 值有两种情况：1.有定义的模板名字 2.模板名字为空,自定义的值
/// </summary>
[Editor("代码-自定义")]
public class CustomCodeTemplate : NotifyObject, IExpression
{
    [MenuItem("查看值对应的代码", "/WinCore;component/Res/find.ico")]
    public void ShowViewCode()
    {
        CustomMessageBox.Show(ExecContent);
    }

    public override string ToString()
    {
        return Content;
    }
   
    /// <summary>
    /// 编程语言
    /// </summary>
    public string Language
    {
        get { return mLanguage; }
        set { mLanguage = value; }
    }
    public string mLanguage;

    CodeStyleLanguage CodeLanguage
    {
        get
        {
            return CodeStyleNewModel.instance.GetNodeList().ToList().Find(x => x.Name == Language);
        }
    }

    /// <summary>
    /// 模版类型
    /// </summary>
    public string StyleType
    {
        get { return mStyleType; }
        set { mStyleType = value; Update("StyleType"); }
    }
    public string mStyleType;

    /// <summary>
    /// 值: 关联的代码模版名称
    /// </summary>
    //public string mStyleName;

    

    public string Content
    {
        get
        {
            return  Data;
        }
    }

    public string ExecContent
    {
        get
        {
            return Data;
        }
    }


    /// <summary>
    /// 数据
    /// </summary>
    [MinWidth(600),MinHeight(250)]
    [TextBox("数据"), AcceptsReturn(true), AcceptsTab(true)]
    [Priority(4)]
    public string Data 
    {
        get 
        {
            return mData;
        } 
        set
        {
            mData = value;
            Update("Data");
        } 
    }
    public string mData;
    
    [Priority(3)]
    [FileEdit("在打开窗口中编辑")]
    public string DataX
    {
        get
        {
            return Data;
        }
        set
        {
            Data = value;
            Update("DataX");
        }
    }
}