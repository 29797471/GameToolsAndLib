using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 反射还原对应脚本
    /// </summary>
    public class MonoReflection : MonoBehaviour
    {
        [TextBox("脚本")]
        public string scriptName;
        [TextBox("数据内容", true),MinHeight(50)]
        public string scriptData;
    }
}
