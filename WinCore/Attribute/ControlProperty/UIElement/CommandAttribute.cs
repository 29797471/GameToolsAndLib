using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
/// <summary>
/// 控件快捷键绑定外部函数
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class CommandAttribute : LinkControlPropertyAttribute
{
    public Key key;
    public ModifierKeys modifiers;

    public CommandAttribute(string path, Key key, ModifierKeys modifiers= ModifierKeys.None, AttributeTarget at = 0) 
        : base(path)
    {
        this.key = key;
        this.modifiers = modifiers;
    }
    protected override void OnInit(FrameworkElement fe)
    {
        if(modifiers== ModifierKeys.None)
        {
            var tb = (fe as TextBox);
            tb.PreviewKeyDown += Tb_PreviewKeyDown;
        }
        else
        {
            WinUtil.SetInputCommand(fe, () =>
            {
                AssemblyUtil.InvokeMethod(Parent, path, Parent);
            }, key, modifiers);
        }
    }

    private void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if(e.Key==key)
        {
            AssemblyUtil.InvokeMethod(Parent, path, Parent);
        }
    }
}