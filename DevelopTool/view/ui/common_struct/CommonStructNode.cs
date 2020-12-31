using DevelopTool;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonStruct
{

    [MenuItemPath("添加/数据结构(N)", null, System.Windows.Input.Key.N)]
    [Editor("数据结构")]
    public class CommonStructNode : BaseTreeNotifyObject
    {
        [Export("%NameSpace%")]
        public string NameSpace
        {
            get
            {
                string n = null;
                var p = Node;
                while(p!=null )
                {
                    if(p.nodeObj is CommonStructGroup)
                    {
                        if (n == null) n = (p.nodeObj as CommonStructGroup).NameSpace;
                        else n = (p.nodeObj as CommonStructGroup).NameSpace +"."+ n;
                    }
                    p = p.Parent;
                }
                return n;
            }
        }
        [Export("%NodeName%")]
        public string NodeName
        {
            get
            {
                return Name;
            }
        }
        [Priority(1)]
        [Export("%StructName%")]
        [TextBox("结构名称", 100), MinWidth(200)]
        public string StructName
        {
            get { return mStructName; }
            set { mStructName = value; Update("StructName"); }
        }
        public string mStructName;

        /// <summary>
        /// 类修饰符
        /// </summary>

        [Export("%ClassDefine%")]
        public string ClassDefine
        {
            get { if (mClassDefine == null) mClassDefine = ClassDefineList[0]; return mClassDefine; }
            set { mClassDefine = value; Update("ClassDefine"); }
        }

        public string mClassDefine;


        /// <summary>
        /// 类修饰符
        /// </summary>
        [ComboBox("类修饰符", 100), SelectedValue("ClassDefine"), Priority(3)]
        public ObservableCollection<string> ClassDefineList { get { return GameStructModel.instance.setting.ClassDefineList; } }

        [Priority(4)]
        [Export("%Additional%")]
        [TextBox("附加信息", 100), MinWidth(200)]
        public string Additional
        {
            get { return mAdditional; }
            set { mAdditional = value; Update("Additional"); }
        }
        public string mAdditional;

        /// <summary>
        /// 该项是否为数组
        /// </summary>
        [Export("%FunStart%", "%FunEnd%")]
        [Priority(5)]
        [CheckBox("导出通信函数")]
        public bool CanExportFun
        {
            get { return mCanExportFun; }
            set { mCanExportFun = value; Update("CanExportFun"); }
        }
        public bool mCanExportFun;

        [Priority(10)]
        [CheckBox("打印", 100)]
        [Export("%Print%")]
        public bool Print { get { return mPrint; } set { mPrint = value; Update("Print"); } }
        public bool mPrint;

        [Export("%StartItem%", "%EndItem%",0,true)]
        [ListView(), MinHeight(200), Priority(20)]
        public CustomList<CommonStructItem> CustomerList
        {
            get { if (customerList == null) customerList = new CustomList<CommonStructItem>(); return customerList; }
            set { customerList = value; Update("CustomerList"); }
        }

        public CustomList<CommonStructItem> customerList;

    }
}


