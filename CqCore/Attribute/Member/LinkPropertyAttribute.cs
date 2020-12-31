using System;
using System.ComponentModel;

namespace CqCore
{
    /// <summary>
    /// 修饰一个属性,并提供一个数据或者关联同对象下的另一个属性
    /// </summary>
    public class LinkPropertyAttribute : PropertyAttribute
    {
        /// <summary>
        /// 链接的另一个数据改变
        /// 返回移除链接的方法
        /// </summary>
        protected void LinkPropertyChanged_CallBack(Action<object> fun,ICancelHandle handle,bool applyChangedOnce=true)
        {
            var o = Parent as INotifyPropertyChanged;
            if (o == null) return;
            PropertyChangedEventHandler f = (obj, e) =>
            {
                if (e.PropertyName == path) fun?.Invoke(Data);
            };
            if(applyChangedOnce)
            {
                fun?.Invoke(Data);
            }
            o.PropertyChanged += f;
            if(handle!=null)
            {
                handle.CancelAct+= () => o.PropertyChanged -= f;
            }
        }

        /// <summary>
        /// 另一个属性值
        /// </summary>
        protected object Data
        {
            get
            {
                if (path != null) return ConvertTo(AssemblyUtil.GetMemberValue(Parent, path));
                else return data;
            }
            set
            {
                if (path != null) ConvertTo(AssemblyUtil.SetMemberValue(Parent, path, value));
            }
        }
        object data;
        protected string path;
        /// <summary>
        /// 依赖数据转换函数
        /// </summary>
        string convertMethod;

        /// <summary>
        /// 转换函数参数
        /// </summary>
        object arg0;

        public LinkPropertyAttribute(object data=null)
        {
            this.data = data; 
        }
        public LinkPropertyAttribute(string path, string convertMethod=null, object arg0=null)
        {
            this.path = path;
            this.convertMethod = convertMethod;
            this.arg0 = arg0;
        }
        protected override void OnSetTarget()
        {
            base.OnSetTarget();
        }

        /// <summary>
        /// 转换函数
        /// </summary>
        protected object ConvertTo(object v)
        {
            if (convertMethod == null) return v;
            if(arg0==null)return AssemblyUtil.InvokeMethod(Parent, convertMethod, v);
            return AssemblyUtil.InvokeMethod(Parent, convertMethod,v, arg0);
        }
    }
}
