using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 编辑样式
    /// </summary>
    public enum ComBoxStyle
    {
        /// <summary>
        /// 单选框
        /// </summary>
        RadioBox,

        /// <summary>
        /// 复选
        /// </summary>
        CheckBox,
    }

    /// <summary>
    /// 可由ItemsAttribute修饰定义列表元素名称
    /// 组合框<para/>
    /// 一般用于枚举单选,多选等等<para/>
    /// 修饰枚举并且使用多选时 <para/>
    /// a.所有枚举值按位递增排列 <para/>
    /// b. 0 全部不选, -1 全部选中, 其他是枚举之和* 枚举值 = 当前下标值 ^ 2* 默认[0 ^ 2 = 1, 1 ^ 2 = 2, 4, 16, .....]
    /// </summary>
    public class ComBoxAttribute: ControlAttribute
    {
        /// <summary>
        /// 编辑样式
        /// </summary>
        public ComBoxStyle style;


        /// <summary>
        /// 组合框<para/>
        /// 一般用于枚举单选,多选等等
        /// </summary>
        public ComBoxAttribute(string name, ComBoxStyle style) : base(name)
        {
            this.style = style;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnSetTarget()
        {
            base.OnSetTarget();
            
        }

        /// <summary>
        /// 获取显示的列表元素
        /// </summary>
        public string[] DisplayMembers;

        /// <summary>
        /// 对象在容器中的显示的成员名称
        /// </summary>
        public string displayMemberName;

        System.Collections.IList mItems;

        /// <summary>
        /// 获取列表元素
        /// </summary>
        public System.Collections.IList Items
        {
            get
            {
                return mItems;
            }
            set
            {
                mItems = value;
            }
        }
    }
}
