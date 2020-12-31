using System;
using System.ComponentModel;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// 修饰属性的标签基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyAttribute : MemberAttribute
    {
        protected new PropertyInfo Info
        {
            get { return (PropertyInfo)base.Info; }
        }
        
        /// <summary>
        /// 监听标签修饰的对象改变,返回移除函数
        /// </summary>
        protected void PropertyChanged_CallBack(Action fun,ICancelHandle cancelHandle,bool applyChangedOnce=true)
        {
            var o = Parent as INotifyPropertyChanged;
            if (o != null)
            {
                PropertyChangedEventHandler f = (obj, e) =>
                {
                    if (e.PropertyName == base.Info.Name)
                    {
                        fun?.Invoke();
                    }
                };
                if(applyChangedOnce)fun?.Invoke();
                o.PropertyChanged += f;
                if (cancelHandle != null)
                {
                    cancelHandle.CancelAct += () => o.PropertyChanged -= f;
                }
            }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public object Target
        {
            set
            {
                ((PropertyInfo)base.Info).SetValue(Parent, value,null);
            }
            get
            {
                return ((PropertyInfo)base.Info).GetValue(Parent, null);
            }
        }

        protected override void OnSetTarget()
        {
            
        }
    }
}
