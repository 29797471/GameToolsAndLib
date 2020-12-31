using CqCore;
using System;
using System.Collections;
using UnityEditor;

namespace UnityEditorCore
{
    public class ProgressBarData
    {
        public string info;
        public string message;
        public float progress;
    }
    public static class ProgressBarRuning
    {
        public static void RunBuilder(string message, Func<ProgressBarData, IEnumerator> SomeTask)
        {
            if (EditorUtility.DisplayDialog("提示", message + "?", "确定", "取消"))
            {
                var data = new ProgressBarData();
                data.message = message;
                var handle = new CancelHandle();
                GlobalCoroutine.Start(StartProgressBar(data, handle));
                GlobalCoroutine.Start(SomeTask(data), handle);
            }
        }
        static IEnumerator StartProgressBar(ProgressBarData data, CancelHandle handle)
        {
            bool isCancel = false;
            while (data.progress < 1)
            {
                isCancel = EditorUtility.DisplayCancelableProgressBar("提示", data.info, data.progress);
                if (isCancel)
                {
                    handle.CancelAll();
                    EditorUtility.ClearProgressBar();
                    yield break;
                }
                yield return null;
            }
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", data.message+"(成功!)", "好的");
        }
    }
}
