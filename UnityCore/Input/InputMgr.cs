using System.Collections.Generic;
using UnityCore;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 设备输入管理器
/// </summary>
public class InputMgr : Singleton<InputMgr>
{
    KeyBoardInput mKeyBoardInst;
    public KeyBoardInput KeyBoardInst
    {
        get
        {
            if (mKeyBoardInst == null)
            {
                mKeyBoardInst = new KeyBoardInput();
            }
            return mKeyBoardInst;
        }
    }
    MouseInput mMouseLeft;
    public MouseInput MouseLeft
    {
        get
        {
            if (mMouseLeft==null)
            {
                mMouseLeft = new MouseInput(MouseButton.MouseLeft);
            }
            return mMouseLeft;
        }
    }

    MouseInput mMouseRight;
    public MouseInput MouseRight
    {
        get
        {
            if (mMouseRight == null)
            {
                mMouseRight = new MouseInput(MouseButton.MouseRight);
            }
            return mMouseRight;
        }
    }

    MouseInput mMouseMiddle;
    public MouseInput MouseMiddle
    {
        get
        {
            if (mMouseMiddle == null)
            {
                mMouseMiddle = new MouseInput(MouseButton.MouseMiddle);
            }
            return mMouseMiddle;
        }
    }

    TouchInputDouble mDoubleTouch;
    public TouchInputDouble DoubleTouch 
    { 
        get
        {
            if (mDoubleTouch == null)
            {
                mDoubleTouch = new TouchInputDouble();
            }
            return mDoubleTouch;
        }
    }

    TouchInputOne mOneTouch;
    public TouchInputOne OneTouch
    {
        get
        {
            if (mOneTouch == null)
            {
                mOneTouch = new TouchInputOne();
            }
            return mOneTouch;
        }
    }

    /// <summary>
    /// 可穿透UGUI触发输入事件
    /// </summary>
    public bool AllowByOverUGUI { get; set; }

    /// <summary>
    /// 允许处理设备事件,发出对应消息
    /// </summary>
    internal bool AllowUpdate
    {
        get
        {
            return AllowByOverUGUI || !IsPointerOverUIObject;
        }
    }

    PointerEventData eventDataCurrentPosition;
    List<RaycastResult> results;
    /// <summary>
    /// 点击在UGUI上
    /// -1代表鼠标
    /// </summary>
    internal bool IsPointerOverUIObject
    {
        get
        {
            //判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
            if (EventSystem.current == null) return false;
            if(eventDataCurrentPosition==null)
            {
                eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            }
            
            eventDataCurrentPosition.position = Input.mousePosition;

            if (results == null) results = new List<RaycastResult>();
            
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            var bl = results.Count > 0;
            results.Clear();
            return bl;
        }
    }

    /// <summary>
    /// 按下放手时间间隔内视为点击
    /// </summary>
    public const float ClickMaxTime = 0.5f;

    /// <summary>
    /// 按下放手位置相差像素单位内视为点击
    /// </summary>
    public const float ClickPosDis = 10f;


    float lastDownTime;
    Vector2 lastDownPos;

    /// <summary>
    /// 按下和放手
    /// </summary>
    /// <returns></returns>
    internal bool IsClick()
    {
        var realTime = Time.realtimeSinceStartup;
        var mousePosition = (Vector2)Input.mousePosition;
        //Debug.Log("TouchUp deltaTime=" + (realTime - lastDownTime) + " dis:" + (Vector2.Distance(mousePosition, lastDownPos)));
        if (realTime - lastDownTime <= ClickMaxTime)
        {
            if (Vector2.Distance(mousePosition, lastDownPos) <= ClickPosDis)
            {
                return true;
            }
        }
        return false;
    }
    internal void Down()
    {
        //if(Input.touchSupported)
        //{
        //    Debug.Log("DownTouch:" + Torsion.Serialize(Input.GetTouch(0).position));
        //    Debug.Log("Down:" + Torsion.Serialize(Input.mousePosition));
        //}
        //else
        //{
        //    Debug.Log("Down:" + Torsion.Serialize(Input.mousePosition));
        //}
        lastDownTime = Time.realtimeSinceStartup;
        lastDownPos = Input.mousePosition;
    }
}