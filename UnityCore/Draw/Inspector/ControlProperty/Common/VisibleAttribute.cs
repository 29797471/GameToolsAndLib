namespace UnityCore
{
    /// <summary>
    /// 使可见
    /// </summary>
    public class VisibleAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 使可见
        /// </summary>
        public VisibleAttribute(bool value) : base(value)
        {

        }
        public VisibleAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.Visible = (bool)GetValue();
        }
    }
}
