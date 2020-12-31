namespace UnityCore
{
    /// <summary>
    /// 列表框
    /// </summary>
    public class ListBoxAttribute:ControlAttribute
    {
        public ListBoxAttribute(string label):base(label)
        {
        }

        protected override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
