using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCore
{
    /// <summary>
    /// 由分组id控制一组对象的显示或者隐藏
    /// </summary>
    public class RadioActiveGroup : MonoBehaviourExtended
    {
        public enum EnumStyle
        {
            [EnumLabel("显示第一个")]
            ShowFrist,
            [EnumLabel("全部隐藏")]
            HideAll,
        }
        [ComBoxAttribute("起始样式", ComBoxStyle.RadioBox)]
        public EnumStyle e;

        List<RadioActive> list=new List<RadioActive>();
        public GameObject root;
        public bool Add(RadioActive ra)
        {
            if (!list.Contains(ra))
            {
                switch (e)
                {
                    case EnumStyle.ShowFrist:
                        if (list.Count == 0) ra.gameObject.SetActive(true);
                        else ra.gameObject.SetActive(false);
                        break;
                    case EnumStyle.HideAll:
                        ra.gameObject.SetActive(false);
                        break;
                }
                list.Add(ra);
                return true;
            }
            else return false;
        }
        RadioActive last;

        /// <summary>
        /// 显示√/隐藏X
        /// </summary>
        public void SetActive(RadioActive ra)
        {
            if (last != null)
            {
                if (last == ra) return;
                last.gameObject.SetActive(false);
                last = null;
            }
            ra.gameObject.SetActive(true);
            last = ra;
        }
        /// <summary>
        /// 显示√/隐藏X
        /// </summary>
        public bool SetActive(int index)
        {
            if (list.Count <= index) return false;
            var ra = list[index];
            SetActive(ra);
            return true;
        }
    }
}
