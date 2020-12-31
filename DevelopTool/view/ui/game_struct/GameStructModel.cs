using CqCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using WinCore;

/// <summary>
/// 编辑数据结构面板关联的数据模块
/// </summary>
namespace DevelopTool
{
    [Priority(1)]
    [Editor("数据结构编辑器", "/DevelopTool;component/Res/Images/Icons/class.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class GameStructModel : SingleModel<GameStructModel>
    {
        public override Setting Setting { get { return setting; } }
        public GameStructSetting setting { get { return SettingModel.instance.GetSetting<GameStructSetting>(); } }
        

        [Image, Width(20)]
        public string FindIcon
        {
            get
            {
                return "/WinCore;component/Res/find.ico";
            }
        }

        [Priority(0,0,1)]
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
                return o => (o as GameStruct).Name.ToLower().Contains(Seach.ToLower());
            }
        }
        [Priority(0, 1)]
        [DropTarget, DragSource]
        [Export("%Start_StructList%", "%End_StructList%")]
        [ListBox(), Width(300), Height(520), Filter("FilterItem"), SelectedValue("SelectObj"), DisplayMember("Name")]
        public CustomList<GameStruct> NodeList
        {
            get { return mNodeList; }
            set { mNodeList = value; Update("NodeList"); }
        }

        CustomList<GameStruct> mNodeList;
        

        GameStruct mSelectObj;

        [Priority(1)]
        [GroupBox(), Width(820), Height(570), Margin(30, 0)]
        public GameStruct SelectObj
        {
            get {  return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }

        /// <summary>
        /// 由游戏结构定义的类型和基础类型
        /// </summary>
        public List<string> DefineTypeList
        {
            get
            {
                ///合并自定义类型和默认类型
                var k = NodeList.ToList().ConvertAll(x => x.Name);
                k.Sort();
                return setting.BaseTypeList.Concat<string>(k).ToList();
            }
        }
        public override void OnHide()
        {
            SelectObj = null;
        }
        protected override void ReLoad()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            mNodeList = Torsion.Deserialize<CustomList<GameStruct>>(str);
        }
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(mNodeList));
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
    }
}

