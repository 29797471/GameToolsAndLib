using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    public abstract class BaseInput
    {
        /// <summary>
        /// 按下
        /// </summary>
        protected Action OnDown;
        public void Down_CallBack(Action action, ICancelHandle handle = null)
        {
            OnDown += action;
            if (handle != null) handle.CancelAct += () => OnDown -= action;
        }

        /// <summary>
        /// 脱离按下
        /// </summary>
        protected Action OnUp;
        public void Up_CallBack(Action action, ICancelHandle handle = null)
        {
            OnUp += action;
            if (handle != null) handle.CancelAct += () => OnUp -= action;
        }

        /// <summary>
        /// 点击
        /// </summary>
        protected Action OnClick;
        public void Click_CallBack(Action action, ICancelHandle handle = null)
        {
            OnClick += action;
            if (handle != null) handle.CancelAct += () => OnClick -= action;
        }

        /// <summary>
        /// 按下移动时产生移动增量
        /// </summary>
        protected Action<Vector2> OnDownMove;

        public void DownMove_CallBack(Action<Vector2> action, ICancelHandle handle = null)
        {
            OnDownMove += action;
            if (handle != null) handle.CancelAct += () => OnDownMove -= action;
        }
    }
}
