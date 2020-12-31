using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 编辑一个向量的属性
    /// Vector2 Vector3 Vector4
    /// </summary>
    public class VectorAttribute : ControlAttribute
    {
        public VectorAttribute(string name) : base(name)
        {
        }
        protected override void OnGUI()
        {
            base.OnGUI();
            if (Value is Vector4)
            {
                var v = (Vector4)Value;
                GUILayout.Label("x");
                var x=float.Parse(GUILayout.TextField(v.x.ToString()));
                if (x != v.x)
                {
                    v.x = x; Value = v;
                }
                GUILayout.Label("\ty");
                var y=float.Parse(GUILayout.TextField(v.y.ToString()));
                if (y != v.y)
                {
                    v.y = y; Value = v;
                }
                GUILayout.Label("\tz");
                var z=float.Parse(GUILayout.TextField(v.z.ToString()));
                if(z!=v.z)
                {
                    v.z = z;Value = v;
                }
                GUILayout.Label("\tw");
                var w=float.Parse(GUILayout.TextField(v.w.ToString()));
                if(w!=v.w)
                {
                    v.w = w;Value = v;
                }
            }
            else if (Value is Vector3)
            {
                var v = (Vector3)Value;
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
            }
            else if (Value is Vector2)
            {
                var v = (Vector2)Value;
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
            }
        }
    }

}
