using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 多按钮控件
    /// 一般用于枚举多选等等
    /// 修饰枚举时 所有枚举值按位递增排列 
    /// </summary>
    public class ButtonsAttribute : ControlAttribute
    {

        /// <summary>
        /// 多按钮控件
        /// 一般用于枚举多选等等
        /// 修饰枚举时 所有枚举值按位递增排列 
        /// </summary>
        public ButtonsAttribute(string name) : base(name)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnSetTarget()
        {
            base.OnSetTarget();
            if (Info.FieldType.IsEnum)
            {
                Items = EnumUtil.GetEnumNames(Info.FieldType);
            }
        }

        string[] mItems;
        /// <summary>
        /// 获取显示的列表元素
        /// </summary>
        public string[] Items
        {
            get
            {
                return mItems;
            }
            private set
            {
                mItems = value;
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            int buttonsIntValue = Convert.ToInt32(Value);
            GUILayout.Label(buttonsIntValue.ToString());
            for (int i = 0; i < Items.Length; i++)
            {
                var buttonPressed = MathUtil.StateCheck(buttonsIntValue, 1 << i);

                var buttonPressedNew = GUILayout.Toggle( buttonPressed, Items[i]);

                if (buttonPressedNew != buttonPressed)
                {
                    if(buttonPressedNew)Value = MathUtil.StateAdd(buttonsIntValue, 1 << i);
                    else Value = MathUtil.StateDel(buttonsIntValue, 1 << i);
                }
            }
        }
    }
}

