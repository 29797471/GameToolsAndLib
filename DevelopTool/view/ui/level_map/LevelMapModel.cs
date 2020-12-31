using System;

namespace DevelopTool
{
    [Priority(8)]
    [Editor("关卡布局编辑器", "/DevelopTool;component/Res/Images/Icons/map.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class LevelMapModel : SingleModel<LevelMapModel>
    {
        public override Setting Setting { get { return setting; } }
        public LevelMapSetting setting { get { return SettingModel.instance.GetSetting<LevelMapSetting>(); } }

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

        [Priority(0,1)]
        [DropTarget, DragSource]
        [ListBox(), Width(300), Height(520), Filter("FilterItem"), SelectedValue("SelectObj"), DisplayMember("Name")]
        [Export("%Item_Start%", "%Item_End%")]
        public CustomList<LevelMapNode> NodeList
        {
            get {return mRoot; }
            set { mRoot = value; Update("Root"); }
        }
        CustomList<LevelMapNode> mRoot= new CustomList<LevelMapNode>();
        
        

        [Priority(1)]
        [GroupBox(), MinWidth(170), Height(570), Margin(30, 0)]
        public LevelMapNode SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }LevelMapNode mSelectObj;

        public LevelMapModel()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            NodeList = Torsion.Deserialize<CustomList<LevelMapNode>>(str);
        }
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(mRoot));
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            var savePath = setting.TemplateFile.mFolderPath + @"\" + setting.TemplateFile.mFileName;
            FileOpr.SaveFile(savePath, Torsion.Serialize(NodeList));
            //setting.TemplateFile.Make(this);
        }
    }
}

