using System.Collections;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/装饰节点/返回失败")]
    [Editor("返回失败")]
    public class ReturnFailure : CqBehaviorNode
    {
        protected override IEnumerator OnExecute()
        {
            yield return null;
            Result = false;
        }
    }
}

