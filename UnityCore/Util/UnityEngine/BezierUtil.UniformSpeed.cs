namespace UnityEngine
{
    /// <summary>
    /// 匀速贝塞尔
    /// </summary>
    public static partial class BezierUtil
    {
        /// <summary>
        /// 2次贝塞尔曲线长度
        /// </summary>
        public static float PartLength(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            float ax = p0.x - 2 * p1.x + p2.x;

            float ay = p0.y - 2 * p1.y + p2.y;

            float bx = 2 * p1.x - 2 * p0.x;

            float by = 2 * p1.y - 2 * p0.y;



            float A = 4 * (ax * ax + ay * ay);

            float B = 4 * (ax * bx + ay * by);

            float C = bx * bx + by * by;

            float temp1 = Mathf.Sqrt(C + t * (B + A * t));

            float temp2 = (2 * A * t * temp1 + B * (temp1 - Mathf.Sqrt(C)));

            float temp3 = Mathf.Log(B + 2 * Mathf.Sqrt(A) * Mathf.Sqrt(C));

            float temp4 = Mathf.Log(B + 2 * A * t + 2 * Mathf.Sqrt(A) * temp1);

            float temp5 = 2 * Mathf.Sqrt(A) * temp2;

            float temp6 = (B * B - 4 * A * C) * (temp3 - temp4);

            return (temp5 + temp6) / (8 * Mathf.Pow(A, 1.5f));
        }
        
    }
}
