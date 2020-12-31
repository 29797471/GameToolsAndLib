using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 提供给外部获取对象在容器中的名称
/// 并且当属性值变更时通知给外部
/// </summary>
public class DisplayMemberAttribute : LinkControlMemberAttribute
{
    public string display;
    public DisplayMemberAttribute(string display) : base()
    {
        this.display = display;
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if (fe is ItemsControl)
        {
            var listBox = fe as ItemsControl;
            listBox.DisplayMemberPath = display;
        }
    }
}