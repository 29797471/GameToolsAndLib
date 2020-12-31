
namespace CqEvent
{
    [MenuItemPath("添加/事件组(G)", null, System.Windows.Input.Key.G)]
    [Editor("事件组")]
    public class EventGroup : BaseTreeNotifyObject, ICustomSerialize
    {
        [Priority(1)]
        [TextBox("说明",100), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Content { get { return mContent; } set { mContent = value; Update("Content"); } }
        public string mContent;

        public string InjectData(string content)
        {
            content = content.Replace("%Name%", Name);
            content = content.Replace("%Content%", Content);
            return content;
        }
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
        [Button,Click("PrintAll"),Margin(20f,0f,0f,0f)]
        public string Btn1 { get { return "开启所有"; } }
        public void PrintAll(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is EventNode)
                {
                    (x as EventNode).Print = true;
                }
            });
        }

        [Priority(2, 3)]
        [Button,Click("PrintAllCanncel"), Margin(20f, 0f, 0f, 0f)]
        public string Btn2 { get { return "取消所有"; } }
        public void PrintAllCanncel(object obj)
        {
            PreOrderTraversal(x =>
            {
                if (x is EventNode)
                {
                    (x as EventNode).Print = false;
                }
            });
        }
    }
}
