namespace CqBehavior.Task
{
    /// <summary>
    /// 组合节点可以有多个子节点
    /// </summary>
    public class CqCompositeNode : CqBehaviorNode
    {
        [Priority(1)]
        [Label("说明", 100), MinWidth(200)]
        public string Desc
        {
            get
            {
                var toolTip = AssemblyUtil.GetClassAttribute<ToolTipAttribute>(this);
                if (toolTip == null) return null;
                return toolTip.DataValue.ToString();
            }
        }
    }

}
