using CqCore;

namespace UnityCore
{
    /// <summary>
    /// 缓动方式
    /// </summary>
    public enum TweenMode
    {
        /// <summary>
        /// 起始→终止
        /// </summary>
        [EnumLabel("起始→终止")]
        StartToEnd,
        /// <summary>
        /// 终止→起始
        /// </summary>
        [EnumLabel("终止→起始")]
        EndToStart,
        /// <summary>
        /// 当前→起始
        /// </summary>
        [EnumLabel("当前→起始")]
        ToStart,
        /// <summary>
        /// 当前→终止
        /// </summary>
        [EnumLabel("当前→终止")]
        ToEnd,
    }
    public enum TweenMemberMode
    {
        /// <summary>
        /// 起始→终止
        /// </summary>
        [EnumLabel("起始→终止")]
        StartToEnd = 0,
        /// <summary>
        /// 终止→起始
        /// </summary>
        [EnumLabel("终止→起始")]
        EndToStart = 3,
        /// <summary>
        /// 当前→起始
        /// </summary>
        [EnumLabel("当前→起始")]
        ToStart = 2,

        /// <summary>
        /// 当前→终止
        /// </summary>
        [EnumLabel("当前→终止")]
        ToEnd = 1,
    }
}
