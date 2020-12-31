using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [ToolTip("每个子节点都要在开始被执行,\n当其中一个返回true时终止其它正在执行的节点并返回true,\n当所有都返回false时，返回false")]
    [MenuItemPath("添加/组合节点/并行选择")]
    [Editor("并行选择")]
    public class CqParallelSelector : CqCompositeNode
    {
        protected override IEnumerator ExecuteChild(ICancelHandle handle)
        {
            var _handle = new CancelHandle();
            handle.CancelAct += _handle.CancelAll;

            int count = Node.Children.Count;
            foreach (var it in Node.Children)
            {
                var node = it.nodeObj as CqBehaviorNode;

                GlobalCoroutine.Start(node.Execute(_handle), _handle, () =>
                {
                    if (node.Result)
                    {
                        Result = true;
                        _handle.CancelAll();
                        _handle = null;
                    }
                    else
                    {
                        count--;
                        if (count == 0)
                        {
                            Result = false;
                            _handle = null;
                        }
                    }
                });
            }
            while (_handle != null)
            {
                yield return null;
            }
        }
    }

}
