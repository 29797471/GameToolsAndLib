using CqCore;
using UnityCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditorCore
{
    public static class GameObjectEditorUtil
    {
        [MenuItem("GameObject/对象比较", false, 10)]
        public static void CustomGameObjectCompare()
        {
            FixBug.CallMenuItemOnce(() =>
            {
                if (Selection.objects.Length == 2)
                {
                    var a = (GameObject)Selection.objects[0];
                    var b = (GameObject)Selection.objects[1];
                    if (a != null && b != null)
                    {
                        var aMono = a.GetComponent<ICompareMono>();
                        var bMono = b.GetComponent<ICompareMono>();
                        if (aMono != null && bMono != null)
                        {
                            aMono.Compare(bMono);
                        }
                    }
                }
            });
        }

        [MenuItem("GameObject/显示在Hierarchy中的隐藏单位", false, 10)]
        public static void CustomGameObjectShow()
        {
            var objs = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in objs)
            {
                if (MathUtil.StateCheck(obj.hideFlags, HideFlags.HideInHierarchy))
                {
                    obj.hideFlags = MathUtil.StateDel(obj.hideFlags, HideFlags.HideInHierarchy);
                }
            }
        }
    }
}
