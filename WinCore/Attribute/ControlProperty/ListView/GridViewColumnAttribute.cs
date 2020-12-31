using CqCore;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using WinCore;

/// <summary>
/// 用于定义表格组件的表头
/// </summary>
public class GridViewColumnAttribute:MemberAttribute
{
    public string header;
    public GridViewColumnAttribute(string header) 
    {
        this.header = header;
    }
    public virtual void AddGridViewColumn( GridView gv)
    {
        var ctl = AssemblyUtil.GetMemberAttribute<ControlPropertyAttribute>(Info, false, Parent);
        if (ctl == null) return;
        //当发生修改header宽度时,同时调整这列所有控件宽度
        var fef = ctl.CreateFrameworkElementFactory();
        if (fef == null) return;
        GridViewColumn ch = new GridViewColumn();
        gv.Columns.Add(ch);
        GridViewColumnHeader gch = new GridViewColumnHeader() { Content = header,Height=40};
        ch.Header = gch;

        {
            //实现点击排序
            var bl = true;
            var infoName = Info.Name;
            var comp = MathUtil.Comparison((Info as System.Reflection.PropertyInfo).PropertyType);
            Func<object, object> GetPropertyValue = x => AssemblyUtil.GetMemberValue(x, infoName);
            Comparison<object> smallTobig = (x, y) =>
            {
                return comp(GetPropertyValue(x), GetPropertyValue(y));
            };
            Comparison<object> bigTosmall = (x, y) =>
            {
                return comp(GetPropertyValue(y), GetPropertyValue(x));
            };
            gch.Click += (obj, e) =>
            {
                bl = !bl;
                AssemblyUtil.InvokeMethod(Parent, "Sort", bl ? smallTobig : bigTosmall);

            };
        }
        

        var attr = AssemblyUtil.GetMemberAttribute<WidthAttribute>(Info);
        if (attr != null) ch.Width = attr.value;

        //触发自动调整列宽(但貌似没什么卵用)
        //ch.Width = ch.ActualWidth;
        //ch.Width = Double.NaN;

        var dt = new DataTemplate();
        ch.CellTemplate = dt;


        var binding = new Binding("Width");
        binding.Source = ch;
        binding.Converter = new DoubleToDoubleConverter();
        binding.ConverterParameter = -12;
        fef.SetBinding(FrameworkElement.WidthProperty, binding);
        //SetBindings(fef, attrs, Info.Name);
        dt.VisualTree = fef;

        var attrs = AssemblyUtil.GetMemberAttributes<LinkControlPropertyAttribute>(Info, false, Parent);
        foreach (var att in attrs)
        {
            att.Init(fef, ctl);
        }
        var attrs2 = AssemblyUtil.GetMemberAttributes<LinkControlMemberAttribute>(Info, false, Parent);
        foreach (var att in attrs2)
        {
            att.Init(fef);
        }
    }
}


