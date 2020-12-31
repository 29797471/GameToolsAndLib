using DevelopTool;
using System.Collections.ObjectModel;
using System.Linq;

namespace Trigger
{

    [MenuItemPath("添加/触发节点(N)", null, System.Windows.Input.Key.N)]
    [Editor("触发节点")]
    public class TriggerNode : BaseTreeNotifyObject, IExport
    {
        [Export("%NodeNameList%")]
        public string NodeNameList
        {
            get
            {
                return string.Join(" -> ", Node.GetPath());
            }
        }

        [Priority(4)]
        [CheckBox("导出代码")]
        public bool CanExport
        {
            get { return mExportCode; }
            set { mExportCode = value; Update("ExportCode"); }
        }
        public bool mExportCode=true;

        [Priority(3)]
        [Export("%OpenAction%",3)]
        [UnderLine("执行", 50), MinWidth(550), ClickToEditCode("Event")]
        public IExpression Action
        {
            get
            {
                if (mAction == null)
                {
                    mAction = CodeStyleNewModel.instance.DefaultValue(TriggerModel.instance.setting.Action,
                        TriggerModel.instance.setting.Language);
                }
                if (mAction.Language != TriggerModel.instance.setting.Language)
                {
                    mAction.Language = TriggerModel.instance.setting.Language;
                }
                return mAction;
            }
            set { mAction = value; Update("Action"); }
        }
        public IExpression mAction;

        [Priority(2)]
        [Export("%OpenCondition%",2)]
        [UnderLine("条件", 50), ClickToEditCode("Event"), MinWidth(550)]
        public IExpression Condition
        {
            get
            {
                if (mCondition == null)
                {
                    mCondition = CodeStyleNewModel.instance.DefaultValue(TriggerModel.instance.setting.Condition,
                        TriggerModel.instance.setting.Language);
                }
                if(mCondition.Language != TriggerModel.instance.setting.Language)
                {
                    mCondition.Language = TriggerModel.instance.setting.Language;
                }
                return mCondition;
            }
            set
            {
                mCondition = value;
                Update("Condition");
            }
        }
        public IExpression mCondition;

        
        public ObservableCollection<string> Chooses
        {
            get
            {
                return Event.Chooses;
            }
            set
            {
                Event.Chooses = value;
                Update("Chooses");
            }
        }

        [Priority(1)]
        [SelectTreeNode("事件"),SelectedValue("Chooses")]
        public TreeNode EventRoot
        {
            get
            {
                return EventModel.instance.Root;
            }
        }

        //[Priority(3)]
        [Export("%NewEvent%")]
        //[GroupBox()]
        public EventExp Event
        {
            get { if (mEvent == null) mEvent = new EventExp(); return mEvent; }
            set { mEvent = value; Update("Event"); }
        }
        public EventExp mEvent;


        [Export("%Name%")]
        public string OutName
        {
            get
            {
                return Name;
            }
        }
    }
}

