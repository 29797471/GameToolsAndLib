using System.Windows;
namespace CqBehavior.Task
{
    public enum NodeState
    {
        None,
        Runing,
        Success,
        Fail,
    }
    public class CqBehaviorView : BaseTreeNotifyObject
    {
        public void ReSet()
        {
            State = NodeState.None;
        }
        NodeState mState;
        /// <summary>
        /// 图标状态
        /// </summary>
        public NodeState State
        {
            get { return mState; }
            protected set
            {
                mState = value;
                Update("State");
                Update("Runing");
                Node.IconVisibility = Runing? Visibility.Visible: Visibility.Collapsed;
                switch (mState)
                {
                    case NodeState.Runing:
                        Node.Icon = "/WinCore;component/Res/tree/play.ico";
                        break;
                    case NodeState.Success:
                        Node.Icon = "/WinCore;component/Res/tree/correct.ico";
                        break;
                    case NodeState.Fail:
                        Node.Icon = "/WinCore;component/Res/tree/error.ico";
                        break;
                }
            }
        }

        public bool Runing
        {
            get
            {
                return mState != NodeState.None;
            }
        }


        bool mResult;
        public bool Result
        {
            get => mResult;
            protected set
            {
                mResult = value;
            }
        }

    }

    


}
