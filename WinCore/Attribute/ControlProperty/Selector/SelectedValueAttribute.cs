using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using WinCore;

public class SelectedValueAttribute : LinkControlPropertyAttribute
{

    public SelectedValueAttribute(string path) :base(path)
    {
    }

    protected override void OnInit(FrameworkElement fe)
    {
        //需要注意的是List 更新过滤时会更新选中项
        if (fe is Selector)
        {
            var sel = (fe as Selector);
            //SetLinkPropertyChanged(fe, o => sel.SelectedValue=o);
            //sel.SelectionChanged += (obj,e) => Data = sel.SelectedValue;
            (fe as Selector).SetBinding(Selector.SelectedValueProperty, path);

            //SetBindingOrValue(fe, Selector.SelectedValueProperty);
        }
        else if(controlAttr is SelectTreeNodeAttribute)
        {
            var d = controlAttr as SelectTreeNodeAttribute;
            SetLinkPropertyChanged(fe, o => d.ComboBoxGroup.SelectValue = (ObservableCollection<string>)o);
            d.ComboBoxGroup.OnSelectChanged = (o) => Data = o;
        }
        else if (fe is CustomTreeUserControl)
        {
            var huc = fe as CustomTreeUserControl;
            huc.SelectedChanged += (obj, e) =>
            {
                if (huc.SelectedValue != null) Data = huc.SelectedValue.nodeObj;
                else Data = null;
            };
        }
        else if (fe is CustomListText)
        {
            var huc = (fe as CustomListText);
            huc.listBox.SelectionChanged += (obj, e) =>
            {
                if (huc != null) Data = huc.SelectedValue;
            };
        }
    }
    protected override void OnInit(FrameworkElementFactory fe)
    {
        fe.SetBinding(Selector.SelectedValueProperty,new Binding(path));
    }
}
