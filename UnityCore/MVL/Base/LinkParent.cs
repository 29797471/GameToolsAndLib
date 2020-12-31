using System;
using UnityCore;
using UnityEngine;

namespace MVL
{
    /// <summary>
    /// 上级组件绑定的数据对象的改变会导致子组件重新绑定数据对象
    /// </summary>
    public abstract class LinkParent : LinkChild
    {
        public object mDataContent;

        /// <summary>
        /// 绑定的数据
        /// </summary>
        public object DataContent
        {
            get
            {
                return mDataContent;
            }
            set
            {
                if (mDataContent != value)
                {
                    mDataContent = value;

                    //下级重置关联的数据
                    var bns = transform.GetComponentsInChildren<LinkChild>(true);
                    foreach (var bn in bns)
                    {
                        if (bn.ParentNode == this)
                        {
                            bn.LinkParent();
                        }
                    }
                }
            }
        }
    }
}

