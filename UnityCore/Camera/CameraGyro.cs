using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 陀螺仪实现全景图效果
    /// </summary>
    public class CameraGyro : MonoBehaviour
    {
        private Quaternion quatMult = new Quaternion(0, 0, 1, 0);   // 沿着 Z 周旋转的四元数 因子

        [TextBox("陀螺仪跟随速度")]
        public float speedH = 0.2f;    //差值

        ScreenOrientation last= ScreenOrientation.AutoRotation;

        /// <summary>
        /// 陀螺仪开启后固定的屏幕方向
        /// </summary>
        [ComBox("固定屏幕方向",ComBoxStyle.RadioBox)]
        public ScreenOrientation enterScreen= ScreenOrientation.Landscape;
        void Start()
        {
            if(!Application.isMobilePlatform)
            {
                enabled = false;
                return;
            }

            //激活陀螺仪
            Input.gyro.enabled = true;
        }
        private void OnEnable()
        {
            //禁止自动屏幕旋转
            //last = Screen.orientation;
            Screen.orientation = enterScreen;
        }
        private void OnDisable()
        {
            Screen.orientation = last;
        }
        /// <summary>
        /// 因安卓设备的陀螺仪四元数水平值为[0,0,0,0]水平向下，所以将相机初始位置修改与其对齐
        /// 相当于父级角度90,90,0
        /// </summary>
        Quaternion baseQuat = Quaternion.Euler(90, 90, 0);


        Quaternion gyroscopeQua;    //陀螺仪四元数

        /// <summary>
        /// 陀螺仪运动更新摄像机
        /// </summary>
        protected void Update()
        {
            gyroscopeQua = baseQuat * (attitude * quatMult);      //为球面运算做准备

            transform.localRotation = Quaternion.Slerp(transform.localRotation, gyroscopeQua, speedH);
        }
        Quaternion attitude
        {
            get
            {
                //return GyroDownload.attitude;
                return Input.gyro.attitude;
            }
        }

    }
}
