using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

/// <summary>
/// 修饰string 表示一个本地图片形如:"/WinCore;component/Res/find.ico"
/// 或者修饰一个外部图片路径
/// 或者由一个外部文件路径,获取一个图标
/// </summary>
public class ImageAttribute : ControlPropertyAttribute
{
    public ImageAttribute() : base()
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new Image();
        SetPropertyChanged(ctl,() =>
        {
            var path = (string)Target;
            if (FileOpr.IsFilePath(path))
            {
                ctl.Source=ShellIcon.GetImageSource(path);
            }
            else
            {
                ctl.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            }
        });
        return ctl;
    }
    public override FrameworkElementFactory CreateFrameworkElementFactory()
    {
        FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Image));
        var binding = new Binding(Info.Name);
        fef.SetBinding(Image.SourceProperty, binding);
        return fef;
    }
}



