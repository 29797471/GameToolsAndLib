using CqCore;
using System.Collections;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 摄像机输入控制脚本<para/>
    /// 同时兼容鼠标和触屏操作
    /// </summary>
    public class CameraInputCol : CameraColBase
    {
        /// <summary>
        /// 触屏与鼠标的参数控制系数
        /// </summary>
        static float argK=1f;


        private void OnEnable()
        {
            argK = Input.touchSupported ? 1000f : 1f;
            if (Input.touchSupported) InputMgr.instance.DoubleTouch.MoveByDis_CallBack(MoveNearOrFar, DisabledHandle);
            else InputMgr.instance.MouseMiddle.MouseScroll_CallBack(MoveNearOrFar, DisabledHandle);

            if (Input.touchSupported) InputMgr.instance.DoubleTouch.MoveByAngle_CallBack(RotatingOrthographic, DisabledHandle);
            else InputMgr.instance.MouseRight.DownMoveByAngle_CallBack(RotatingOrthographic, DisabledHandle);

            if (Input.touchSupported) InputMgr.instance.DoubleTouch.DownMove_CallBack(RotatingPerspective, DisabledHandle);
            else InputMgr.instance.MouseRight.DownMove_CallBack(RotatingPerspective, DisabledHandle);

            if (Input.touchSupported) InputMgr.instance.OneTouch.DownMove_CallBack(Move, DisabledHandle);
            else InputMgr.instance.MouseLeft.DownMove_CallBack(Move, DisabledHandle);
            ReCalc();
        }

        /// <summary>
        /// 抖动
        /// </summary>
        [ContextMenu("抖动一次")]
        public void Shake()
        {
            StartCoroutine(Shake_IT(5));
        }
        IEnumerator Shake_IT(float seconds = 1f)
        {
            var originalProjection = Cam.projectionMatrix;

            var endTick = GlobalCoroutine.GetTickTime(seconds);
            while (GlobalCoroutine.Tick < endTick)
            {
                var p = originalProjection;
                p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
                p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
                Cam.projectionMatrix = p;
                yield return null;
            }
            Cam.projectionMatrix = originalProjection;
        }

        /// <summary>
        /// 旋转透视摄像机<para/>
        /// 鼠标:右键按下旋转,触屏:双指同方向移动
        /// </summary>
        void RotatingPerspective(Vector2 delta)
        {
            if (Cam.orthographic) return;
            xzAlpha += delta.x * rotSpeed.x;
            YAlpha += delta.y * -rotSpeed.y;
            ReCalc();
        }

        /// <summary>
        /// 旋转正交摄像机<para/>
        /// 鼠标:右键按下绕屏幕中心旋转,触屏:双指扭动
        /// </summary>
        void RotatingOrthographic(float angle)
        {
            if (!Cam.orthographic) return;
            xzAlpha += angle;
            ReCalc();
        }

        /// <summary>
        /// 拉近拉远摄像机<para/>
        /// 鼠标:中间滚动,触屏:双指夹取
        /// </summary>
        void MoveNearOrFar(float k)
        {
            dis *= (1 - k * disSpeed / argK);

            ReCalc();
        }

        /// <summary>
        /// 移动摄像机<para/>
        /// </summary>
        void Move(Vector2 d)
        {
            var delta = new Vector3(d.x, d.y, 0);
            /*按住Ctrl 鼠标左键移动改变俯视角度
            if (InputMgr.instance.KeyBoardInst.IsKeyDown(KeyCode.LeftControl) ||
                InputMgr.instance.KeyBoardInst.IsKeyDown(KeyCode.RightControl))
            {
                YAlpha += delta.y * -rotSpeed.y;
                ReCalc();
            }
            else*/
            {
                if (dragTargetPos)
                {
                    var currentPos = Cam.MouseRayCrossYPlane();
                    var lastPos = Cam.ScreenToWorldPoint(Input.mousePosition - (Vector3)delta, yPlane);
                    if (lastPos != null && currentPos != null)
                    {
                        var deltaTarget = (Vector3)lastPos - (Vector3)currentPos;
                        target += deltaTarget;
                        ReCalc();
                    }
                }
                else
                {
                    //delta = transform.localToWorldMatrix.MultiplyVector(delta);
                    delta.Scale(tranSpeed);
                    var dragSpeed = delta * Mathf.Sqrt(dis);
                    var camY = target - Cam.transform.position;
                    camY.y = 0;
                    camY.Normalize();
                    var camX = camY.ToVector2().Rot90().ToVector3();
                    target += camX * dragSpeed.x + camY * -dragSpeed.y;
                    ReCalc();
                }
            }
        }
    }
}
