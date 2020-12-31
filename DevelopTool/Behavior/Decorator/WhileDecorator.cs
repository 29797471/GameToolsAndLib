using CqCore;
using System.Collections;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/装饰节点/循环")]
    [Editor("循环")]
    public class WhileDecorator : CqBehaviorNode
    {

        [CheckBox("成功时一直循环")]
        [Priority(2)]
        public bool SuccessExit { get { return mSuccessExit; } set { mSuccessExit = value; Update("OnSuccessExit"); } }
        public bool mSuccessExit;

        protected override IEnumerator ExecuteChild(ICancelHandle handle)
        {
            do
            {
                yield return base.ExecuteChild(handle);
            }
            while (mSuccessExit == Result);
        }
        
    }
}
    