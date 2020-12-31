using DevelopTool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guide
{
    [MenuItemPath("添加/指引链(L)", null, System.Windows.Input.Key.L)]
    [Editor("指引链")]
    public class GuideLink : BaseTreeNotifyObject
    {
        [Export("%ExportName%")]
        public string ExportName
        {
            get
            {
                return Name;
            }
        }
        [Priority(1)]
        [Export("%IdName%")]
        [TextBox("指引Id",100),IsEnabled(false)]
        public string IdName
        {
            get
            {
                return CustomHash.CRCHash(Name).ToString();
            }
        }

        [Priority(20)]
        [TextBox("说明",100),TextWrapping(System.Windows.TextWrapping.Wrap),Height(89),Width(350)]
        public string Comment { get { return mComment; } set { mComment = value; Update("Comment"); } }
        public string mComment;

        [Export("%Trigger_Start%", "%Trigger_End%", 1, true)]
        [Priority(4)]
        [DropTarget, DragSource]
        [ListBox("指引触发列表"), Width(400), Height(100), DoubleSelectedValue(DoubleClickStyle.OpenEditorWindow)]
        public CustomList<GuideTrigger> TriggerList
        {
            get
            {
                if (mTriggerList == null) mTriggerList = new CustomList<GuideTrigger>(); return mTriggerList;
            }
            set
            {
                mTriggerList = value; Update("TriggerList");
            }
        }
        public CustomList<GuideTrigger> mTriggerList;

        
        public string LinkType
        {
            set
            {
                linkType = value;
                Update("LinkType");
            }
            get
            {
                if (linkType == null) linkType = LinkTypes[0];
                return linkType;
            }
        }
        public string linkType;

        [Export("%LinkTypeValue%")]
        public string LinkTypeValue
        {
            get
            {
                return GuideModel.instance.setting.LinkTypesList.ToList().Find(x=>x.Key==linkType).Value;
            }
        }

        [Priority(2)]
        [Export("%CheckInSpecialEvent%")]
        [CheckBox("任务刷新触发", 220), Width(100)]
        public bool CheckInSpecialEvent
        {
            get { return mCheckInSpecialEvent; }
            set { mCheckInSpecialEvent = value; Update("CheckInSpecialEvent"); }
        }
        public bool mCheckInSpecialEvent;

        [Priority(19)]
        [Export("%CompleteAction%")]
        [UnderLine("完成后执行", 100), Width(350), ClickToEditCode("NewEvent")]
        public IExpression CompleteAction
        {
            get { if (mCompleteAction == null) mCompleteAction = CodeStyleNewModel.instance.DefaultValue("void"); return mCompleteAction; }
            set { mCompleteAction = value; Update("CompleteAction"); }
        }
        public IExpression mCompleteAction;

        [Priority(15)]
        [ComboBox("执行方式",100),SelectedValue("LinkType")]
        public static List<string> LinkTypes
        {
            get
            {
                return GuideModel.instance.setting.LinkTypesList.ToList().ConvertAll(x => x.Key);
            }
        }

        [Export("%NodeList_Start%", "%NodeList_End%")]
        public List<GuideNode> NodeList
        {
            get
            {
                return Node.Children.ToList().ConvertAll(x => x.nodeObj as GuideNode);
            }
        }
    }

}
