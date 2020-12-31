using UnityCore;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVL
{
    /// <summary>
    /// 接点击系统点击事件回调关联数据结构的方法
    /// </summary>
    public class LinkMethodByClick : LinkChild, IPointerClickHandler
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

        [Button("测试回调"), Click("OnTest")]
        public string _1;
        public void OnTest()
        {
            OnPointerClick(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ParentNode.DataContent != null) AssemblyUtil.InvokeMethod(ParentNode.DataContent, Name);
        }
    }
}

