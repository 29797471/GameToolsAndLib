using DevelopTool;
using System.Collections.ObjectModel;
using System.Linq;

namespace CqEvent
{
    [MenuItemPath("添加/事件节点(N)",null,System.Windows.Input.Key.N)]
    [Editor("事件节点")]
    public class EventNode : BaseTreeNotifyObject//, ICustomSerialize
    {

        [Priority(1)]
        [TextBox("事件" ,100), MinWidth(300)]
        [Export("%EventId%")]
        public string EventId { get { return m_EventId; } set { m_EventId = value; Update("EventId"); } }
        public string m_EventId;
        public string ExecContent
        {
            get
            {
                return EventId;
            }
        }

        [Priority(4)]
        [ListView(),MinHeight(200)]
        [Export("%Start%", "%End%",1,true)]
        public CustomList<EventStructItem> CustomerList
        {
            get { if (customerList == null) customerList = new CustomList<EventStructItem>(); return customerList; }
            set { customerList = value; Update("CustomerList"); }
        }

        public CustomList<EventStructItem> customerList;

        [Priority(3)]
        [CheckBox("打印",50)]
        [Export("%Print[%", "%]Print%")]
        public bool Print
        {
            get { return UserSetting.Data["Event_"+Name]; }
            set
            {
                UserSetting.Data["Event_"+Name] = value;
                Update("Print");
            }
        }
        //public bool mPrint;


        [Priority(3,1)]
        [CheckBox("Lua事件", 80),Margin(100,0,0,0,AttributeTarget.Parent)]
        [Export("%IsLua[%","%M%", "%]IsLua%")]
        public bool IsLua { get { return mIsLua; } set { mIsLua = value; Update("IsLua"); } }
        public bool mIsLua;

        [Export("%Name%")]
        public string ExportName
        {
            get
            {
                return Name;
            }
        }

        [Export("%Path%")]
        public string Path
        {
            get
            {
                return string.Join(" -> ", Node.GetPath());
            }
        }

        [Priority(11)]
        [TextBox("说明", 100), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true)]
        [MinWidth(300),MinHeight(80)]
        public string Content { get { return mContent; } set { mContent = value; Update("Content"); } }
        public string mContent;
    }
}


