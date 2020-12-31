using CqCore;
using System;
using System.Collections;

namespace CqBehavior.Task
{

    [ToolTip("每个子节点都要在开始被执行,\n" +
        "当其中一个返回false时终止其它正在执行的节点并返回false,\n" +
        "当所有都返回ture时，返回true.\n\n" +
        "监视器(Monitor)是并行器的应用之一，\n" +
        "通过在行为运行过程中不断检查是否满足某条件，\n" +
        "如果不满足则立刻退出。\n" +
        "将条件节点放在并行器的尾部即可。")]
    [MenuItemPath("添加/组合节点/并行")]
    [Editor("并行")]
    public class CqParallel : CqCompositeNode
    {
        protected override IEnumerator ExecuteChild(ICancelHandle handle)
        {
            var _handle = new CancelHandle();
            handle.CancelAct += _handle.CancelAll;

            int count = Node.Children.Count;
            foreach (var it in Node.Children)
            {
                var node = it.nodeObj as CqBehaviorNode;

                GlobalCoroutine.Start(node.Execute(_handle), _handle,()=> 
                {
                    if (!node.Result)
                    {
                        Result = false;
                        _handle.CancelAll();
                        _handle = null;
                    }
                    else
                    {
                        count--;
                        if(count==0)
                        {
                            Result = true;
                            _handle = null;
                        }
                    }
                });
            }
            while(_handle!=null)
            {
                yield return null;
            }
        }
    }
}


