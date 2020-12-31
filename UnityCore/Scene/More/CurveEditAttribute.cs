using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 在Scene视图中编辑曲线
    /// 修饰List_Vector2,
    /// </summary>
    public class CurveEditAttribute : SceneEditAttribute
    {
        
        float a;
        /// <summary>
        /// 多点转连续贝塞尔曲线系数1
        /// </summary>
        public float A
        {
            get
            {
                if (aName == null) return a;
                return (float)AssemblyUtil.GetMemberValue(Parent, aName);
            }
        }
        string aName;

        
        float b;
        /// <summary>
        /// 多点转连续贝塞尔曲线系数2
        /// </summary>
        public float B
        {
            get
            {
                if (bName == null) return b;
                return (float)AssemblyUtil.GetMemberValue(Parent, bName);
            }
        }
        string bName;

        bool isClose;
        string isCloseName;
        public bool IsClose
        {
            get
            {
                if (isCloseName == null) return isClose;
                return (bool)AssemblyUtil.GetMemberValue(Parent, isCloseName);
            }
        }
        /// <summary>
        /// 在Scene视图中编辑曲线
        /// 修饰List_Vector2,
        /// </summary>
        public CurveEditAttribute(bool isClose = true,float a = 0.25f, float b = 0.25f)
        {
            this.a = a;
            this.b = b;
            this.isClose = isClose;
        }

        /// <summary>
        /// 在Scene视图中编辑曲线
        /// 修饰List_Vector2,
        /// </summary>
        public CurveEditAttribute(string isCloseName,string aName, string bName )
        {
            this.aName = aName;
            this.bName = bName;
            this.isCloseName = isCloseName;
        }
    }
}


