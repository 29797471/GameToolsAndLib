using CqBehavior.Task;
using CqCore;
using System.Windows.Media;

[Editor("颜色变量"), Width(400, AttributeTarget.Self), Height(300, AttributeTarget.Self)]
public class ColorVariable : NotifyObject
{
    [Priority(0)]
    [CheckBox("是变量")]
    public bool IsVariable { get { return mIsVariable; } set { mIsVariable = value; Update("IsVariable"); } }
    public bool mIsVariable;

    [MaxWidth(200)]
    [MinWidth(100)]
    [Visibility("IsVariable", AttributeTarget.Parent)]
    [TextBox("变量名")]
    [Priority(1)]
    public string Val1
    {
        get { if (mVal1 == null) mVal1 = ""; return mVal1; }
        set { mVal1 = value; Update("Val1"); }
    }
    public string mVal1;


    [Width(200)]
    [Visibility("IsVariable", AttributeTarget.Parent, "x=0")]
    [ColorBox("颜色")]
    [Priority(2)]
    public Color ScreenColor
    {
        get
        {
            return WinUtil.ToMediaColor(mScreenColor);
        }
        set
        {
            mScreenColor = value.ToString();
            Update("ScreenColor");
        }
    }
    public string mScreenColor = "#000000";

    [Width(200)]
    [Visibility("IsVariable", AttributeTarget.Parent, "x=0")]
    [UnderLine("屏幕位置"), Click]
    [Priority(3)]
    public UVData ScreenPos
    {
        get { if (mScreenPos == null) mScreenPos = new UVData(); return mScreenPos; }
        set { mScreenPos = value; Update("ScreenPos"); }
    }
    public UVData mScreenPos;

    //[Visibility("IsVariable", AttributeTarget.Panel, "ConverNot")]
    [Button,Click("Sampling")]
    [Priority(4)]
    public string Btn { get { return "从屏幕位置抓取一个颜色"; } }
    public void Sampling(object obj)
    {
        ScreenColor = WinUtil.GetScreenPixel((int)ScreenPos.U, (int)ScreenPos.V);
    }
    

    public Color GetValue()
    {
        return ScreenColor;
    }

    public void SetValue( Color color)
    {
        {
            ScreenColor = color;
        }
        Update("Name");
    }

    
    public string Name
    {
        get
        {
            return ScreenColor.ToString();
        }
    }
    public override string ToString()
    {
        return Name;
    }
    public ColorVariable()
    {
        PropertyChanged += (sender, args) => { if (args.PropertyName != "Name") Update("Name"); };
    }
}