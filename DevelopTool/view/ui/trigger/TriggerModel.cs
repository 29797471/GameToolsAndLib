using System;
using System.Collections;
using System.Linq;
using Trigger;
using WinCore;

namespace DevelopTool
{
    [Priority(4)]
    [Editor("触发器", "/DevelopTool;component/Res/Images/Icons/trigger.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class TriggerModel : SingleModel<TriggerModel>
    {
        /// <summary>
        /// 触发器对应语言
        /// </summary>
        public CodeStyleLanguage CodeStyleLanguage
        {
            get
            {
                return CodeStyleNewModel.instance.GetLanguageData( setting.Language);
            }
        }
        public override Setting  Setting { get { return setting; } }
        public TriggerSetting setting { get { return SettingModel.instance.GetSetting<TriggerSetting>(); } }
        
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

        [TreeView("Trigger"), Priority(0, 1), Width(300), Height(520), SelectedValue("SelectObj"), Filter("FilterItem")]
        [Export("%Start%", "%End%")]
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
        public override void OnHide()
        {
            SelectObj = null;
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
            setting.TemplateFile.Make(this);
        }

        [Export("%Var%", "%_Var%")]
        public CustomList<TriggerVar> CustomerList
        {
            get
            {
                return setting.CustomerList;
            }
        }


        /// <summary>
        /// 将变量名称转译成代码中的变量名
        /// </summary>
        public string ToVarName(string Var)
        {
            return setting.PrefixVar + CustomHash.CRCHash(Var).ToString();
        }
    }
}
