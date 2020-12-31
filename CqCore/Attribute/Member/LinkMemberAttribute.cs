using System;
using System.ComponentModel;
namespace CqCore
{
    /// <summary>
    /// 关联同对象下另一属性或者一个固定值
    /// </summary>
    public class LinkMemberAttribute : MemberAttribute
    {
        /// <summary>
        /// 链接的另一个数据改变
        /// </summary>
        protected void PropertyChanged_CallBack(Action<object> fun,ICancelHandle cancelHandle=null,bool applyChangedOnce=true)
        {
            PropertyChangedEventHandler f = (obj, e) =>
            {
                if (e.PropertyName == path) fun?.Invoke(Data);
            };
            if(applyChangedOnce)
            {
                fun?.Invoke(Data);
            }
            var o = Parent as INotifyPropertyChanged;
            if (o != null)
            {
                o.PropertyChanged += f;
                if(cancelHandle!=null)
                {
                    cancelHandle.CancelAct+= () => o.PropertyChanged -= f;
                }
            }
        }

        protected object Data
        {
            get
            {
                if (path != null)
                {
                    var d = AssemblyUtil.GetMemberValue(Parent, path);
                    var dd=ConvertTo(d);
                    if (defaultConvert != null) return defaultConvert(dd);
                    else return dd;
                }
                else return data;
            }
        }
        object data;

        public object DataValue { get { return data; } }
        protected string path;
        /// <summary>
        /// 依赖数据转换函数
        /// </summary>
        Func<object, object> convertFunc;


        /// <summary>
        /// 默认的依赖数据转换函数
        /// </summary>
        protected Func<object, object> defaultConvert;

        protected string convertExpression;
        public LinkMemberAttribute(object data = null)
        {
            this.data = data;
        }
        public LinkMemberAttribute(string path, string convertExpression = null)
        {
            this.path = path;
            this.convertExpression = convertExpression;
            convertFunc =Arithmetic.Parse_FxWithEqual( convertExpression);
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
            if (convertFunc == null) return v;
            return convertFunc(v);
        }
    }
}