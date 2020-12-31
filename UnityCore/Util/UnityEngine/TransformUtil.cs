using System.Collections.Generic;

namespace UnityEngine
{
    public static class TransformUtil
    {
        /// <summary>
        /// UGUI的方式重置默认适配参数
        /// </summary>
        public static void ReSet(this RectTransform rt)
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
        }
        public static void SetWorldMatrix(this Transform tran, Matrix4x4 matrix)
        {
            tran.position = matrix.GetPosition();
            tran.rotation = matrix.rotation;
            tran.localScale = matrix.lossyScale;
        }

        /// <summary>
        /// 从父级开始向上查找组件
        /// GetComponentInParent不能找到被隐藏的父对象
        /// </summary>
        public static T FindComponentInParent<T>(this Transform tran,bool startBySelf=false) where T : Component
        {
            Transform tempParent = tran.transform;
            if (!startBySelf)
            {
                tempParent = tempParent.parent;
                if (tempParent == null) return null;
            }
            T t;
            while ((t=tempParent.GetComponent<T>()) == null)
            {
                tempParent = tempParent.parent;
                if (tempParent == null) return null;
            }
            return t;
        }
        /// <summary>
        /// 在子中查找组件(包含隐藏对象)
        /// </summary>
        public static List<T> FindComponentsInChildren<T>(this Transform tran) where T : Component
        {
            var list = new List<T>();
            tran.GetComponentsInChildren<T>(true,list);
            var elist = new List<T>();
            tran.GetComponents<T>(elist);
            foreach(var it in elist)
            {
                list.Remove(it);
            }
            return list;
        }

        /// <summary>
        /// 删除所有子对象
        /// </summary>
        public static void RemoveAllChildren(this Transform tran,bool immediate=true)
        {
            if(immediate)
            {
                for (int i = tran.childCount - 1; i >= 0; i--)
                {
                    Object.DestroyImmediate(tran.GetChild(i).gameObject);
                }
            }
            else
            {
                for (int i = tran.childCount - 1; i >= 0; i--)
                {
                    Object.Destroy(tran.GetChild(i).gameObject);
                }
            }
            
        }
        /// <summary>
        /// 先序遍历子节点(含分支和叶子)
        /// </summary>
        public static void PreorderTraversal(this Transform tran,System.Action<Transform> OnTraversal)
        {
            if (tran.childCount>0)
            {
                for(int i=0;i<tran.childCount;i++)
                {
                    var child = tran.GetChild(i);
                    OnTraversal(child);
                    child.PreorderTraversal(OnTraversal);
                }
            }
        }

        /// <summary>
        /// 先序遍历查找(含分支和叶子)
        /// </summary>
        public static Transform FindChildDeep(this Transform tran, System.Predicate<Transform> match)
        {
            if (tran.childCount > 0)
            {
                for (int i = 0; i < tran.childCount ;i++)
                {
                    var child = tran.GetChild(i);
                    var bl= match(child);
                    if (bl) return child;
                    var result= child.FindChildDeep(match);
                    if (result != null) return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 遍历子节点,查找所有匹配列表
        /// </summary>
        public static List<Transform> FindAll(this Transform tran, System.Predicate<Transform> match)
        {
            var list = new List<Transform>();
            var childCount = tran.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = tran.GetChild(i);
                if (match(child)) list.Add(child);
            }
            return list;
        }

    }
}
