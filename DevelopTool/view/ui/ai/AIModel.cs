using CqBehavior.Task;
using System;
using System.Linq;

namespace DevelopTool
{
    /// <summary>
    /// 编辑数据结构面板关联的数据模块
    /// </summary>
    [Priority(12)]
    [Editor("行为编辑器", "/DevelopTool;component/Res/Images/Icons/ai.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class AIModel : SingleModel<AIModel>
    {
        public override Setting Setting { get { return setting; } }
        
        public AISetting setting { get { return SettingModel.instance.GetSetting<AISetting>(); } }

        [Image, Width(20)]
        public string FindIcon
        {
            get
            {
                return "/WinCore;component/Res/find.ico";
            }
        }

        [Priority(0, 0, 1)]
        [TextBox(), ToolTip("检索过滤"), MinWidth(170)]
        public string Seach
        {
            get { return mSeach; }
            set { mSeach = value; Update("Seach"); Update("FilterItem"); }
        }
        string mSeach;

        public Predicate<object> FilterItem
        {
            get
            {
                if (string.IsNullOrEmpty(Seach)) return null;
                return o => o.ToString().ToLower().Contains(Seach.ToLower());
            }
        }

        [Priority(0, 1)]
        [TreeView("CqBehavior.Task"), Width(300), Height(520), SelectedValue("SelectObj"), Filter("FilterItem")]
        public TreeNode Root
        {
            get {  return mRoot; }
            set { mRoot = value;Update("Root"); }
        }
        TreeNode mRoot;

        [Priority( 1)]
        [GroupBox(), Width(820), Height(570), Margin(30, 0)]
        public BaseTreeNotifyObject SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }
        BaseTreeNotifyObject mSelectObj;
        public override void OnHide()
        {
            SelectObj = null;
        }
        protected override void ReLoad()
        {
            Root = Torsion.TryDeserialize<TreeNode>(FileOpr.ReadFile(setting.SetPath));
        }
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(Root));
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            //string[] pargs = Environment.GetCommandLineArgs();
            Root.PreorderTraversal(node =>
            {
                if(node.nodeObj is CqRoot)
                {
                    var root = (node.nodeObj as CqRoot);
                    if(root.DoCMD)
                    {
                        root.Start(null);
                    }
                }
            });
        }
    }
}

