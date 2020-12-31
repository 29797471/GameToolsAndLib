using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityCore
{
    public class CqPropertyAttribute: PropertyAttribute, CqCore.IMemberAttribute
    {
        public Type InfoType
        {
            get
            {
                return Info.FieldType;
            }
        }
        FieldInfo mInfo;
        protected FieldInfo Info
        {
            get { return mInfo; }
        }
        object mTarget;
        public object Target
        {
            get { return mTarget; }
            private set { mTarget = value; }
        }
        public void SetTarget(MemberInfo info, object target)
        {
            this.mTarget = target;
            mInfo = info as FieldInfo;
            OnSetTarget();
        }

        /// <summary>
        /// 获取修饰属性的值
        /// </summary>
        public object Value
        {
            get
            {
                return Info.GetValue(Target);
            }
            set
            {
                Info.SetValue(Target, value);
            }
        }

        protected virtual void OnSetTarget()
        {

        }
    }
}
