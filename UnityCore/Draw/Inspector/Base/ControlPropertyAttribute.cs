using System;
using UnityEngine;

namespace UnityCore
{
    public enum CustomConvert
    {
        None,
        BindingPropertyType_To_Type,
        object_To_Type,
    }
    /// <summary>
    /// 控件属性的基类
    /// </summary>
    public class ControlPropertyAttribute: CqPropertyAttribute
    {
        /// <summary>
        /// 获取构造定义的一个关联属性的值
        /// </summary>
        public virtual object GetValue()
        {
            if (value != null) return value;
            if (AssemblyUtil.HasMember(ctl.Target, path))
            {
                var v= AssemblyUtil.GetMemberValue(ctl.Target, path);
                if(convertFunc!=null)
                {
                    return convertFunc(v);
                }
                return v;
            }
            Debug.LogError("未找到关联的字段或者属性 path="+path);
            return null;
        }
        public string path;

        public bool SetValue(object v)
        {
            if (AssemblyUtil.HasMember(ctl.Target, path))return  AssemblyUtil.SetMemberValue(ctl.Target, path,v);
            return false;
        }

        /// <summary>
        /// 依赖数据转换函数
        /// </summary>
        Func<object, object> convertFunc;
        protected object value;
        protected object parameter;
        protected ControlAttribute ctl;

        public ControlPropertyAttribute(object value)
        {
            this.value = value;
        }
        public ControlPropertyAttribute(string path, string convertMethod = null, object parameter = null)
        {
            this.path = path;
            convertFunc= CqCore.Arithmetic.Parse_FxWithEqual(convertMethod);
            this.parameter = parameter;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(ControlAttribute ctl)
        {
            this.ctl = ctl;
            OnInit(ctl);
        }
        protected virtual void OnInit(ControlAttribute ctl)
        {

        }
        public virtual GUILayoutOption GetGUILayoutOption()
        {
            return null;
        }
    }
}
