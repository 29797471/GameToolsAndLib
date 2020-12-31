using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

/// <summary>
/// 控件特性接口,控件特性声明这个接口
/// </summary>
public interface IControlAttriubte
{
    /// <summary>
    /// 添加控件到面板parent
    /// </summary>
    void AddControl(Panel parent);
}
