namespace CqCore
{
    public enum LogType
    {
        /// <summary>
        /// 错误
        /// </summary>
        [EnumLabel("错误")]
        [EnumColor("#FF0000")]
        Error = 0,

        /// <summary>
        /// 断言
        /// </summary>
        [EnumLabel("断言")]
        [EnumColor("#FF00FF")]
        Assert = 1,

        /// <summary>
        /// 警告
        /// </summary>
        [EnumLabel("警告")]
        [EnumColor("#1E90FF")]
        Warning = 2,

        /// <summary>
        /// 日志
        /// </summary>
        [EnumLabel("日志")]
        [EnumColor("#000000")]
        Log = 3,

        /// <summary>
        /// 异常
        /// </summary>
        [EnumLabel("异常")]
        [EnumColor("#800080")]
        Exception = 4,
    }
}
