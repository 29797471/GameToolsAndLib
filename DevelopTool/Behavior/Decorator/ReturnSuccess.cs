using System.Collections;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/装饰节点/返回成功")]
    [Editor("返回成功")]
    public class ReturnSuccess : CqBehaviorNode
    {
        protected override IEnumerator OnExecute()
        {
            yield return null;
            Result = true;
        }
    }
}

