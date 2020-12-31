using System.Windows;
using WinCore;

/// <summary>
/// 修饰树型结构
/// </summary>
public class TreeViewAttribute : ControlPropertyAttribute
{
    public string types;
    public TreeViewAttribute(string types, string name = null, float width = 0f) :base(name,width)
    {
        this.types = types;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new CustomTreeUserControl();

        SetPropertyChanged(ctl, () =>
        {
            
            WinUtil.AddContextMenu(ctl, Target, () => new object[] { ctl.SelectedValue });
            ctl.Types = AssemblyUtil.GetTypesByNamespace(types);
            ctl.DataContext = Target;
        });
        return ctl;
    }
}
