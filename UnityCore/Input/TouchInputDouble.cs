using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 双手指触屏输入
    /// </summary>
    public class TouchInputDouble:BaseInput
    {
        public TouchInputDouble()
        {
            GlobalMono.Inst.OnFixedUpdate += Update;
        }

        
        event Action<float> OnMoveByDis;

        /// <summary>
        /// 按下移动时两手指间距离变化
        /// </summary>
        public void MoveByDis_CallBack(Action<float> action, ICancelHandle handle = null)
        {
            OnMoveByDis += action;
            if (handle != null) handle.CancelAct += () => OnMoveByDis -= action;
        }

        event Action<float> OnMoveByAngle;
        /// <summary>
        /// 按下移动时两手指间角度变化(弧度)
        /// </summary>
        public void MoveByAngle_CallBack(Action<float> action, ICancelHandle handle = null)
        {
            OnMoveByAngle += action;
            if (handle != null) handle.CancelAct += () => OnMoveByAngle -= action;
        }

        Vector2 lastDir;
        Vector2 lastCenterPos;
        /// <summary>
        /// 当两手移动间距变化量小于这个值视为平移,否则视为pinch
        /// </summary>
        const float diffMoveK = 10;
        
        bool mDown;
        public bool Down
        {
            get
            {
                return mDown;
            }
            set
            {
                if (mDown == value) return;
                mDown = value;
                if(mDown)
                {
                    var t0 = Input.GetTouch(0);
                    var t1 = Input.GetTouch(1);
                    lastDir = t0.position - t1.position;
                    lastCenterPos = (t0.position + t1.position) / 2;
                    if (OnDown!=null) OnDown();
                }
                else
                {
                    if (OnUp != null) OnUp();
                }
            }
        }
        void Update()
        {
            if (!InputMgr.instance.AllowUpdate) return;
            Down = (Input.touchCount == 2);
            if(mDown)
            {
                var t0 = Input.GetTouch(0);
                var t1 = Input.GetTouch(1);
                if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
                {
                    var currentDir = t0.position - t1.position;
                    var currentCenterPos = (t0.position + t1.position) / 2;
                    var disDelta = currentDir.magnitude - lastDir.magnitude;
                    if (Math.Abs(disDelta) < diffMoveK)
                    {
                        if (OnDownMove != null)
                        {
                            OnDownMove(currentCenterPos-lastCenterPos);
                        }
                    }
                    else
                    {
                        if (OnMoveByDis != null)
                        {
                            OnMoveByDis(currentDir.magnitude - lastDir.magnitude);
                        }
                    }
                    
                    if (OnMoveByAngle != null)
                    {
                        OnMoveByAngle(Vector2.SignedAngle(lastDir, currentDir));
                    }
                    lastDir = currentDir;
                    lastCenterPos = currentCenterPos;
                }
            }
        }
    }
}
