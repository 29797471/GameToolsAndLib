using CqBehavior.Task;
using CqCore;
using System;

[Editor("坐标变量"), Width(300), Height(300)]
public class ScreenPosVariable : NotifyObject
{
    [Priority(1)]
    [CheckBox("是变量")]
    public bool IsVariable { get { return mIsVariable; } set { mIsVariable = value; Update("IsVariable"); } }
    public bool mIsVariable;

    [MaxWidth(200)]
    [MinWidth(100)]
    [Visibility("IsVariable", AttributeTarget.Parent)]
    [TextBox("变量名")]
    [Priority(2)]
    public string Val1
    {
        get { if (mVal1 == null) mVal1 = ""; return mVal1; }
        set { mVal1 = value; Update("Val1"); }
    }
    public string mVal1;


    [Width(100)]
    [Visibility("IsVariable",AttributeTarget.Parent, "x=0")]
    [UnderLine("屏幕位置"), Click]
    [Priority(3)]
    public UVData ScreenPos
    {
        get { if (mScreenPos == null) mScreenPos = new UVData(); return mScreenPos; }
        set { mScreenPos = value; Update("ScreenPos"); }
    }
    public UVData mScreenPos;

    public UVData GetValue()
    {
        Update("Name");
        return ScreenPos;
    }
    public void SetValue( UVData value)
    {
        
        {
            ScreenPos = value;
        }
        Update("Name");
    }

    CqBehaviorNode node;
    public void SetRoot(CqBehaviorNode node)
    {
        this.node = node;
    }
    public string Name
    {
        get
        {
            if (!IsVariable)
            {
                return ScreenPos.ToString();
            }
            else 
            {
                return "@" + Val1 + "=null";
            }
        }
    }
    public override string ToString()
    {
        return Name;
    }
    public ScreenPosVariable()
    {
        PropertyChanged += (sender, args) => { if (args.PropertyName != "Name") Update("Name"); };
    }

}