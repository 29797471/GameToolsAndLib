namespace CqBehavior.Task
{

    [ToolTip("主动选择器(ActiveSelector),\n" +
        "主动选择器会不断的主动检查已经做出的决策，并不断的尝试高优先级行为的可行性，\n" +
        "当高优先级行为可行时立即打断低优先级行为的执行")]
    [MenuItemPath("添加/组合节点/主动选择")]
    [Editor("主动选择")]
    public class CqActiveSelector : CqCompositeNode
    {
        
    }
}
