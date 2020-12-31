using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 修饰类成员的特性
    /// </summary>

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MemberAttribute:Attribute, IMemberAttribute
    {
        MemberInfo mInfo;
        protected MemberInfo Info
        {
            get { return mInfo; }
        }
        object mParent;
        protected object Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }
        public void SetTarget(MemberInfo info, object parent)
        {
            this.mParent = parent;
            mInfo = info;
            OnSetTarget();
        }

        protected virtual void OnSetTarget()
        {

        }

    }
}
