using System.Collections;
using UnityCore;

namespace MVL
{
    /// <summary>
    /// 关联列表元素
    /// </summary>
    public class LinkItem : LinkParent
    {
        protected override void OnLinkParent()
        {
        }
        public override string LocalPath
        {
            get
            {
                return string.Format("[{0}]", (index == -1) ? "-" : index.ToString());
            }
        }
        [TextBox("索引"), IsEnabled(false)]
        public int index = -1;
        protected override void UpdateProperty()
        {
            var linkList = ((LinkList)ParentNode);
            if (linkList.DataContent is IList)
            {
                var list = (IList)linkList.DataContent;
                if (index >= 0 && index < list.Count)
                {
                    DataContent = list[index];
                }
            }
            else if (linkList.DataContent is ISetGetEnumerable)
            {
                var list = (ISetGetEnumerable)linkList.DataContent;
                if (index >= 0 && index < list.Count)
                {
                    DataContent = list[index];
                }
            }
        }
    }
}

