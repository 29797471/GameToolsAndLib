using DevelopTool;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

[Editor("文件编辑"),Width(1100),Height(800)]
public class FileEdit : NotifyObject
{
    public OtherSetting Settting
    {
        get
        {
            if(mSettting==null)
            {
                mSettting= SettingModel.instance.GetSetting<OtherSetting>();
            }
            return mSettting;
        }
    }
    OtherSetting mSettting;
    public object Foreground
    {
        get
        {
            return new SolidColorBrush(Settting.Foreground);
        }
    }
    public object Background
    {
        get
        {
            return new SolidColorBrush(Settting.Background);
        }
    }
    public double FontSize
    {
        get
        {
            return Settting.FontSize;
        }
    }
    [TextBox, AcceptsReturn, AcceptsTab, Foreground("Foreground"), Background("Background"),FontSize("FontSize")]
    [HorizontalAlignment(HorizontalAlignment.Stretch), VerticalAlignment(VerticalAlignment.Stretch)]
    public string CustomCode
    {
        get { if (mCustomCode == null) mCustomCode = ""; return mCustomCode; }
        set { mCustomCode = value; Update("CustomCode"); }
    }
    public string mCustomCode;
}

/// <summary>
/// 通过打开一个编辑窗口来编辑一段代码或者文件内容
/// 修饰string 属性
/// </summary>
public class FileEditAttribute : ControlPropertyAttribute
{
    public string btnName;
    public FileEditAttribute(string btnName) : base(null)
    {
        this.btnName = btnName;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var btn = new Button();
        btn.Content = btnName;
        btn.Click += (obj, args) =>
        {
            var xx = WinUtil.OpenEditorWindow(new FileEdit { CustomCode = (string)Target });
            if (xx != null)
            {
                Target = xx.CustomCode;
            }
        };
        return btn;
    }
}