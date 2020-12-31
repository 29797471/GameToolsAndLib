using CqCore;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 处理UI上一组按钮,
    /// 这些按钮有固定的先后顺序,需要在显示隐藏或者不在列表中时动态排列按钮
    /// 排列缓动(动态添加,删除)
    /// </summary>
    public class MoveGroup : MonoBehaviour
    {
        List<Vector3> poss;

        /// <summary>
        /// 每索引对应位置
        /// </summary>
        public List<Vector3> Poss { get { return poss; } }

        List<MoveItem> list;

        [TextBox("移动时间")]
        public float time;

        void Start()
        {
            poss = new List<Vector3>();
            list = new List<MoveItem>();
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var mi = child.gameObject.AddComponent<MoveItem>();
                mi.priority = i;
                poss.Add(child.localPosition);
                mi.group = this;

                if(child.gameObject.activeSelf)
                {
                    list.Add(mi);
                }
            }
            SetAll();
        }
        public bool Add(MoveItem mi,bool tween=false)
        {
            if(list.Contains(mi))
            {
                return false;
            }
            list.Add(mi);
            list.Sort(x => x.priority);
            if (tween) MoveAll();
            else SetAll();
            return true;
        }
        public bool Remove(MoveItem mi, bool tween = false)
        {
            if(!list.Contains(mi))
            {
                return false;
            }
            list.Remove(mi);
            list.Sort(x => x.priority);
            if (tween) MoveAll();
            else SetAll();
            return true;
        }
        /// <summary>
        /// 排列所有子控件
        /// </summary>
        public void SetAll()
        {
            for (var i = 0; i < list.Count; i++)
            {
                var child = list[i];
                child.transform.localPosition = poss[i];
            }
        }

        CancelHandle CancelMove = new CancelHandle();

        /// <summary>
        /// 移动所有子控件
        /// </summary>
        public void MoveAll()
        {
            if (poss == null) return;
            CancelMove.CancelAll();
            var ease = EaseFun.ConvertToOut(EaseFun.QuadraticEase);
            for (var i = 0; i < list.Count; i++)
            {
                var child = list[i].transform;
                if (time == 0)
                {
                    child.localPosition = poss[i];
                }
                else
                {
                    var handle = new CqTweenLerp_Vector3()
                    {
                        memberProxy = MemberProxy.GetMemberProxy(child, "localPosition"),
                        Evaluate = ease,
                        start = child.localPosition,
                        end = poss[i],
                    };

                    handle.Play(time, CancelMove);
                }
            }
        }
    }
}
