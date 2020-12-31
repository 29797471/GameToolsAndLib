using System;
using System.Windows;
using System.Windows.Controls;

public class ListViewAttribute : ControlPropertyAttribute
{
    public ListViewAttribute(string name=null) : base(name)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new ListView();
        
        SetPropertyChanged(ctl, () =>
        {
            WinUtil.AddContextMenu(ctl, Target, () => new object[] { ctl.SelectedItem });
        });
        Type itemType = Info.PropertyType.GetGenericArguments()[0];
        var attr = AssemblyUtil.GetClassAttribute<EditorAttribute>(itemType);
        
        DrawInListView(ctl,Target);
        ctl.SetBinding(ListView.ItemsSourceProperty, Info.Name);

        return ctl;
    }
    /// <summary>
    /// 将修饰的对象绘制在ListView中
    /// </summary>
    public void DrawInListView(ListView listView, object target)
    {
        var gw = new GridView();
        listView.View = gw;

        var list = PriorityAttribute.GetMembersBySort(target.GetType().GetGenericArguments()[0]);
        foreach (var member in list)
        {
            var attr = AssemblyUtil.GetMemberAttribute<GridViewColumnAttribute>(member, false, target);
            if (attr != null)
            {
                attr.AddGridViewColumn(gw);
            }
        }
    }

}


