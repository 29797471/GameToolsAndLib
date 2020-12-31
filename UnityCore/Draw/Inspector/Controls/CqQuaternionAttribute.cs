using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 编辑一个Quaternion属性
    /// </summary>
    public class CqQuaternionAttribute : CqLabelAttribute
    {
        public CqQuaternionAttribute(string name) : base(name)
        {
        }
        protected override void OnGUI()
        {
            base.OnGUI();
            if (Value is Quaternion)
            {
                var v = (Quaternion)Value;
                GUILayout.Label("x");
                var x = float.Parse(GUILayout.TextField(v.x.ToString()));
                if (x != v.x)
                {
                    v.x = x; Value = v;
                }
                GUILayout.Label("\ty");
                var y = float.Parse(GUILayout.TextField(v.y.ToString()));
                if (y != v.y)
                {
                    v.y = y; Value = v;
                }
                GUILayout.Label("\tz");
                var z = float.Parse(GUILayout.TextField(v.z.ToString()));
                if (z != v.z)
                {
                    v.z = z; Value = v;
                }
                GUILayout.Label("\tw");
                var w = float.Parse(GUILayout.TextField(v.w.ToString()));
                if (w != v.w)
                {
                    v.w = w; Value = v;
                }
            }
        }
    }
}

