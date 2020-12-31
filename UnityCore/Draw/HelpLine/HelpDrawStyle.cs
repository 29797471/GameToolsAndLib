namespace UnityCore
{
    public enum HelpDrawStyle
    {
        None,
        /// <summary>
        /// 非运行时不可执行
        /// 不可在移动设备上显示
        /// 开启Gizmos后在Game视图显示
        /// </summary>
        Debug,

        /// <summary>
        /// 非运行时可执行
        /// 不可在移动设备上显示
        /// 开启Gizmos后在Game视图显示
        /// </summary>
        Gizmos,

        /// <summary>
        /// 非运行时不可执行
        /// 可在移动设备上显示
        /// 不受Gizmos影响
        /// </summary>
        Graphics,
    }
}
