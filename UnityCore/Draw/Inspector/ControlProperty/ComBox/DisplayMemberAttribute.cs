using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCore
{
    /// <summary>
    /// 提供给外部获取对象在容器中的名称
    /// 并且当属性值变更时通知给外部
    /// </summary>
    public class DisplayMemberAttribute : ControlPropertyAttribute
    {
        public string display;
        public DisplayMemberAttribute(string display) : base(null)
        {
            this.display = display;
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            if (ctl is ComBoxAttribute)
            {
                var cb = ctl as ComBoxAttribute;
                cb.displayMemberName = display;
            }
        }
    }
}
