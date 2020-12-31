using UnityCore;
using UnityEngine;

namespace MVL
{
    public class LinkBase : MonoBehaviourExtended
    {
        [TextBox("绝对路径"), IsEnabled(false), LinkProperty("FullPath"), Visible("IsChild")]
        public string _FullPath;


        public virtual string LocalPath
        {
            get
            {
                return "";
            }
        }

        public string FullPath
        {
            get
            {
                return IsChild ? (ParentNode.FullPath + LocalPath) : "*";
            }
        }
        /// <summary>
        /// 是否是子点
        /// </summary>
        public bool IsChild
        {
            get { return ParentNode != null; }
        }

        protected LinkParent mParent;
        protected bool doOnce;
        public virtual LinkParent ParentNode
        {
            get
            {
                if (!doOnce)
                {
                    doOnce = true;
                    mParent = transform.FindComponentInParent<LinkParent>(!(this is LinkParent));
                }
                return mParent;
            }
        }
    }
}

