using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace DevelopTool
{
    [Editor("设置",null, Orientation.Horizontal), Width(1024), Height(720)]
    public class AllSetting : NotifyObject
    {
        [Priority(0)]
        [ListBox(), SelectedValue("Select")]
        [Width(200), Height(550), DisplayMember("Name")]
        public ObservableCollection<Setting> List
        {
            get
            {
                if (mList == null)
                {
                    var tts = AssemblyUtil.GetTypesByNamespace("DevelopTool").ToList();
                    var xxx = tts.FindAll(x => x.BaseType==typeof(Setting));

                    var temp = new List<Setting>();
                    foreach(var t in xxx)
                    {
                        var saveItem = saveData.Find(x => x.GetType() == t);
                        if(saveItem!=null)
                        {
                            temp.Add(saveItem);
                        }
                        else
                        {
                            temp.Add(AssemblyUtil.CreateInstance(t) as Setting);
                        }
                    }
                    temp.Sort(x =>
                    {
                        var attr = AssemblyUtil.GetClassAttribute<PriorityAttribute>(x);
                        return attr != null ? attr.pri1 : 0;
                    });
                    mList = new ObservableCollection<Setting>(temp);
                    saveData = temp;
                }
                return mList;
            }
        }
        ObservableCollection<Setting> mList;
        public List<Setting> saveData=new List<Setting>();

        [Priority(1)]
        [GroupBox(), Width(700), Height(570), Margin(30, 0)]
        public Setting Select
        {
            get
            {
                if (mSelect == null)
                {
                    CqCore.MathUtil.BetweenRange(ref UserSetting.Data.mLastSelectSet, 0, List.Count-1);
                    mSelect = List[UserSetting.Data.LastSelectSet];
                }
                return mSelect;
            }
            set
            {
                mSelect = value;
                Update("Select");
                UserSetting.Data.LastSelectSet = List.IndexOf(value);
            }
        }
        Setting mSelect;

        public List<string> unityInstallFolders;
    }
}
