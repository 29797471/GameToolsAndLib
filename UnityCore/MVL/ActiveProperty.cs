namespace UnityEngine
{
    /// <summary>
    /// 提供操作GameObject的显示/隐藏
    /// </summary>
    public class ActiveProperty:MonoBehaviour
    {
        public bool Active
        {
            set
            {
                gameObject.SetActive(value);
            }
            get
            {
                return gameObject.activeSelf;
            }
        }
    }
}
