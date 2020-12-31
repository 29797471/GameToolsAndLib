using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 在Scene视图中编辑的特性基类
    /// </summary>
    public class SceneEditAttribute : CqCore.FieldAttribute
    {
        public MonoBehaviour Mono
        {
            get
            {
                if (Parent == null) return null;
                return Parent as MonoBehaviour;
            }
        }
    }
}
