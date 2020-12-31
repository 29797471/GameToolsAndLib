using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 修饰节点类,通过CanExport定义是否导出整个实例
/// </summary>
public interface IExport
{
    bool CanExport { get;}
}
