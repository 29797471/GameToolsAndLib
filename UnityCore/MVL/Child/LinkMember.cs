using UnityCore;
using UnityEngine;
using UnityEngine.Events;

namespace MVL
{
    /// <summary>
    /// 特别地,双向关联时,界面改变驱动数据改变后,要阻止数据继续驱动界面改变
    /// </summary>
    public class LinkMember : LinkChild
    {
        /// <summary>
        /// 对象显示隐藏
        /// </summary>
        public bool GameObjectActiveSelf
        {
            get
            {
                return gameObject.activeSelf;
            }
            set
            {
                gameObject.SetActive(value);
            }
        }
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
        public BindingFPType type;

        public System.Type GetBindType
        {
            get
            {
                return AssemblyUtil.GetType(type.ToString().Replace("_", "."));
            }
        }

        /// <summary>
        /// 关联组件属性
        /// </summary>
        [ComponentProperty("关联组件属性"), ComponentFPType("GetBindType")]
        public ComponentProperty comp;

        public System.Type GetToDataType
        {
            get
            {
                return typeof(UnityEvent<>).MakeGenericType(GetBindType);
            }
        }

        /// <summary>
        /// 组件属性改变通知
        /// </summary>
        [ComponentProperty("组件属性改变通知"), ComponentFPType("GetToDataType")]
        public ComponentProperty toData;


        protected override void UpdateProperty()
        {
            if (doing) return;
            if (Data != null)
            {
                doing = true;
                comp.Value = ConvertUtil.ChangeType(Data, GetBindType);
                doing = false;
            }
        }
        /// <summary>
        /// 界面->数据或者 数据->界面的 改变的过程中
        /// </summary>
        bool doing;
        protected override void OnLink()
        {
            base.OnLink();
            if (!hasAddListen)
            {
                LinkViewToData();
                hasAddListen = true;
            }
        }
        bool hasAddListen;
        protected override void OnPropertyChanged(string PropertyName)
        {
            if (PropertyName == Name)
            {
                UpdateProperty();
            }
        }
        void LinkViewToData()
        {
            if (toData != null && toData.Value != null)
            {
                var ueb = toData.Value as UnityEventBase;

                ueb.SetCallBack(type, (v) =>
                {
                    if (doing) return;
                    if (ParentNode.DataContent != null)
                    {
                        doing = true;
                        AssemblyUtil.SetMemberValue(ParentNode.DataContent, Name, v);
                        doing = false;
                    }
                }, DestroyHandle);

            }
        }
    }
}

