using CqCore;
using System;
using System.Collections;
namespace CqBehavior.Task
{

    public class CqBehaviorNode : CqBehaviorView
    {
        /// <summary>
        /// 外部调用执行,返回终止调用
        /// </summary>
        public IEnumerator Execute(ICancelHandle handle)
        {
            State = NodeState.Runing;
            Action act= () => State = NodeState.None;
            handle.CancelAct += act;
            if(!Node.IsLeaf())
            {
                yield return ExecuteChild(handle);
            }
            OnDone();
            yield return OnExecute();
            handle.CancelAct -= act;
            State = Result ? NodeState.Success : NodeState.Fail;
        }

        protected virtual IEnumerator ExecuteChild(ICancelHandle handle)
        {
            Result = false;
            var children = Node.Children;
            foreach (var it in children)
            {
                var node = it.nodeObj as CqBehaviorNode;
                yield return node.Execute(handle);
                if (!node.Result)
                {
                    yield break;
                }
            }
            Result = true;
        }


        /// <summary>
        /// 子类重写在执行时回掉
        /// </summary>
        protected virtual IEnumerator OnExecute()
        {
            Result = true;
            yield break;
        }
        protected virtual void OnDone()
        { 
        }

    }
    

}
