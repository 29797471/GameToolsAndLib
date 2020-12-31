using System.Collections.Generic;

namespace Trigger
{
    [MenuItemPath("添加/触发组(G)", null, System.Windows.Input.Key.G)]
    [Editor("触发组")]
    public class TriggerGroup : BaseTreeNotifyObject
    {
        [Priority(1)]
        [TextBox("说明", 100), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Content { get { return mContent; } set { mContent = value; Update("Content"); } }
        public string mContent;

        [Priority(2, 2)]
        [Button, Click("PrintAll")]
        public string Btn1 { get { return "开启所有"; } }
        public void PrintAll(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is TriggerNode)
                {
                    (x as TriggerNode).CanExport = true;
                }
            });
        }

        [Priority(2, 3) ,Margin(20f, 0f,0f,0f)]
        [Button, Click("PrintAllCanncel")]
        public string Btn2 { get { return "禁用所有"; } }
        public void PrintAllCanncel(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is TriggerNode)
                {
                    (x as TriggerNode).CanExport = false;
                }
            });
        }
    }
}
