using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 绘制区域
    /// </summary>
    public class DrawRectAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 绘制区域
        /// </summary>
        public DrawRectAttribute(float startPercent = 0, float startOffset = 100, float endPercent = 1, float endOffset = 0) : 
            base(new Vector4(startPercent, startOffset, endPercent, endOffset))
        {

        }


        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            //ctl.rect = (Vector4)GetValue();
        }
    }
}

