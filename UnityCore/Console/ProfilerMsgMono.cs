using CqCore;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 包含内存数据和操作的组件
    /// </summary>
    public class ProfilerMsgMono:MonoBehaviourExtended, ICompareMono
    {
        TreeNode<MemoryDataNode> head
        {
            get
            {
                if (msg == null) msg = Torsion.Deserialize<ProfilerMsg>(data);
                return msg.head;
            }
        }
        //[Button("生成内存快照"),Click("MakeMsgTree")]
        //public string _1;
        [HideInInspector]
        public long time;

        public ProfilerMsg msg { set ; private get; }

        [HideInInspector]
        public string data;

        
        public void MakeMsgTree()
        {
            Debug.Log(Torsion.Serialize(msg,true,false,false,1));
            transform.RemoveAllChildren();
            var obj = head.Data.MakeGameObject(ConsoleConfig.Inst.hierarchyNameTblCount,head.Height());
            obj.transform.SetParent(transform);
        }

        public void Compare(ICompareMono x)
        {
            var other = (ProfilerMsgMono)x;
            if (other == null) return;
            //保证之后和之前对比
            if(time<other.time)
            {
                other.Compare(this);
                return;
            }
            var delta = head.Data - other.head.Data;
            var deltaObj = new GameObject(string.Format("{0} - {1}", name, other.name));
            if(delta!=null)
            {
                var obj = delta.MakeGameObject(ConsoleConfig.Inst.hierarchyNameTblCount, head.Height(), true);
                obj.transform.SetParent(deltaObj.transform);
            }
        }
    }
}
