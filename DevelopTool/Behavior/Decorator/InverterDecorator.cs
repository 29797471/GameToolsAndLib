using System.Collections;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/装饰节点/取反")]
    [Editor("取反")]
    public class InverterDecorator : CqBehaviorNode
    {
        protected override IEnumerator OnExecute()
        {
            yield return null;
            Result = !Result;
        }
    }
}
    