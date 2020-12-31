using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 高度
    /// </summary>
    public class HeightAttribute:ControlPropertyAttribute
    {
        /// <summary>
        /// 高度
        /// </summary>
        public HeightAttribute(float value= EditorGUIConfig.Unity_Item_Height) : base(value)
        {

        }
        public HeightAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {

        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.defaultHeight = (float)GetValue();
        }
        public override GUILayoutOption GetGUILayoutOption()
        {
            return GUILayout.Height((float)GetValue());
        }
    }
}
