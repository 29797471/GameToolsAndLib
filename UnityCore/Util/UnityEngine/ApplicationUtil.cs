using CqCore;
using System;

namespace UnityEngine
{
    public static class ApplicationUtil
    {
        public static string streamingAssetsPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }
        
        public static string persistentDataPath
        {
            get
            {
                if(!Application.isMobilePlatform && runByMobileDevice)
                {
                    return Application.persistentDataPath+ "_MobileTest";
                }
                return Application.persistentDataPath;
            }
        }

        /// <summary>
        /// 模拟移动设备测试
        /// </summary>
        public static bool runByMobileDevice;

        /// <summary>
        /// 项目目录<para/>
        /// Application.dataPath的上一级目录
        /// </summary>
        public static string ProjPath
        {
            get
            {
                if(mProjPath==null)
                {
                    mProjPath = FileOpr.GetParent(Application.dataPath);
                }
                return mProjPath;
            }
        }
        static string mProjPath;

        public static void logMessageCallBack(Application.LogCallback act,ICancelHandle cancelHandle=null)
        {
            Application.logMessageReceived += act;
            if (cancelHandle!=null)
            {
                cancelHandle.CancelAct+= ()=>Application.logMessageReceived -= act;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {

            //if (CqCore.CqDebug.LogHandle != null) return;

            //if (Application.isMobilePlatform && Application.platform == RuntimePlatform.Android)
            //{
            //    CqCore.CqDebug.LogHandle = (msg, y) =>
            //    {
            //        UnityCore.UnityThread.Call(() =>
            //        {
            //            var _AndroidLog = new AndroidJavaClass("android.util.Log");
            //            _AndroidLog.CallStatic<int>("d", "Unity", y.ToString() + ":" + msg);
            //        });
            //    };
            //}
            //else
            RegUnityLogToCqDebug();
            Vector3Util.RegOperatorEx();
        }

        /// <summary>
        /// 注册unity debug.log 给通用log接口
        /// </summary>
        public static void RegUnityLogToCqDebug()
        {
            CqCore.CqDebug.LogHandle = (x, y) =>
            {
                switch (y)
                {
                    case CqCore.LogType.Error:
                        Debug.LogError(x);
                        break;
                    case CqCore.LogType.Assert:
                        Debug.LogAssertion(x);
                        break;
                    case CqCore.LogType.Warning:
                        Debug.LogWarning(x);
                        break;
                    case CqCore.LogType.Log:
                        Debug.Log(x);
                        break;
                    case CqCore.LogType.Exception:
                        Debug.LogError(x);
                        break;
                    default:
                        break;
                }
            };
        }

        public const string projectSettingsFilePath = @"ProjectSettings\ProjectSettings.asset";
        
        /// <summary>
        ///  获取或者设置版本号
        /// </summary>
        [Obsolete]
        public static string version
        {
            get
            {               
                return Application.version;
            }
            set
            {
                var x = FileOpr.ReadFile(projectSettingsFilePath);
                var y = x.MatchReplace("bundleVersion:.*", (index, mat) =>
                {
                    return "bundleVersion: "+value;
                });
                FileOpr.SaveFile(projectSettingsFilePath, y,true);
            }
        }
    }
}
