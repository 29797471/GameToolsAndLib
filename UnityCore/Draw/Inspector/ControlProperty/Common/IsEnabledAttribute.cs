namespace UnityCore
{
    /// <summary>
    /// 使可用
    /// </summary>
    public class IsEnabledAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 使可用
        /// </summary>
        public IsEnabledAttribute(bool value) : base(value)
        {

        }
        public IsEnabledAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.Enabled = (bool)GetValue();
        }
    }
}
