namespace UnityEngine
{
    public static class Matrix4x4Util
    {
        /// <summary>
        /// 获取位置
        /// </summary>
        public static Vector3 GetPosition(this Matrix4x4 matrix)
        {
            Vector3 position;
            position.x = matrix.m03;
            position.y = matrix.m13;
            position.z = matrix.m23;
            return position;
        }

        /// <summary>
        /// 设置位置<para/>
        /// 结构体需要带ref来修改内部参数
        /// </summary>
        public static void SetPosition(ref this Matrix4x4 matrix, Vector3 position)
        {
            matrix.m03 = position.x;
            matrix.m13 = position.y;
            matrix.m23 = position.z;
        }

        /// <summary>
        /// 射线矩阵变换
        /// </summary>
        public static Ray MultiplyRay(this Matrix4x4 mat,Ray ray)
        {
            return new Ray(mat.MultiplyPoint(ray.origin), mat.MultiplyVector(ray.direction));
        }
    }
}
