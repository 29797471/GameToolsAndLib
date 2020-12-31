using CqCore;
using UnityCore;
using UnityEngine;

namespace MVL
{/// <summary>
 /// 绑定缓动的属性类型
 /// </summary>
    public enum BindingTweenType
    {

        /// <summary>
        /// 浮点数(float)
        /// </summary>
        [EnumLabel("浮点数(float)", typeof(float), typeof(CqTweenLerp_float))]
        System_Single,

        /// <summary>
        /// 二维向量(Vector2)
        /// </summary>
        [EnumLabel("二维向量(Vector2)", typeof(Vector2), typeof(CqTweenLerp_Vector2))]
        UnityEngine_Vector2,

        /// <summary>
        /// 三维向量(Vector3)
        /// </summary>
        [EnumLabel("三维向量(Vector3)", typeof(Vector3), typeof(CqTweenLerp_Vector3))]
        UnityEngine_Vector3,

        /// <summary>
        /// 四维向量(Vector4)
        /// </summary>
        [EnumLabel("四维向量(Vector4)", typeof(Vector4), typeof(CqTweenLerp_Vector4))]
        UnityEngine_Vector4,

        /// <summary>
        /// 三维向量(Vector3)
        /// </summary>
        [EnumLabel("四元素(Quaternion)", typeof(Quaternion), typeof(CqTweenLerp_Quaternion))]
        UnityEngine_Quaternion,


        /// <summary>
        /// 颜色(Color)
        /// </summary>
        [EnumLabel("颜色(Color)", typeof(Color), typeof(CqTweenLerp_Color))]
        UnityEngine_Color,
    }
    /// <summary>
    /// 对象的属性变化时驱动对应的绑定属性缓动到同样的属性值<para/>
    /// 暂不支持在运行中改变绑定的属性
    /// </summary>
    public class LinkMemberTween : LinkChild
    {
        

        [TextBox("属性")]
        public string Name;
        public override string LocalPath
        {
            get
            {
                return "." + Name;
            }
        }
        protected object Data
        {
            get
            {
                if (ParentNode.DataContent == null) return null;
                return AssemblyUtil.GetMemberValue(ParentNode.DataContent, Name);
            }
        }

        [ComBox("绑定类型", ComBoxStyle.RadioBox)]
        public BindingTweenType bindingType;

        void Awake()
        {
            var attr =EnumUtil.GetEnumAttr<EnumLabelAttribute>(bindingType);
            fieldType = attr.types[0];
            tweenHandleType = attr.types[1];
            DestroyHandle.CancelAct += () =>
            {
                if (tweenHandle != null) tweenHandle.Cancel();
            };
        }
        System.Type fieldType;
        System.Type tweenHandleType;

        public System.Type GetBindType
        {
            get
            {
                return EnumUtil.GetEnumAttr<EnumLabelAttribute>(bindingType).types[0];
            }
        }

        [ComponentProperty("属性→组件属性"), ComponentFPType("GetBindType")]
        public ComponentProperty comp;


        [ComBox("缓动函数", ComBoxStyle.RadioBox)]
        public EaseFunEnum efe;

        [ComBox("缓动样式", ComBoxStyle.RadioBox)]
        public EaseStyleEnum ese;

        [TextBox("缓动时间")]
        public float time=1f;

        CqTweenLerp tweenHandle;

        protected override void UpdateProperty()
        {
            if (Data != null)
            {
                var targetValue = ConvertUtil.ChangeType(Data, fieldType);

                if(tweenHandle==null)
                {
                    tweenHandle=(CqTweenLerp)AssemblyUtil.CreateInstance(tweenHandleType);
                    tweenHandle.Evaluate = UnityEngine.EaseFun.GetEase(efe, ese);
                    tweenHandle.memberProxy = comp.MemProxy;
                }
                else
                {
                    tweenHandle.Cancel();
                }
                tweenHandle.StartValue = comp.Value;
                tweenHandle.EndValue = targetValue;
                tweenHandle.Play(time);
            }
        }

        protected override void OnPropertyChanged(string PropertyName)
        {
            if (PropertyName == Name)
            {
                UpdateProperty();
            }
        }
        
    }
}

