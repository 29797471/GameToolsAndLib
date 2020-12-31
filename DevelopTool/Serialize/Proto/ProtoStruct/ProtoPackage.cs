namespace Proto
{
    [MenuItemPath("添加/包")]
    [EditorAttribute]
    public class ProtoPackage : BaseTreeNotifyObject
    {
        [PriorityAttribute(1)]
        [TextBox("包"), MinWidth(100)]
        public string Package { get { return mPackage; } set { mPackage = value; Update("Package"); } }
        public string mPackage;
    }
}
