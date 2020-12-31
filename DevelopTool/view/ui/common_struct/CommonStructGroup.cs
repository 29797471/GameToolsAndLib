using DevelopTool;
using System.Collections.ObjectModel;
using System;

namespace CommonStruct
{

    [MenuItemPath("添加/分组(G)", null, System.Windows.Input.Key.G)]
    [Editor("分组")]
    public class CommonStructGroup : BaseTreeNotifyObject
    {
        [Priority(1)]
        [TextBox("名称空间"),MinWidth(200)]
        public string NameSpace
        {
            get { return mNameSpace; }
            set { mNameSpace = value; Update("NameSpace"); }
        }
        public string mNameSpace;

        [Priority(2, 1)]
        [Label]
        public string Txt
        {
            get
            {
                return "子事件的打印";
            }
        }
        [Priority(2, 2)]
        [Button, Click("PrintAll"), Margin(20f, 0f, 0f, 0f)]
        public string Btn1 { get { return "开启所有"; } }
        public void PrintAll(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is CommonStructNode)
                {
                    (x as CommonStructNode).Print = true;
                }
            });
        }

        [Priority(2, 3)]
        [Button, Click("PrintAllCanncel"), Margin(20f, 0f, 0f, 0f)]
        public string Btn2 { get { return "取消所有"; } }
        public void PrintAllCanncel(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is CommonStructNode)
                {
                    (x as CommonStructNode).Print = false;
                }
            });
        }
    }
}


