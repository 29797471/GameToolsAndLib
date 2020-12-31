namespace UnityCore
{
    /// <summary>
    /// 悬浮提示 不同于UnityEngine.TooltipAttriubte
    /// 它依赖于控件,不独立存在
    /// </summary>
    public class ToolTipAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 悬浮提示
        /// </summary>
        public ToolTipAttribute(string value) : base((object)value)
        {

        }
        public ToolTipAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.LabelToolTip = (string)GetValue();
        }
    }
}

