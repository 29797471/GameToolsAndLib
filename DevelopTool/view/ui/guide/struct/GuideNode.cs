using DevelopTool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guide
{
    [MenuItemPath("添加/指引节点(N)", null, System.Windows.Input.Key.N)]
    [Editor("指引节点")]
    public class GuideNode : BaseTreeNotifyObject
    {
        [Export("%ExportName%")]
        public string ExportName
        {
            get
            {
                return Name;
            }
        }

        [Export("%CompleteType%")]
        public string ExportCompleteType
        {
            get
            {
                return GuideModel.instance.setting.NodeTypesList.ToList().Find(x => x.Key == CompleteType).Value;
            }
        }
        public string CompleteType
        {
            get
            {
                if (mCompleteType == null)
                {
                    mCompleteType = NodeTypes[0];
                }
                return mCompleteType;
            }
            set
            {
                mCompleteType = value;
                Update("CompleteType");
                Update("ClickButton");
                Update("IsTrigger");
                Update("IsStoryDlg");
                Update("IsClickPos");
            }
        }
        public string mCompleteType;

        [Priority(1)]
        [ComboBox("完成方式",100), SelectedValue("CompleteType")]
        public List<string> NodeTypes
        {
            get { return GuideModel.instance.setting.NodeTypesList.ToList().ConvertAll(x => x.Key); }
        }

        public bool ClickButton
        {
            get
            {
                return mCompleteType.Contains("点击按钮");
            }
        }
        
        public bool IsTrigger
        {
            get
            {
                return mCompleteType.Contains("触发");
            }
        }
        public bool IsClickPos
        {
            get
            {
                return mCompleteType.Contains("指定位置");
            }
        }
        public bool IsStoryDlg
        {
            get
            {
                return mCompleteType.Contains("剧情对话");
            }
        }

        [Priority(3)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [Export("%WinCtlName%")]
        [TextBox("窗口:按钮", 100), MinWidth(200)]
        public string WinCtlName
        {
            get { return mWinCtlName; }
            set { mWinCtlName = value; Update("WinCtlName"); }
        }
        public string mWinCtlName;

        [Priority(4)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [Export("%DialogBtn%")]
        [UnderLine("特定按钮", 100), Width(350), ClickToEditCode()]
        public IExpression DialogBtn
        {
            get
            {
                if (mDialogBtn == null)
                    mDialogBtn = CodeStyleNewModel.instance.DefaultValue("object");
                return mDialogBtn;
            }
            set { mDialogBtn = value; Update("DialogBtn"); }
        }
        public IExpression mDialogBtn;

        [Export("%Dir%")]
        public int Dir
        {
            get
            {
                CustomList<KeyValue<int>> temp = null;
                if (GuideHandle == "手") temp = GuideModel.instance.setting.GuideDir;
                else temp = GuideModel.instance.setting.GuideArrowDir;
                return temp.ToList().Find(x=>x.Key==DirName).Value;
            }
        }

        [Export("%GuideHandle%")]
        public string E_GuideHandle
        {
            get
            {
                return GuideModel.instance.setting.GuideIcon.ToList().Find(x => x.Key == GuideHandle).Value;
            }
        }

        public string GuideHandle
        {
            get { if (mGuideHandle == null && GuideHandleList.Count>0) mGuideHandle = GuideHandleList[0]; return mGuideHandle; }
            set { mGuideHandle = value; Update("GuideHandle");Update("DirList"); }
        }
        public string mGuideHandle;

        [Priority(5, 0)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [ComboBox("指示", 50), SelectedValue("GuideHandle")]
        public List<string> GuideHandleList
        {
            get { return GuideModel.instance.setting.GuideIcon.ToList().ConvertAll(x => x.Key); }
        }

        public string DirName
        {
            get { if (mDirName == null && DirList.Count>0) mDirName = DirList[0]; return mDirName; }
            set { mDirName = value; Update("DirName"); }
        }
        public string mDirName;

        [Priority(5,1)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [ComboBox("方向", 50), SelectedValue("DirName")]
        public List<string> DirList
        {
            get
            {
                CustomList<KeyValue<int>> temp = null;
                if (GuideHandle == "手") temp = GuideModel.instance.setting.GuideDir;
                else temp = GuideModel.instance.setting.GuideArrowDir;

                return temp.ToList().ConvertAll(x => x.Key);
            }
        }

        [Export("%DeltaPos%")]
        [Priority(5,2)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [UnderLine("偏移"), MinWidth(100), MaxWidth(200), Click]
        public Vector2Float DeltaPos
        {
            get { if (mDeltaPos == null) mDeltaPos = new Vector2Float(); return mDeltaPos; }
            set { mDeltaPos = value; Update("DeltaPos"); }
        }
        public Vector2Float mDeltaPos;

        [Priority(3,1)]
        [Export("%MoveBtn%")]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [CheckBox("运动的按钮", 100), Width(100)]
        public bool MoveBtn
        {
            get { return mMoveBtn; }
            set { mMoveBtn = value; Update("MoveBtn"); }
        }
        public bool mMoveBtn;


        [Priority(6,1)]
        [Export("%ShowCircle%")]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [CheckBox("显示圈圈", 100)]
        public bool ShowCircle
        {
            get { return mShowCircle; }
            set { mShowCircle = value; Update("ShowCircle"); }
        }
        public bool mShowCircle=true;

        [Export("%CircleDeltaPos%")]
        [Priority(6, 2)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [UnderLine("偏移"), MinWidth(100), MaxWidth(200), Click]
        public Vector2Int CircleDeltaPos
        {
            get { if (mCircleDeltaPos == null) mCircleDeltaPos = new Vector2Int(); return mCircleDeltaPos; }
            set { mCircleDeltaPos = value; Update("CircleDeltaPos"); }
        }
        public Vector2Int mCircleDeltaPos;


        [Priority(8)]
        [Export("%ConditionDo%", 3)]
        [UnderLine("条件执行", 100), Width(350), ClickToEditCode()]
        public IExpression ConditionDo
        {
            get
            {
                if (mConditionDo == null) mConditionDo = CodeStyleNewModel.instance.DefaultValue("bool");
                return mConditionDo;
            }
            set { mConditionDo = value; Update("ConditionDo"); }
        }
        public IExpression mConditionDo;



        [Priority(10)]
        [Visibility("IsTrigger", AttributeTarget.Parent)]
        [SelectTreeNode("完成事件"), SelectedValue("Chooses")]
        public TreeNode EventRoot
        {
            get
            {
                return EventModel.instance.Root;
            }
        }
        public ObservableCollection<string> Chooses
        {
            get
            {
                return ComEvent.Chooses;
            }
            set
            {
                ComEvent.Chooses = value;
                Update("Chooses");
            }
        }
        [Export("%ComEvent%")]
        public EventExp ComEvent
        {
            get { if (mNewEvent == null) mNewEvent = new EventExp(); return mNewEvent; }
            set { mNewEvent = value; Update("ComEvent"); }
        }
        public EventExp mNewEvent;

        [Priority(11)]
        [Visibility("IsTrigger", AttributeTarget.Parent)]
        [Export("%Condition%", 3)]
        [UnderLine("完成条件", 100), Width(350), ClickToEditCode("ComEvent")]
        public IExpression Condition
        {
            get
            {
                if (mCondition == null) mCondition = CodeStyleNewModel.instance.DefaultValue("bool");
                return mCondition;
            }
            set { mCondition = value; Update("Condition"); }
        }
        public IExpression mCondition;

        [Export("%DialogIsLeft%")]
        [Priority(12,1)]
        [Visibility("IsStoryDlg", AttributeTarget.Parent)]
        [CheckBox("秘书",45), Width(50)]
        public bool DialogIsLeft
        {
            get {return mDialogIsLeft; }
            set { mDialogIsLeft = value; Update("DialogIsLeft"); }
        }
        public bool mDialogIsLeft;

        [Export("%AutoSkipping%")]
        [Priority(12, 2)]
        [Visibility("IsStoryDlg", AttributeTarget.Parent)]
        [CheckBox("自动跳过", 80), Width(50)]
        public bool AutoSkipping
        {
            get { return mAutoSkipping; }
            set { mAutoSkipping = value; Update("AutoSkipping"); }
        }
        public bool mAutoSkipping;

        [Export("%NoSkipping%")]
        [Priority(12,3)]
        [Visibility("IsStoryDlg", AttributeTarget.Parent)]
        [CheckBox("禁止跳过", 80), Width(50)]
        public bool NoSkipping
        {
            get { return mNoSkipping; }
            set { mNoSkipping = value; Update("NoSkipping"); }
        }
        public bool mNoSkipping;

        [Export("%DoOnceOnline%")]
        [Priority(12, 4)]
        //[Visibility("IsStoryDlg", AttributeTarget.Parent)]
        [CheckBox("在线只触发一次", 135)]
        public bool DoOnceOnline
        {
            get { return mDoOnceOnline; }
            set { mDoOnceOnline = value; Update("DoOnceOnline"); }
        }
        public bool mDoOnceOnline;
        //[Priority(14)]
        //[Export("%RoleIcon%")]
        //[Visibility("IsStoryDlg", AttributeTarget.Parent)]
        //[TextBox("人物图片", 100), Width(200)]
        //public string RoleIcon
        //{
        //    get { return mRoleIcon; }
        //    set { mRoleIcon = value; Update("RoleIcon"); }
        //}
        //public string mRoleIcon;
        
        [Priority(13)]
        [Visibility("IsTrigger", AttributeTarget.Parent)]
        [Export("%LockCamera%")]
        [CheckBox("锁定像机", 100), Width(100)]
        public bool LockCamera
        {
            set { mLockCamera = value; Update("LockCamera");  }
            get { return mLockCamera; }
        }
        public bool mLockCamera;

        [Priority(14)]
        [Visibility("IsTrigger", AttributeTarget.Parent)]
        [Export("%LinkWinName%")]
        [UnderLine("关联窗口", 100), Width(350), ClickToEditCode()]
        public IExpression LinkWinName
        {
            get
            {
                if (mLinkWinName == null)
                    mLinkWinName = CodeStyleNewModel.instance.DefaultValue("string");
                return mLinkWinName;
            }
            set { mLinkWinName = value; Update("LinkWinName"); }
        }
        public IExpression mLinkWinName;


        [Priority(15)]
        [Visibility("IsStoryDlg", AttributeTarget.Parent)]
        [Export("%DialogContent%")]
        [TextBox("对话内容", 100), MinWidth(150), AcceptsReturn]
        public string DialogContent
        {
            get
            {
                return mDialogContent;
            }
            set { mDialogContent = value; Update("DialogContent"); }
        }
        public string mDialogContent;

        [Priority(21)]
        [Visibility("IsClickPos", AttributeTarget.Parent)]
        [Export("%AboutWinName%")]
        [UnderLine("基于窗口", 100), Width(350), ClickToEditCode()]
        public IExpression AboutWinName
        {
            get
            {
                if (mAboutWinName == null)
                    mAboutWinName = CodeStyleNewModel.instance.DefaultValue("string");
                return mAboutWinName;
            }
            set { mAboutWinName = value; Update("AboutWinName"); }
        }
        public IExpression mAboutWinName;


        [Export("%ClickDeltaPos%")]
        [Priority(22)]
        [Visibility("IsClickPos", AttributeTarget.Parent)]
        [UnderLine("偏移"), MinWidth(100), MaxWidth(200), Click]
        public Vector2Int ClickDeltaPos
        {
            get { if (mClickDeltaPos == null) mClickDeltaPos = new Vector2Int(); return mClickDeltaPos; }
            set { mClickDeltaPos = value; Update("ClickDeltaPos"); }
        }
        public Vector2Int mClickDeltaPos;

        [Priority(23)]
        [Visibility("ClickButton", AttributeTarget.Parent)]
        [Export("%NoBlock%")]
        [CheckBox("不遮挡背景", 150), Width(150)]
        public bool NoBlock
        {
            set { mNoBlock = value; Update("NoBlock"); ShowCircle = !value; }
            get { return mNoBlock; }
        }
        public bool mNoBlock;

        //[Priority(18)]
        //[Visibility("IsTrigger", AttributeTarget.Parent)]
        //[SelectTreeNode("关闭手势滑动"), SelectedValue("StopHandMoveChooses")]
        public TreeNode StopHandMove
        {
            get
            {
                return EventModel.instance.Root;
            }
        }
        public ObservableCollection<string> StopHandMoveChooses
        {
            get
            {
                return StopHandMoveEvent.Chooses;
            }
            set
            {
                StopHandMoveEvent.Chooses = value;
                Update("Chooses");
            }
        }
        [Export("%StopHandMoveEvent%")]
        public EventExp StopHandMoveEvent
        {
            get { if (mStopHandMoveEvent == null) mStopHandMoveEvent = new EventExp(); return mStopHandMoveEvent; }
            set { mStopHandMoveEvent = value; Update("StopHandMoveEvent"); }
        }
        public EventExp mStopHandMoveEvent;

        public IExpression CompleteCondition
        {
            get { return mCompleteCondition; }
            set { mCompleteCondition = value; Update("CompleteCondition"); }
        }
        public IExpression mCompleteCondition;

        [Priority(26)]
        [Export("%EnterAction%")]
        [UnderLine("进入时执行", 100), Width(350), ClickToEditCode()]
        public IExpression EnterAction
        {
            get
            {
                if (mEnterAction == null)
                    mEnterAction = CodeStyleNewModel.instance.DefaultValue("void");
                return mEnterAction;
            }
            set { mEnterAction = value; Update("EnterAction"); }
        }
        public IExpression mEnterAction;

        [Priority(27)]
        [Export("%LeaveAction%")]
        [UnderLine("离开时执行", 100), Width(350), ClickToEditCode()]
        public IExpression LeaveAction
        {
            get
            {
                if (mLeaveAction == null)
                    mLeaveAction = CodeStyleNewModel.instance.DefaultValue("void");
                return mLeaveAction;
            }
            set { mLeaveAction = value; Update("LeaveAction"); }
        }
        public IExpression mLeaveAction;

        //[Priority(22)]
        //[Visibility("ClickButton", AttributeTarget.Parent)]
        [Export("%ExitOnClickComplete%")]
        //[CheckBox("完成点击后离开", 150), Width(150)]
        public bool ExitOnClickComplete
        {
            set { mExitOnClickComplete = value; Update("ExitOnClickComplete"); }
            get { return mExitOnClickComplete; }
        }
        public bool mExitOnClickComplete;


        [Priority(30)]
        [Export("%OpenCancelEvent%")]
        [CheckBox("接事件打断", 150), Width(150)]
        public bool OpenCancelEvent
        {
            set { mOpenCancelEvent = value; Update("OpenCancelEvent"); }
            get { return mOpenCancelEvent; }
        }
        public bool mOpenCancelEvent;


        [Priority(33)]
        [Visibility("OpenCancelEvent", AttributeTarget.Parent)]
        [SelectTreeNode("打断事件"), SelectedValue("CancelEventChooses")]
        public TreeNode CancelEventRoot
        {
            get
            {
                return EventModel.instance.Root;
            }
        }
        public ObservableCollection<string> CancelEventChooses
        {
            get
            {
                return CancelEvent.Chooses;
            }
            set
            {
                CancelEvent.Chooses = value;
                Update("CancelEventChooses");
            }
        }

        [Export("%CancelEvent%")]
        public EventExp CancelEvent
        {
            get { if (mCancelEvent == null) mCancelEvent = new EventExp(); return mCancelEvent; }
            set { mCancelEvent = value; Update("CancelEvent"); }
        }
        public EventExp mCancelEvent;


        [Priority(90)]
        [Export("%Arg0%")]
        [TextBox("附加参数", 100), MinWidth(200)]
        public float Arg0
        {
            get { return mArg0; }
            set { mArg0 = value; Update("Arg0"); }
        }
        public float mArg0;

        [Priority(100)]
        [TextBox("说明", 100), TextWrapping(System.Windows.TextWrapping.Wrap), Height(89), Width(350)]
        public string Comment
        {
            get { return mComment; }
            set { mComment = value; Update("Comment"); }
        }
        public string mComment;

        [Export("%GuideAniList~", "~GuideAniList%")]
        public List<GuideAni> GuideAniList
        {
            get
            {
                return Node.Children.ToList().ConvertAll(x => x.nodeObj as GuideAni);
            }
        }
    }
}
    