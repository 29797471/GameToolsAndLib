using CqCore;
using System.Collections;

namespace CqBehavior.Task
{

    [ToolTip("选择器(Selector),\n它会依次执行每个子行为直到其中一个成功执行或者全部失败为止.")]
    [MenuItemPath("添加/组合节点/选择")]
    [Editor("选择")]
    public class CqSelector : CqCompositeNode
    {
        protected override IEnumerator ExecuteChild(ICancelHandle handle)
        {
            Result = false;
            var children = Node.Children;
            foreach (var it in children)
            {
                var node = it.nodeObj as CqBehaviorNode;
                yield return node.Execute(handle);
                if (node.Result)
                {
                    Result = true;
                    yield break;
                }
            }
        }
    }

}
