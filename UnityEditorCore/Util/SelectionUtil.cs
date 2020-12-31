using System;

namespace UnityEditor
{
    public static class SelectionUtil
    {
        /// <summary>
        /// 由路径选中一个文件
        /// </summary>
        public static UnityEngine.Object SelectByPath(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            Selection.activeObject = obj;
            return obj;
        }
        /// <summary>
        /// 当Hierarchy面板选中改变时回调一次
        /// </summary>
        public static void ChangedOnce(Action callBackOnce)
        {
            if (callBackOnce == null) return;
            System.Action a = null;
            a = () =>
            {
                callBackOnce();
                Selection.selectionChanged -= a;
            };
            Selection.selectionChanged += a;
        }
    }
}
