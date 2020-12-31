using CqCore;
using UnityCore;
using UnityEngine.Events;

namespace UnityEngine
{
    /// <summary>
    /// unity事件扩展
    /// </summary>
    public static class UnityEventUtil
    {
        /// <summary>
        /// 注册一个unity事件委托,并托管给一个对象释放
        /// </summary>
        public static void SetCallBack(this UnityEvent ue, UnityAction fun, ICancelHandle handle = null)
        {
            ue.AddListener(fun);
            if (handle != null)
            {
                handle.CancelAct += () => ue.RemoveListener(fun);
            }
        }

        /// <summary>
        /// 注册一个unity事件委托,并托管给一个对象释放
        /// </summary>
        public static void SetCallBack<T>(this UnityEvent<T> ue, UnityAction<T> fun,ICancelHandle handle=null)
        {
            ue.AddListener(fun);
            if(handle!=null)
            {
                handle.CancelAct+= () => ue.RemoveListener(fun);
            }
        }
        /// <summary>
        /// 注册一个unity事件委托,并托管给一个对象释放
        /// </summary>
        public static void SetCallBack(this UnityEventBase ueb, BindingFPType type, UnityAction<object> fun, ICancelHandle handle = null)
        {
            switch (type)
            {
                case BindingFPType.System_String:
                    {
                        var obj = ueb as UnityEvent<string>;
                        obj.SetCallBack(v=>fun(v), handle);
                        break;
                    }
                case BindingFPType.System_Boolean:
                    {
                        var obj = ueb as UnityEvent<bool>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.System_Int32:
                    {
                        var obj = ueb as UnityEvent<int>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.System_Single:
                    {
                        var obj = ueb as UnityEvent<float>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Vector2:
                    {
                        var obj = ueb as UnityEvent<Vector2>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Vector3:
                    {
                        var obj = ueb as UnityEvent<Vector3>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Vector4:
                    {
                        var obj = ueb as UnityEvent<Vector4>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Quaternion:
                    {
                        var obj = ueb as UnityEvent<Quaternion>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Color:
                    {
                        var obj = ueb as UnityEvent<Color>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Color32:
                    {
                        var obj = ueb as UnityEvent<Color32>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Sprite:
                    {
                        var obj = ueb as UnityEvent<Sprite>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.UnityEngine_Texture:
                    {
                        var obj = ueb as UnityEvent<Texture>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
                case BindingFPType.System_Object:
                    {
                        var obj = ueb as UnityEvent<object>;
                        obj.SetCallBack(v => fun(v), handle);
                        break;
                    }
            }
        }
    }
}
