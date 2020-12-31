using System;
using UnityCore;
using UnityEngine.Events;

namespace MVL
{
    public class LinkCallBack:LinkChild
    {
        [TextBox("注入的对象方法"), ToolTip("给绑定对象生成一个方法,调用时回调callBack所添加的脚本函数")]
        public string methodName;

        public UnityEvent callBack;

        [Button("测试回调"), Click("DoneEvent")]
        public string _1;
        public void DoneEvent()
        {
            callBack.Invoke();
        }

        protected override void UpdateProperty()
        {
            AssemblyUtil.SetCallBack(ParentNode.DataContent, methodName, (Action)DoneEvent);
        }
    }
}
