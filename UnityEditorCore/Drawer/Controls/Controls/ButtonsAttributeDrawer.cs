using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using CqCore;
using UnityCore;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制多按钮(修饰枚举变量,位的方式)
    /// </summary>
    [CustomPropertyDrawer(typeof(ButtonsAttribute))]
    public class ButtonsAttributeDrawer : CqPropertyDrawer<ButtonsAttribute>
    {
        private const float startX = 200f;//按钮起始位置
        /// <summary>
        /// 当面板绘制时
        /// </summary>
        public override System.Action OnCqGUI(SerializedProperty property)
        {
            int buttonsIntValue = 0;
            EditorGUI.LabelField(BaseDrawControl(0, 100, 0, startX, 20f), property.intValue.ToString());

            for (int i = 0; i < attribute.Items.Length; i++)
            {
                var buttonPressed = MathUtil.StateCheck(property.intValue, 1 << i);

                buttonPressed=GUI.Toggle(BaseDrawControl(
                    1f*i / attribute.Items.Length ,
                    startX * (attribute.Items.Length-i)/ attribute.Items.Length + 5,
                    1f*(i+1)/ attribute.Items.Length,
                    startX * (attribute.Items.Length - i-1) / attribute.Items.Length - 5,
                    20f), buttonPressed, attribute.Items[i], "Button");

                if (buttonPressed) buttonsIntValue =MathUtil.StateAdd(buttonsIntValue, 1 << i);
            }

            return ()=>
            {
                if (property.intValue != buttonsIntValue) property.intValue = buttonsIntValue;
            };
        }
    }
}

