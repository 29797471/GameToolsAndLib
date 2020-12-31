namespace CqCore
{
    /// <summary>
    /// 网络连接状态
    /// </summary>
    public enum NetState
    {
        [EnumLabel("未连接")]
        None,

        [EnumLabel("断开连接")]
        Notconnect,

        [EnumLabel("正在连接")]
        Connecting,

        [EnumLabel("已连接")]
        Connected,

        [EnumLabel("连接失败")]
        ConnectFailed,
    }
}
