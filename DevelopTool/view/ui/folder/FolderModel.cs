using ResModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevelopTool
{
    [Priority(7)]
    [Editor("资源属性编辑器", "/DevelopTool;component/Res/Images/Icons/folder.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class FolderModel : SingleModel<FolderModel>
    {
        public override Setting Setting { get { return setting; } }
        public FolderSetting setting { get { return SettingModel.instance.GetSetting<FolderSetting>(); } }

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

        [TreeView("ResModel"), Priority(0, 1), Width(300), Height(520), SelectedValue("SelectObj"), Filter("FilterItem")]
        [Export("%Start%", "%End%")]
        public TreeNodeX Root
        {
            get { return mRoot; }
            set { mRoot = value; Update("Root"); }
        }
        TreeNodeX mRoot = new TreeNodeX();

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
            if (FileOpr.IsFolderPath(setting.FolderPath) )
            {
                var temp = Torsion.TryDeserialize<TreeNodeX>(FileOpr.ReadFile(setting.SetPath));
                Root = MakeFolderNode(setting.FolderPath, temp);
                Root.InitRelation();
            }
        }

        public override bool OnSave()
        {
            return FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(Root));
        }

        TreeNodeX MakeFolderNode(string path, TreeNodeX last)
        {
            var treenode = new TreeNodeX();
            var node = new FolderNode();
            if (last != null && last.nodeObj is FolderNode)
            {
                var temp = (last.nodeObj as FolderNode);
                node.mTypeIndex = temp.mTypeIndex;
            }
            treenode.nodeObj = node;
            node.Name = FileOpr.GetFileName(path);
            var folders = System.IO.Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                TreeNodeX it = null;
                if(last!=null)it=(TreeNodeX)last.Children.ToList().Find(x => x.nodeObj is FolderNode && x.Name == FileOpr.GetFileName(folder));
                treenode.AddChildren(MakeFolderNode(folder,it));
            }

            var files = System.IO.Directory.GetFiles(path);
            foreach (var file in files)
            {
                TreeNode it = null;
                if (last != null)it= last.Children.ToList().Find(x => x.nodeObj is FileNode && x.Name == FileOpr.GetFileName(file));
                if (it != null)
                {
                    treenode.AddChildren(it);
                }
                else
                {
                    var temp = new TreeNode();
                    var fileNode = new FileNode();
                    temp.nodeObj = fileNode;
                    fileNode.Name = FileOpr.GetFileName(file);
                    treenode.AddChildren(temp);
                }
            }
            return treenode;
        }
        /// <summary>
        /// 生成资源文件版本清单
        /// </summary>
        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;

            //setting.TemplateFile.Make(this);
        }
    }
}
