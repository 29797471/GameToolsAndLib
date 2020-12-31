
namespace CodeStyle
{
    /// <summary>
    /// 代码模板组
    /// </summary>
    [MenuItemPath("添加/模板组(G)", null, System.Windows.Input.Key.G)]
    [Editor("模板组")]
    public class CodeStyleGroup : BaseTreeNotifyObject, ICustomSerialize
    {
        [Priority(1)]
        [TextBox("说明", 100), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true), MinWidth(100)]
        public string Content { get { return mContent; } set { mContent = value; Update("Content"); } }
        public string mContent;

        public string InjectData(string content)
        {
            content = content.Replace("%Name%", Name);
            content = content.Replace("%Content%", Content);
            return content;
        }
    }
}

