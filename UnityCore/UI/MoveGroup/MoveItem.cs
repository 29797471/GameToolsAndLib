using CqCore;
using System.Collections;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 处理UI上按钮,开启时缓动排列位置
    /// </summary>
    public class MoveItem : MonoBehaviourExtended
    {
        [TextBox("排序优先级")]
        public int priority;
        public MoveGroup group;

        [CheckBox("在隐藏时从排列中移除")]
        public bool OnDisableDoRemove = true;

        [CheckBox("在显示时添加到排列中")]
        public bool OnEnableDoAdd = true;

        /// <summary>
        /// 从列表中移除,重新排列列表
        /// </summary>
        public void Remove(bool tween = false)
        {
            if (group == null) return;
            group.Remove(this, tween);
        }
        /// <summary>
        /// 添加到列表中,重新排列列表
        /// </summary>
        public void Add(bool tween = false)
        {
            if (group == null) return;
            group.Add(this, tween);
        }
        void OnDisable()
        {
            if (group == null) return;
            //当是父容器的隐藏,而本身并没有关闭
            if (gameObject.activeSelf)
            {
                StartCoroutine(CheckRemove());
                return;
            }
            if (OnDisableDoRemove) Remove(true);
        }
        void OnEnable()
        {
            if (group == null) return;
            if(OnEnableDoAdd) Add(true);
        }

        /// <summary>
        ///  父容器关闭,本身开着时,一直检查
        ///  当本身关闭后,直接remove,然后退出
        ///  当父容器打开,直接退出
        /// </summary>
        IEnumerator CheckRemove()
        {
            while(!gameObject.activeInHierarchy)
            {
                if (!gameObject.activeSelf)
                {
                    if (OnDisableDoRemove) Remove(false);
                    yield break;
                }
                yield return null;
            }
        }
    }
}
