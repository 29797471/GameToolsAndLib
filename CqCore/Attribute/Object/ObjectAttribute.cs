using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 修饰类/结构的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ObjectAttribute:Attribute, IObjectAttribute
    {
        public void SetTarget( object target)
        {
            mTarget = target;
            OnSetTarget();
        }
        protected virtual void OnSetTarget()
        {

        }
        /// <summary>
        /// 对象
        /// </summary>
        public object Target
        {
            get
            {
                return mTarget;
            }
        }
        object mTarget;
    }
}
