using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 控件的基类
    /// </summary>
    public class ControlAttribute: CqPropertyAttribute
    {

        /// <summary>
        /// 前缀名称
        /// </summary>
        public string label;

        /// <summary>
        /// 前缀标签宽度
        /// </summary>
        public float realPrefixWidth=100f;

        public bool Enabled = true;

        public bool Visible = true;

        public string LabelToolTip;

        public string LinkPropertyName;

        /// <summary>
        /// 数据内容高度
        /// </summary>
        float dataHeight;

        public void SetViewLineCount(int num)
        {
            dataHeight = (num-1) * EditorGUIConfig.Unity_TextOneLine_Height+ EditorGUIConfig.Unity_Item_Height;
        }

        /// <summary>
        /// 实际绘制大小
        /// </summary>
        public float RealHeight
        {
            get
            {
                return Mathf.Max(dataHeight, defaultHeight);
            }
        }
        /// <summary>
        /// 默认设置的高度
        /// </summary>
        public float defaultHeight= EditorGUIConfig.Unity_Item_Height;
        public object LinkValue
        {
            get
            {
                if (LinkPropertyName == null) return null;
                return AssemblyUtil.GetMemberValue(Target, LinkPropertyName);
            }
        }

        protected GUILayoutOption[] options;
        /// <summary>
        /// 修饰属性的控件特性的基类
        /// </summary>
        public ControlAttribute(string label = "")
        {
            this.label = label;
        }
        
        /// <summary>
        /// 初始化控件特性,传入它所修饰的对象
        /// </summary>
        protected override void OnSetTarget()
        {
            base.OnSetTarget();
            var _attr=AssemblyUtil.GetClassAttribute<InpectorDrawStyleAttribute>(Target);
            if (_attr != null) realPrefixWidth = _attr.minPrefixLabelWidth;
             var list = new List<GUILayoutOption>();
            var attrs = Attribute.GetCustomAttributes(Info);
            foreach (var attr in attrs)
            {
                var att = attr as ControlPropertyAttribute;
                if (att!=null)
                {
                    att.SetTarget(Info, Target);
                    att.Init(this);
                    var option = att.GetGUILayoutOption();
                    if(option!=null)list.Add(option);
                }
            }
            options = list.ToArray();
        }
        public void Draw()
        {
            OnGUI();
        }
        protected virtual void OnGUI()
        {
            if(!label.IsNullOrEmpty()) GUILayout.Label(label);
        }
        public void OnValueChanged()
        {
            //Debug.Log("组件属性改变");
            var att=AssemblyUtil.GetMemberAttribute<OnValueChangedAttribute>(Info,false,Target);
            if (att != null)
            {
                att.Init(this);
                att.OnValueChange();
            }
        }
    }
}
