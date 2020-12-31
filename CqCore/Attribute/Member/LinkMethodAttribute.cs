using System;
using System.Reflection;
using CqCore;
using System.ComponentModel;

namespace CqCore
{
    /// <summary>
    /// 链接所在对象的一个方法
    /// </summary>
    public class LinkMethodAttribute : MethodAttribute
    {
        protected string methodName;
        public LinkMethodAttribute(string methodName)
        {
            this.methodName = methodName;
        }
        protected override void OnSetTarget()
        {
            //var o = Parent as INotifyMemberChanged;
            //if (o!=null)
            //{
            //    o.PropertyChanged += (obj,e)=>
            //    {
            //        if (e.PropertyName == path) PropertyChanged?.Invoke(Data);
            //    };
            //}
        }
    }
}
