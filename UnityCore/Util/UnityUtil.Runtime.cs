using System;
using UnityEngine;
namespace UnityCore
{
    public static partial class UnityUtil
    { 
        /// <summary>
        /// 注册缓动相关的Unity类型
        /// 发生在启动Unity项目的时候
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void RegUnityType()
        {
            AssemblyUtil.RegType(
            typeof(Vector4),
            typeof(Vector3),
            typeof(Vector2),
            typeof(Quaternion),
            typeof(Color32),
            typeof(Rect),
            typeof(Bounds),
            typeof(Color));
        }
    }
}
