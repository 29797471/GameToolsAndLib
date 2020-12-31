namespace UnityEngine
{
    /// <summary>
    /// 一个顶点,包含是否是曲线的起始端点
    /// </summary>
    public class CurveLinePoint
    {
        public CurveLinePoint(float x, float y, bool isCurveStartPoint = false)
        {
            pos = new Vector2(x, y);
            this.isCurveStartPoint = isCurveStartPoint;
        }
        /// <summary>
        /// 曲线或者折线上的拐点
        /// </summary>
        public Vector2 pos;

        /// <summary>
        /// 是曲线的起始点
        /// </summary>
        public bool isCurveStartPoint;

        public CurveLinePoint next;
        public CurveLinePoint prev;

        public void MakeVerts(Vector3[] vertices, int index, float lineWidth)
        {
            if (isCurveStartPoint)
            {
                //曲线线生成顶点
                //将曲线上的点拆分成上下两个垂直于切线方向的直线延宽度得到的顶点
                var dir = next.pos - prev.pos;
                dir.Normalize();
                vertices[index] = pos + new Vector2(-dir.y, dir.x) * lineWidth / 2;
            }
            else
            {
                //if (index >= 2)
                //{
                //    //直线生成顶点
                //    var leftUp = new Ray2D(vertices[index - 2], pos - prev.pos);

                //    var right = new Ray2D(next.pos, next.pos - pos);
                //    var dir_right = new Vector2(right.direction.y, -right.direction.x);
                //    dir_right.Normalize();
                //    var rightUp = new Ray2D(right.origin + dir_right * -lineWidth / 2, right.direction);

                //    vertices[index] = (Vector2)leftUp.IntersectPoint(rightUp);
                //    //var t = Vector2.Dot((Vector2)vertices[index] - leftUp.origin, leftUp.direction);
                //    //if (t < 0)
                //    //{
                //    //    //反向三角形
                //    //}

                //}
                //else
                {
                    //直线生成顶点
                    var left = new Ray2D(pos, pos - prev.pos);
                    var dir_left = new Vector2(left.direction.y, -left.direction.x);
                    dir_left.Normalize();
                    var leftUp = new Ray2D(left.origin + dir_left * -lineWidth / 2, left.direction);
                    var leftDown = new Ray2D(left.origin + dir_left * lineWidth / 2, left.direction);

                    var right = new Ray2D(next.pos, next.pos - pos);
                    var dir_right = new Vector2(right.direction.y, -right.direction.x);
                    dir_right.Normalize();
                    var rightUp = new Ray2D(right.origin + dir_right * -lineWidth / 2, right.direction);
                    var rightDown = new Ray2D(right.origin + dir_right * lineWidth / 2, right.direction);

                    var a = leftUp.TryIntersect(rightUp);
                    if (a == null)
                    {
                        var dir = next.pos - prev.pos;
                        dir.Normalize();
                        vertices[index] = pos + new Vector2(-dir.y, dir.x) * lineWidth / 2;
                    }
                    else
                    {
                        vertices[index] = (Vector2)a;
                    }
                }
            }
            vertices[index + 1] = (Vector3)(2 * pos) - vertices[index];
        }
    }
}
