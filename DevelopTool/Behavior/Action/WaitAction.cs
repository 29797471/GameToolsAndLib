using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/行为节点/等待/等待一段时间")]
    [Editor("等待一段时间")]
    public class WaitAction : CqBehaviorNode
    {
        [TextBox("时间(s)",100), MinWidth(100)]
        [Priority(1)]
        public float Duration { get { return mDuration; } set { mDuration = value; Update("Duration"); } }
        public float mDuration=1;

        [Priority(2)]
        [TextBox("剩余时间"), IsEnabled(false), Visibility("Runing", AttributeTarget.Parent)]
        public string LeftTime { get; set; }


        protected override IEnumerator OnExecute()
        {
            var endTick = GlobalCoroutine.GetTickTime( mDuration);
            while (GlobalCoroutine.Tick < endTick)
            {
                yield return null;
                LeftTime =string.Format("{0:N1}", GlobalCoroutine.ToSeconds(endTick));
                Update("LeftTime");
            }
            //yield return GlobalCoroutine.Sleep(mDuration);
            Result = true;
        }
    }
}
