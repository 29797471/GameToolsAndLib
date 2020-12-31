using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 摄像机控制脚本
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class CameraColBase : MonoBehaviourExtended
    {
        [Vector("观看点位置")]
        public Vector3 target;

        [TextBox("观看距离"), OnValueChanged("ReCalc")]
        public float dis= 100;


        [TextBox("俯视角度"), OnValueChanged("ReCalc")]
        public float yAlpha=45;
        public float YAlpha
        {
            set
            {
                yAlpha = Mathf.Clamp(value, 0, 89.999f);
            }
            get
            {
                return yAlpha;
            }
        }

        [TextBox("平视方向")]
        public float xzAlpha;

        [TextBox("远近切换速度")]
        public float disSpeed = 1;

        [CheckBox("拖动按下点平移")]
        public bool dragTargetPos=true;

        [Vector("平移速度"), IsEnabled("dragTargetPos", "x=0")]
        public Vector2 tranSpeed = Vector2.one/100;

        [Vector("旋转速度"), OnValueChanged("ReCalc")]
        public Vector2 rotSpeed=Vector2.one/10;

        Camera cam;
        protected Camera Cam
        {
            get
            {
                if (cam == null)
                {
                    cam = GetComponent<Camera>();
                }
                return cam;
            }
        }

        protected Plane yPlane=new Plane(Vector3.up, Vector3.zero);

        [Button("更新"), Click("ReCalc")]
        public string _1;

        public void ReCalc()
        {
            if (Cam.orthographic)
            {
                Cam.orthographicSize = dis;
                yAlpha = 89.999f;
            }
            var xzRad = Mathf.Deg2Rad * xzAlpha;
            var yRad = Mathf.Deg2Rad * yAlpha;
            var y = Mathf.Sin(yRad) * dis;
            var Ro = Mathf.Cos(yRad) * dis;
            transform.position = target + Vector3.up * y + Vector3.right * Mathf.Sin(xzRad) * Ro + Vector3.forward * Mathf.Cos(xzRad) * Ro;
            transform.LookAt(target,Vector3.up);
            CameraChange?.Invoke();
        }
        event Action CameraChange;
        /// <summary>
        /// 通知外部摄像机改变
        /// </summary>
        public void CameraChange_CallBack(Action callBack,ICancelHandle cancelHandle=null)
        {
            CameraChange += callBack;
            if (cancelHandle != null) cancelHandle.CancelAct += () => CameraChange -= callBack;
        }
    }
}
