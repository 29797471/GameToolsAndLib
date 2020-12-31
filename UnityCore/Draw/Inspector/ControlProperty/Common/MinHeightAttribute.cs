using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 最小高度
    /// </summary>
    public class MinHeightAttribute:ControlPropertyAttribute
    {
        /// <summary>
        /// 高度
        /// </summary>
        public MinHeightAttribute(float value= EditorGUIConfig.Unity_Item_Height) : base(value)
        {

        }
        public MinHeightAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {

        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.defaultHeight = (float)GetValue();
        }
        public override GUILayoutOption GetGUILayoutOption()
        {
            return GUILayout.MinHeight((float)GetValue());
        }
    }
}
