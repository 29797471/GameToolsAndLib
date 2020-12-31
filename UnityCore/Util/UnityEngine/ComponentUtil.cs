using System.Collections.Generic;

namespace UnityEngine
{
    public static class ComponentUtil
    {
        /// <summary>
        /// 从父对象中获取组件
        /// </summary>
        public static Component GetComponentInParent(this Component mb, string type)
        {
            return mb.GetComponentInParent(AssemblyUtil.GetType(type));
        }
        /// <summary>
        /// 从子对象中获取组件
        /// </summary>
        public static Component GetComponentInChildren(this Component mb, string type)
        {
            return mb.GetComponentInChildren(AssemblyUtil.GetType(type));
        }

        /// <summary>
        /// 仅在下一层查找组件
        /// </summary>
        public static List<T> GetComponentsInChildrenNoDeep<T>(this Component com, bool includeInactive=false)
        {
            var list = new List<T>();
            for (var i = 0; i < com.transform.childCount; i++)
            {
                var child = com.transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf) continue;
                var t=child.GetComponent<T>();
                if (t != null) list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 获取脚本在Hierarchy中的路径
        /// </summary>
        public static string PathInHierarchy(this Component mb,Transform root=null)
        {
            string log = mb.ToString();
            Transform tran = mb.transform.parent;
            while (tran != root)
            {
                log = string.Format("{0} - {1}", tran.name, log);
                tran = tran.parent;
            }
            return log;
        }
    }
}
