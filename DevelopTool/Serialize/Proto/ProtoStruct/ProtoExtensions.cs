namespace Proto
{
    [EditorAttribute]
    public class ProtoExtensions : BaseTreeNotifyObject
    {
        [PriorityAttribute(1)]
        [TextBox("扩展"), MinWidth(100)]
        public int Extensions { get { return mExtensions; } set { mExtensions = value; Update("Extensions"); } }
        public int mExtensions;
    }
}
