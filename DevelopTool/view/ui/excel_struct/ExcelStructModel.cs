using System;
using System.Collections.ObjectModel;
using System.Drawing;
using WinCore;

/// <summary>
/// 编辑数据结构面板关联的数据模块
/// </summary>
namespace DevelopTool
{
    [Priority(6)]
    [Editor("配置表数据结构编辑器", "/DevelopTool;component/Res/Images/Icons/class.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class ExcelStructModel : SingleModel<ExcelStructModel>
    {
        public override Setting Setting { get { return setting; } }
        public CustomStructSetting setting { get { return SettingModel.instance.GetSetting<CustomStructSetting>(); } }
        

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
                return o => (o as CustomStruct).Name.ToLower().Contains(Seach.ToLower());
            }
        }

        [Priority(0,1)]
        [Export("%StructList~", "~StructList%")]
        [DropTarget, DragSource]
        [ListBox(), Width(300), Height(520), Filter("FilterItem"), SelectedValue("SelectObj"), DisplayMember("Name")]
        public CustomList<CustomStruct> CustomStructList
        {
            get { return mCustomStructList; }
            set { mCustomStructList = value; Update("CustomStructList"); }
        }

        CustomList<CustomStruct> mCustomStructList = new CustomList<CustomStruct>();
        
        [Priority(1)]
        [GroupBox(), Width(700), Height(570), Margin(30, 0)]
        public CustomStruct SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }
        CustomStruct mSelectObj;
        public override void OnHide()
        {
            SelectObj = null;
        }
        public ExcelStructModel()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            mCustomStructList = Torsion.Deserialize<CustomList<CustomStruct>>(str);
        }
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(mCustomStructList));
            return true;
        }
        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            setting.TemplateFile.Make(this);
        }
    }
}

