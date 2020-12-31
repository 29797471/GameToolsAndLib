using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 单手指触屏输入
    /// </summary>
    public class TouchInputOne:BaseInput
    {
        public TouchInputOne()
        {
            GlobalMono.Inst.OnFixedUpdate += Update;
        }
        
        bool mDown;
        Vector2 lastTouchPos;
        public Vector2 TouchPos
        {
            set
            {
                if (lastTouchPos == value) return;
                var temp = lastTouchPos;
                lastTouchPos = value;
                if(OnDownMove != null) OnDownMove(lastTouchPos - temp);
            }
            get
            {
                return lastTouchPos;
            }
        }
        
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
                    if (OnDown!=null) OnDown();
                    InputMgr.instance.Down();

                    lastTouchPos = t0.position;
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
        void Update()
        {
            if (!InputMgr.instance.AllowUpdate) return;
            Down = (Input.touchCount == 1);
            if(mDown)
            {
                var t0 = Input.GetTouch(0);
                if (t0.phase == TouchPhase.Moved)
                {
                    TouchPos = t0.position;
                }
                else
                {
                    //Debug.Log(t0.phase.ToString());
                }
            }
        }
    }
}
