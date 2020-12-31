namespace UnityEngine
{
    public static class EventUtil
    {
        /// <summary>
        /// 获取鼠标在平面上移动的坐标点
        /// </summary>
        public static Vector3? GetMousePointInPlane(this Event evt,Camera cam, Plane plane)
        {
            var _mousePos = evt.GetMouseScreenPosition(cam);
            var ray = cam.ScreenPointToRay(_mousePos);
            float enter;
            Vector2 mPos = Vector2.zero;
            if (plane.Raycast(ray, out enter))
            {
                return ray.GetPoint(enter);
            }
            return null;
        }

        /// <summary>
        /// 获取鼠标在屏幕坐标系下的位置
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMouseScreenPosition(this Event evt, Camera cam)
        {
            var mousePos = evt.mousePosition;
            mousePos.y = cam.pixelHeight - mousePos.y;
            return mousePos;
        }
    }
}
