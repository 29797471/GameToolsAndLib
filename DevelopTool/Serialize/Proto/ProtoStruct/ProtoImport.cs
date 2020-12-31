
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Proto
{
    [MenuItemPath("添加/导入")]
    [Editor]
    public class ProtoImport : BaseTreeNotifyObject
    {
        [Priority(1)]
        [TextBox("导入"), MinWidth(100)]
        public string Import { get { return mImport; } set { mImport = value; Update("Import"); } }
        public string mImport;
    }
}
