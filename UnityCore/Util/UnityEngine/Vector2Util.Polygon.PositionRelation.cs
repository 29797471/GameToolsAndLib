using CqCore;

namespace UnityEngine
{
    /// <summary>
    /// 多边形与圆位置关系
    /// </summary>
    public enum PolyCircleRelations
    {
        /// <summary>
        /// 包含
        /// </summary>
        [EnumLabel("包含")]
        Contains,
        /// <summary>
        /// 内切
        /// </summary>
        [EnumLabel("内切")]
        Inscribe,
        /// <summary>
        /// 相交
        /// </summary>
        [EnumLabel("相交")]
        Intersection,
        /// <summary>
        /// 外切
        /// </summary>
        [EnumLabel("外切")]
        Circumscribe,
        /// <summary>
        /// 相离
        /// </summary>
        [EnumLabel("相离")]
        Separation,
    }
    /// <summary>
    /// 多边形与点位置关系
    /// </summary>
    public enum PolyPointRelations
    {
        /// <summary>
        /// 里面
        /// </summary>
        [EnumLabel("里面")]
        Inside,
        /// <summary>
        /// 边上或者与顶点重合
        /// </summary>
        [EnumLabel("边上或者与顶点重合")]
        Contain,

        /// <summary>
        /// 外面
        /// </summary>
        [EnumLabel("外面")]
        Outside,
    }
    /// <summary>
    /// 多边形(任意凹凸)位置关系<para/>
    /// 外贴,外离时两个多边形没有重叠的面积(交集)<para/>
    /// 内贴,包含时两个多边形重叠的面积(交集)等于其中一个包含在内的多边形的面积<para/>
    /// </summary>
    public enum PolyRelations
    {
        /// <summary>
        /// 无<para/>
        /// 非正常状态(不是两个正确的多边形)
        /// </summary>
        [EnumLabel("无")]
        None,

        /// <summary>
        /// 包含<para/>
        /// 一个多边形所有顶点都在另一个多边形内,并且所有边没有交点
        /// </summary>
        [EnumLabel("包含")]
        Contains,

        /// <summary>
        /// 内贴<para/>
        /// 一个多边形的所有顶点和它与另一个多边形的边的所有的交点都在另一个多边形内或者边上.
        /// </summary>
        [EnumLabel("内贴")]
        Inscribe,

        /// <summary>
        /// 相交<para/>
        /// 两个多边形有任何一条边相交视为多边形相交.
        /// </summary>
        [EnumLabel("相交")]
        Intersection,

        /// <summary>
        /// 外贴<para/>
        /// 两个多边形没有交集(重叠的面积),存在一个多边形的顶点在另一个多边形边上,或者一条边和另一个多边形的边重叠.
        /// </summary>
        [EnumLabel("外贴")]
        Circumscribe,

        /// <summary>
        /// 相离<para/>
        /// 一个多边形所有顶点都在另一个多边形外,并且所有边没有交点(两个多边形没有交集)<para/>
        /// </summary>
        [EnumLabel("相离")]
        Separation,
    }
}