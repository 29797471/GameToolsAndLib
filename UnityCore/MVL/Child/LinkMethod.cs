using UnityCore;
using UnityEngine;
using UnityEngine.UI;
namespace MVL
{
    /// <summary>
    /// 接按钮点击回调关联数据结构中的方法
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class LinkMethod : LinkChild
    {
        [TextBox("方法"),ToolTip("当按钮/开关点击时回调对应绑定的方法")]
        public string Name;
        public override string LocalPath
        {
            get
            {
                return "." + Name + "()";
            }
        }
        protected override void OnLink()
        {
            var btn = GetComponent<Button>();
            if (btn)
            {
                btn.onClick.SetCallBack(()=> OnButtonClick(), ClearBinding);
                return;
            }
            Debug.LogError(string.Format("在{0}中没有Button", transform.PathInHierarchy()));
            /*
            var toggle = GetComponent<Toggle>();
            if(toggle)
            {
                toggle.onValueChanged.AddListener(OnToggleClick);
                ClearBinding += () => { toggle.onValueChanged.RemoveListener(OnToggleClick); };
                return;
            }
            Debug.LogError(string.Format("在{0}中没有Toggle", transform.PathInHierarchy()));
            */

        }

        [Button("测试回调"), Click("OnTest")]
        public string _1;
        public void OnTest()
        {
            var btn = GetComponent<Button>();
            if (btn)
            {
                OnButtonClick();
                return;
            }
            /*
            var toggle = GetComponent<Toggle>();
            if (toggle)
            {
                OnToggleClick(toggle.isOn);
                return;
            }
            */
        }
        void OnButtonClick()
        {
            if (ParentNode.DataContent != null) AssemblyUtil.InvokeMethod(ParentNode.DataContent, Name);
        }
        /*
        void OnToggleClick(bool bl)
        {
            if (ParentNode.DataContent != null) AssemblyUtil.InvokeMethod(ParentNode.DataContent, Name,bl);
        }
        */
        }
    }

