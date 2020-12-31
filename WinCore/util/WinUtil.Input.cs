using CqCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCore;

public static partial class WinUtil
{
    /// <summary>
    /// 控件监听设备输入,执行回调
    /// 一定是组合键,不能处理单独的按键响应
    /// </summary>
    public static void SetInputCommand(UIElement ctl, Action OnClick, Key key, ModifierKeys modifiers )
    {
        if (key == Key.None ||  modifiers== ModifierKeys.None) return;
        CommandBinding c = new CommandBinding() { Command = new RoutedUICommand() };
        c.Executed += (obj, e) =>
        {
            OnClick?.Invoke();
        };
        ctl.CommandBindings.Add(c);
        ctl.InputBindings.Add(new KeyBinding(c.Command, key, modifiers));
    }
    /// <summary>
    /// 控件监听设备输入,执行回调
    /// (同上)区别是,可以是组合键也可以是单独按键
    /// </summary>
    public static void SetInputCommandX(UIElement ctl, Action OnClick, Key key = Key.None, ModifierKeys modifiers = ModifierKeys.None)
    {
        KeyEventHandler fun = (sender, e) =>
        {
            if (e.Key == key && Keyboard.Modifiers==modifiers)
            {
                OnClick?.Invoke();
            }
        };
        ctl.AddHandler(Keyboard.KeyDownEvent, fun);
        (ctl as FrameworkElement).Unloaded+=(obj,e)=>
        ctl.RemoveHandler(Keyboard.KeyDownEvent, fun);
    }

    
    
}
