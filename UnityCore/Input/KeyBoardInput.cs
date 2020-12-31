using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// Input.GetKey按键按下期间返回true
    /// Input.GetKeyDown按键按下的第一帧返回true
    /// Input.GetKeyUp按键松开的第一帧返回true
    /// </summary>
    public class KeyBoardInput
    {
        Array keyCodes;
        public KeyBoardInput()
        {
            downList = new List<KeyCode>();
            keyCodes = Enum.GetValues(typeof(KeyCode));
            GlobalMono.Inst.OnUpdate += Update;
        }
        
        event Action<KeyCode> OnKeyDown;
        /// <summary>
        /// 按键按下
        /// </summary>
        public void KeyDown_CallBack(Action<KeyCode> action, ICancelHandle handle = null)
        {
            OnKeyDown += action;
            if (handle != null) handle.CancelAct += () => OnKeyDown -= action;
        }

        event Action<KeyCode> OnKeyUp;
        /// <summary>
        /// 按键抬起
        /// </summary>
        public void KeyUp_CallBack(Action<KeyCode> action, ICancelHandle handle = null)
        {
            OnKeyUp += action;
            if (handle != null) handle.CancelAct += () => OnKeyUp -= action;
        }

        List<KeyCode> downList;

        public bool IsKeyDown(KeyCode keyCode)
        {
            return downList.Contains(keyCode);
        }

        void Update()
        {
            //CqCore.CqDebug.BeginSample("KeyBoardInput");
            Event e = Event.current;
            if (Input.anyKeyDown)
            {
                for(int i=0;i<keyCodes.Length;i++)
                {
                    var keyCode = (KeyCode)keyCodes.GetValue(i);
                    if (Input.GetKeyDown(keyCode))
                    {
                        if(OnKeyDown!=null) OnKeyDown(keyCode);
                        if(!downList.Contains(keyCode)) downList.Add(keyCode);
                    }
                }
            }
            for (int i = downList.Count - 1; i >= 0; i--)
            {
                var key = downList[i];
                if (Input.GetKeyUp(key))
                {
                    if (OnKeyUp != null) OnKeyUp(key);
                    downList.Remove(key);
                }
            }
            //CqCore.CqDebug.EndSample();
        }
    }
}
