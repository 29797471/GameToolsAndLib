using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    public enum MouseButton
    {
        MouseLeft,
        MouseRight,
        MouseMiddle,
    }
    public class MouseInput: BaseInput
    {
        
        event Action<Vector2> OnMove;
        /// <summary>
        /// 鼠标移动
        /// </summary>
        public void Move_CallBack(Action<Vector2> action, ICancelHandle handle = null)
        {
            OnMove += action;
            if (handle != null) handle.CancelAct += () => OnMove -= action;
        }

        
        event Action<float> OnDownMoveByAngle;
        /// <summary>
        /// 基于屏幕中心点,判定鼠标移动的角度变化
        /// </summary>
        public void DownMoveByAngle_CallBack(Action<float> action, ICancelHandle handle = null)
        {
            OnDownMoveByAngle += action;
            if (handle != null) handle.CancelAct += () => OnDownMoveByAngle -= action;
        }

        Vector2 screen_center = new Vector2(Screen.width / 2, Screen.height / 2);

        int button;
        
        Vector2 lastMovePos;
        bool mMouseDown;
        public bool MouseDown
        {
            set
            {
                if(mMouseDown!=value)
                {
                    mMouseDown = value;
                    if (mMouseDown)
                    {
                        if (OnDown != null) OnDown(); 
                        InputMgr.instance.Down();

                        lastMovePos = Input.mousePosition;
                    }
                    else
                    {
                        if (OnUp != null) OnUp();
                        if (InputMgr.instance.IsClick())
                        {
                            if (OnClick != null) OnClick();
                        }
                    }
                }
            }
            get
            {
                return mMouseDown;
            }
        }
        
        
        #region 鼠标中键滚动
        
        event Action<float> OnMouseScroll;
        /// <summary>
        /// 鼠标中键滚动
        /// </summary>
        public void MouseScroll_CallBack(Action<float> action, ICancelHandle handle = null)
        {
            OnMouseScroll += action;
            if (handle != null) handle.CancelAct += () => OnMouseScroll -= action;
        }

        float mouseScrollWheel;
        #endregion
        public MouseInput(MouseButton button)
        {
            this.button = (int)button;
            GlobalMono.Inst.OnUpdate += Update;
        }
        void Update()
        {
            if (!InputMgr.instance.AllowUpdate) return;
            //CqDebug.BeginSample("MouseInput");
            if ( Input.GetMouseButtonDown(button)) MouseDown = true;
            if (Input.GetMouseButtonUp(button)) MouseDown = false;

            if (OnMove != null || OnDownMove != null || OnDownMoveByAngle!=null)
            {
                var mousePosition = (Vector2)Input.mousePosition;
                if (lastMovePos != mousePosition)
                {
                    if (OnMove != null) OnMove(mousePosition - lastMovePos);
                    if (OnDownMove!=null && MouseDown) OnDownMove(mousePosition - lastMovePos);
                    if(OnDownMoveByAngle!=null && MouseDown)
                    {
                        var currentVec = mousePosition - screen_center;
                        var lastVec = lastMovePos - screen_center;
                        OnDownMoveByAngle(Vector2.SignedAngle( lastVec, currentVec));
                    }
                    lastMovePos = mousePosition;
                }
            }
            if (OnMouseScroll != null)
            {
                var temp = Input.GetAxis("Mouse ScrollWheel");
                if (temp != mouseScrollWheel)
                {
                    mouseScrollWheel = temp;
                    OnMouseScroll(mouseScrollWheel);
                }
            }
            //CqDebug.EndSample();
        }
    }
}
