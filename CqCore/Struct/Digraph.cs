using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 有向图
    /// </summary>
    public class Digraph
    {
        public List<MapPoint> points;

        public List<Way> paths;
        
        /// <summary>
        /// 找寻一条最短路径
        /// </summary>
        public List<Way> FindMinWeightWay(MapPoint start,MapPoint dest)
        {
            var ways = new List<Way>();
            return ways;
        }
    }
    public class MapPoint
    {
        public float x, y;
    }
    public class Way
    {
        public MapPoint start;
        public MapPoint end;
        public float inX, inY, outX, outY;
        /// <summary>
        /// 权重
        /// </summary>
        public float weight;
    }


}
