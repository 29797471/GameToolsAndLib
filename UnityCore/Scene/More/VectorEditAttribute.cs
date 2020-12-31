using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 在Scene视图中编辑坐标
    /// </summary>
    public class VectorEditAttribute : SceneEditAttribute
    {
        public string Label { get; private set; }

        public Color? Color
        {
            get
            {
                if (ColorPath == null) return null;
                return (Color)AssemblyUtil.GetMemberValue(Parent, ColorPath);
            }
        }
        string ColorPath;
        public VectorEditAttribute(string label,string colorPath=null)
        {
            Label = label;
            ColorPath = colorPath;
        }
    }
}


