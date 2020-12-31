using System;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// 修饰方法的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MethodAttribute : MemberAttribute
    {
        protected new MethodInfo  Info
        {
            get { return (MethodInfo)base.Info; }
        }
        public object Exec(params object[] args)
        {
            return ((MethodInfo)Info).Invoke(Parent, args);
        }
    }

}
