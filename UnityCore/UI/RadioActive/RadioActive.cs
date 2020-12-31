using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 设置显示隐藏的对象,由DoneActiveGroup控制
    /// </summary>
    public class RadioActive : MonoBehaviour
    {
        public RadioActiveGroup group;
        private void Awake()
        {
            group.Add(this);
        }
    }
}
