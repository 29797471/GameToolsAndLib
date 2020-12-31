using System;

namespace DevelopTool
{
    [Priority(13)]
    [Editor("剧情编辑器", "/DevelopTool;component/Res/Images/Icons/play.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class StoryModel : SingleModel<StoryModel>
    {
        public override Setting Setting { get { return setting; } }
        public StorySetting setting { get { return SettingModel.instance.GetSetting<StorySetting>(); } }

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
                return o => (o as StoryData).Name.ToLower().Contains(Seach.ToLower());
            }
        }
        
        [Priority(0,1)]
        [ListBox(), Width(300), Height(520), Filter("FilterItem"), SelectedValue("SelectObj"), DisplayMember("Name")]
        public CustomList<StoryData> NodeList
        {
            get { return mNodeList; }
            set { mNodeList = value; Update("NodeList"); }
        }
        CustomList<StoryData> mNodeList = new CustomList<StoryData>();
        

        StoryData mSelectObj;

        [Priority(1)]
        [GroupBox(), MinWidth(170), Height(570), Margin(30, 0)]
        public StoryData SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }
        
        
        public StoryModel()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            NodeList = Torsion.Deserialize<CustomList<StoryData>>(str);
        }

       
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(NodeList));
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
        }
    }
}

