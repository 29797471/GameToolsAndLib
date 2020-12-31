using CqCore;
using Guide;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WinCore;

namespace DevelopTool
{
    [Priority(2)]
    [Editor("新手引导编辑器", "/DevelopTool;component/Res/Images/Icons/guide.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class GuideModel : SingleModel<GuideModel>,ICustomSerialize
    {
        public override Setting Setting { get { return setting; } }
        public GuideSetting setting { get { return SettingModel.instance.GetSetting<GuideSetting>(); } }
        

        [Image, Width(20)]
        public string FindIcon
        {
            get
            {
                return "/WinCore;component/Res/find.ico";
            }
        }

        [TextBox(), ToolTip("检索名称或者指引链id"), MinWidth(170), Priority(0,0,1)]
        public string Seach
        {
            get { return mSeach; }
            set { mSeach = value; Update("Seach"); Update("FilterItem"); }
        }
        string mSeach;

        [Button, Click("OnExportJson")]
        [Priority(0,0,2)]
        public string Btn { get { return "导出Json"; } }

        public class ExportLink
        {
            public string name;
            public System.Collections.Generic.List<string> list;
        }
        public void OnExportJson(object btn)
        {
            var dic = new Dictionary<string, ExportLink>();
            Root.PreorderTraversal(node =>
            {
                var obj = node.nodeObj as GuideLink;
                if (obj != null)
                {
                    var el = new ExportLink();
                    el.name = obj.Name;
                    el.list = new List<string>();
                    foreach(var it in obj.NodeList)
                    {
                        el.list.Add(it.Name);
                    }
                    dic[obj.IdName]=el;
                }
            });
            var f = "";
            WinUtil.SaveFileDialog(ref f, "文本文件(*.txt)|*.txt", "导出指引数据");
            System.IO.File.WriteAllText(f, JsonX.Serialize(dic), System.Text.Encoding.UTF8);
        }

        public Predicate<object> FilterItem
        {
            get
            {
                if (string.IsNullOrEmpty(Seach)) return null;
                
                return o => o.ToString().ToLower().Contains(Seach.ToLower()) ||
                ((o as TreeNode).nodeObj is GuideLink && ((o as TreeNode).nodeObj as GuideLink).IdName.Contains(Seach.ToLower()));
            }
        }

        [TreeView("Guide"), Priority(0, 1), Width(300), Height(520), SelectedValue("SelectObj"), Filter("FilterItem")]
        [Export("%Start%", "%End%")]
        public TreeNode Root
        {
            get { return mRoot; }
            set { mRoot = value; Update("Root"); }
        }
        TreeNode mRoot = new TreeNode();
        
        

        [Priority(1)]
        [GroupBox(), MinWidth(750), Height(570), Margin(30, 0)]
        public BaseTreeNotifyObject SelectObj
        {
            get { return mSelectObj; }
            set
            {
                try
                {
                    mSelectObj = value; Update("SelectObj");
                }
                catch (Exception e)
                {
                    CustomMessageBox.Show(e.Message);
                    throw;
                }
            }
        }
        BaseTreeNotifyObject mSelectObj;
        public override void OnHide()
        {
            SelectObj = null;
        }
        protected override void ReLoad()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            Root = Torsion.Deserialize<TreeNode>(str);
        }

        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(Root));
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            var makefile = setting.TemplateGuideLink;
            DirOpr.Delete(makefile.FolderPath);

            yield return Root.PreorderTraversalByCoroutine(node =>
            {
                var obj = node.nodeObj as GuideLink;
                if (obj != null)
                {
                    setting.TemplateGuideLink.Make(obj);
                    //obj.MakeFile();
                }
            });
            setting.TemplateRequire.Make(this);
        }
        
        public string InjectData(string content)
        {
            return StringUtil.ReplaceSubStr(content, "%START%", "%END%", x =>
            {
                var str = "";
                Root.PreorderTraversal(node =>
                {
                    var obj = node.nodeObj as GuideLink;
                    if (obj != null)
                    {
                        str = str + ExportAttribute.InjectData(x, obj);
                    }
                });
                return str;
            });
        }
    }
}

