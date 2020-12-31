using System.Linq;
using UnityEngine.SceneManagement;

namespace UnityEngine
{
    public static class GameObjectUtil
    {
        /// <summary>
        /// 克隆一个物体
        /// </summary>
        public static GameObject Clone(this GameObject obj,string newName=null,Transform newParent=null)
        {

            var  clone= GameObject.Instantiate(obj, newParent==null?obj.transform.parent: newParent);
            if (newName == null) newName = obj.name;
            clone.name = newName;
            return clone;
        }

        /// <summary>
        /// 从父对象中获取组件
        /// </summary>
        public static Component GetComponentInParent(this GameObject mb, string type)
        {
            return mb.GetComponentInParent(AssemblyUtil.GetType(type));
        }

        /// <summary>
        /// 从子对象中获取组件
        /// </summary>
        public static Component GetComponentInChildren(this GameObject mb, string type)
        {
            return mb.GetComponentInChildren(AssemblyUtil.GetType(type));
        }

        /// <summary>
        /// 查找隐藏物体
        /// </summary>
        /// <param name="name">物体名称</param>
        /// <returns></returns>
        public static GameObject Find(string name)
        {
            //PreorderTraversal
            GameObject tGo1 = GameObject.Find(name);
            if (tGo1 != null)
            {
                return tGo1;
            }
            else
            {
                var objs = SceneManager.GetActiveScene().GetRootGameObjects();
                System.Predicate<Transform> callBack = x => x.name == name;
                foreach (var it in objs)
                {
                    var v = it.transform.FindChildDeep(callBack);
                    if (v != null) return v.gameObject;
                }
                return null;
            }
        }
    }
}
