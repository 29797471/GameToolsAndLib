using CommonStruct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevelopTool
{
    [Priority(20)]
    [Editor("通用数据结构", "/DevelopTool;component/Res/Images/Icons/class.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class CommonStructModel : SingleModel<CommonStructModel>
    {
        public override Setting Setting { get { return setting; } }
        public CommonStructSetting setting { get { return SettingModel.instance.GetSetting<CommonStructSetting>(); } }

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

        [TreeView("CommonStruct"), Priority(0, 1), Width(300), Height(520), SelectedValue("SelectObj"), Filter("FilterItem")]
        [Export("%StartNode%", "%EndNode%")]
        public TreeNode Root
        {
            get { return mRoot; }
            set { mRoot = value; Update("Root"); }
        }
        TreeNode mRoot = new TreeNode();

        BaseTreeNotifyObject mSelectObj;

        [Priority(1)]
        [GroupBox(), MinWidth(750), Height(570), Margin(30, 0)]
        public BaseTreeNotifyObject SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }

        protected override void ReLoad()
        {
            Root = Torsion.Deserialize<TreeNode>(FileOpr.ReadFile(setting.SetPath));
        }

        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(Root));
            return true;
        }


        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            foreach (var makefile in setting.TemplateFileList)
            {
                makefile.Make(this);
            }
        }

        /// <summary>
        /// 查找定义的类型
        /// </summary>
        public List<string> DefineTypeList
        {
            get
            {
                ///合并自定义类型和默认类型
                var list = new List<string>();
                Root.PreorderTraversal(x =>
                {
                    if(x.nodeObj is CommonStructNode)
                    {
                        list.Add((x.nodeObj as CommonStructNode).StructName);
                    }
                });
                return setting.BaseTypeList.Concat<string>(list).ToList();
            }
        }
    }
}

