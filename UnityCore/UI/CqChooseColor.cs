using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 选中时改变颜色
    /// </summary>
    public class CqChooseColor:MonoBehaviourExtended
    {
        [ComponentProperty("属性"), ComponentFPType(typeof(Color))]
        public ComponentProperty toData;
        public Color Ture;
        public Color False;

        public bool SetA
        {
            set
            {
                toData.Value = value? Ture : False;
            }
        }
    }
}
