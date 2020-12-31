using CqCore;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/装饰节点/根节点")]
    [Editor("根节点")]
    public class CqRoot: CqBehaviorNode
    {
        [Priority(2, 1)]
        [IsEnabled("State", "x!=1"),Margin(30,0,0,0)]
        [Button,Click("Start")]
        public string Btn1 { get { return "执行"; } }

        CancelHandle cancelHandle=new CancelHandle();

        public void Start(object obj)
        {
            Stop(null);
#if CMD
            GlobalCoroutine.BlockingCall(Execute(cancelHandle));
#else
            GlobalCoroutine.Start(Execute(cancelHandle), cancelHandle);
#endif

        }

        [Priority(2, 2)]
        [IsEnabled("State","x=1"), Margin(30, 0, 0, 0)]
        [Button, Click("Stop")]
        public string Btn2 { get { return "终止"; } }
        public void Stop(object obj)
        {
            Node.PreorderTraversal(x => (x.nodeObj as CqBehaviorView).ReSet());
            cancelHandle.CancelAll();
        }

        [Priority(1)]
        [CheckBox("在控制台中执行", 140), MinWidth(100)]
        public bool DoCMD
        {
            get { return mDoCMD; }
            set { mDoCMD = value; Update("DoCMD"); }
        }
        public bool mDoCMD;
    }

}
