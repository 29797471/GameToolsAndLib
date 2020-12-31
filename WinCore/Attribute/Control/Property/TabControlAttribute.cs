using System.Windows.Controls;
using System.Reflection;
using WinCore;
using System.Collections;
using System.Windows;

/// <summary>
/// 标签页
/// 修饰ObservableCollection列表
/// 一般用于索引不同的数据结构
/// </summary>
public class TabControlAttribute : ControlPropertyAttribute
{
    public TabControlAttribute(string name = null) : base(name)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var tc = new TabControl();
        tc.Visibility = Visibility.Collapsed;
        SetPropertyChanged(tc,() =>
        {
            tc.Items.Clear();
            foreach (var it in Target as IList)
            {
                //var e = AssemblyUtil.GetClassAttribute<EditorAttribute>(it);
                //var eName = e.name;
                //if (eName == null) eName = it.ToString();
                //tc.Items.Add(new TabItem() { Header = eName });
                tc.Items.Add(WinUtil.DrawHCC<TabItem>(it));
                
            }
            tc.SelectedIndex = 0;
            tc.Visibility = Visibility.Visible;
        });
        tc.SelectionChanged += Tc_SelectionChanged;
        tc.DataContext = Target;
        return tc;
    }

    private void Tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tc = sender as TabControl;
        var item = tc.SelectedItem as TabItem;
        var list = (tc.DataContext as IList);
        

    }
}


