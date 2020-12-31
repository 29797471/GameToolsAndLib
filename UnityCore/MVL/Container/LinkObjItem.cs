//using System.Collections;
//namespace MVL
//{
//    /// <summary>
//    /// 关联列表元素
//    /// </summary>
//    public class LinkItem : LinkParent
//    {
//        public override string LocalPath
//        {
//            get
//            {
//                return string.Format("[{0}]", (Index == -1) ? "-" : Index.ToString());
//            }
//        }

//        protected override void OnLinkParent()
//        {
//        }
//        public int Index
//        {
//            get
//            {
//                var linkList = ((LinkList)ParentNode);
//                if (linkList.CloneList != null)
//                {
//                    return linkList.CloneList.IndexOf(this);
//                }
//                return -1;
//            }
//        }
//        protected override void UpdateProperty()
//        {
//            var linkList = ((LinkList)ParentNode);
//            if (linkList.DataContent is IList)
//            {
//                var list = (IList)linkList.DataContent;
//                if (linkList.CloneList != null)
//                {
//                    var index = Index;
//                    if (index >= 0 && index < linkList.CloneList.Count)
//                    {
//                        DataContent = list[index];
//                    }
//                }
//            }
//            else if(linkList.DataContent is ISetGetEnumerable)
//            {
//                var list = (ISetGetEnumerable)linkList.DataContent;
//                if (linkList.CloneList != null)
//                {
//                    var index = Index;
//                    if (index >= 0 && index < linkList.CloneList.Count)
//                    {
//                        DataContent = list[index];
//                    }
//                }
//            }

//        }
//    }
//}

