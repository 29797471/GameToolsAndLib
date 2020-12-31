using CqCore;
using DevelopTool;
using System.Collections.Generic;
using System.Linq;
using WinCore;

/// <summary>
/// 事件代码模版
/// </summary>
[Editor("事件成员")]
public class EventArgCodeTemplate : NotifyObject, IExpression
{
    [MenuItem("查看表达式对应的代码", "/WinCore;component/Res/find.ico")]
    public void ShowViewCode()
    {
        CustomMessageBox.Show(ExecContent);
    }
    public override string ToString()
    {
        return Content;
    }

    EventExp ee;
    public EventExp Ee
    {
        set
        {
            ee = value;
            //if(mItemValue==null)
            //{
            //    mItemValue = Item.ExecContent;
            //}
        }
    }
    /// <summary>
    /// 编程语言
    /// </summary>
    public string Language
    {
        get { if (mLanguage == null) mLanguage = CodeStyleNewModel.instance.setting.CodeSettingList[0].Name; return mLanguage; }
        set { mLanguage = value; }
    }
    public string mLanguage;

    /// <summary>
    /// 模版类型
    /// </summary>
    [TextBox("成员:"),IsEnabled(false)]
    [Visibility("ItemsCount", AttributeTarget.Parent,"x>0")]
    [Priority(2)]
    public string StyleType
    {
        get
        {
            return mStyleType;
        }
        set
        {
            mStyleType = value;
            Update("StyleType");
        }
    }
    public string mStyleType;

    /// <summary>
    /// 事件id
    /// </summary>
    [Priority(1)]
    [Label()]
    public string EventName { get { return "事件:"+ee.Chooses.Last(); }  }

    public int ItemsCount
    {
        get
        {
            return Items.Count;
        }
    }

    [Visibility("ItemsCount", AttributeTarget.Parent, "0=x")]
    [Label()]
    [Priority(5)]
    public string NoItem { get { return "没有一个事件成员的类型是:" + mStyleType; } }

    /// <summary>
    /// 事件参数
    /// </summary>
    public string mItemName;
    public string mItemValue;

    
    public EventStructItem Item
    {
        get
        {
            if (mItemName == null && Items.Count > 0)
            {
                mItemName = Items[0].Name;
                mItemValue = Items[0].ExecContent;
            }
            return Items.Find(x => x.Name == mItemName);
        }
        set
        {
            mItemName = value.Name;
            mItemValue = value.ExecContent;
        }
    }

    [Visibility("ItemsCount",  AttributeTarget.Parent,"x>0")]
    [ComboBox(), SelectedValue("Item"), VerticalAlignment(System.Windows.VerticalAlignment.Center)]
    [Priority(2,1)]
    public List<EventStructItem> Items
    {
        get
        {
            if (dicCSharpToLua == null)
            {
                dicCSharpToLua = new Dictionary<string, string>();
                dicCSharpToLua["int"] = "number";
                dicCSharpToLua["float"] = "number";
            }

            
            return ee.Struct.CustomerList.ToList().Where(x =>
            {
                if(dicCSharpToLua.ContainsKey(x.Type))
                {
                    return dicCSharpToLua[x.Type] == StyleType;
                }
                else
                {
                    return StyleType == x.Type;
                }
            }).ToList();
            //var types=EventModel.instance.setting.KeyValueList.ToList().FindAll(x => x.Value == StyleType).ConvertAll(x => x.Key);
            //return ee.Struct.CustomerList.ToList().Where(x=> types.Contains(x.Type)).ToList();
        }
    }

    /// <summary>
    /// lua->C#
    /// </summary>
    static Dictionary<string, string> dicCSharpToLua;

    public string Content
    {
        get
        {
            return mItemName;
        }
    }
    public string ExecContent
    {
        get
        {
            return mItemValue;
        }
    }
}