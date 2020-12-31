using DevelopTool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guide
{
    [MenuItemPath("添加/指引动作(A)", null, System.Windows.Input.Key.A)]
    [Editor("指引动作")]
    public class GuideAni: BaseTreeNotifyObject
    {
        [Export("%AniName%")]
        public string AniName
        {
            get
            {
                return Name;
            }
        }
        [Export("%ActionId%")]
        public string ActionId
        {
            get
            {
                return GuideModel.instance.setting.StoryAction.ToList().Find(x => x.Key == ActionName).Value;
            }
        }
        public string ActionName
        {
            get { if (mActionName == null && ActionList.Count>0) mActionName = ActionList[0]; return mActionName; }
            set { mActionName = value; Update("ActionName"); }
        }
        public string mActionName;

        [Priority(5)]
        [ComboBox("剧情动作", 100), MinWidth(100), SelectedValue("ActionName")]
        public List<string> ActionList
        {
            get
            {
                return GuideModel.instance.setting.StoryAction.ToList().ConvertAll(x => x.Key);
            }
        }
        [Export("%WaitTime%")]
        [Priority(6)]
        [TextBox("等待",100), MinWidth(100)]
        public float WaitTime { get { return mWaitTime; } set { mWaitTime = value; Update("WaitTime"); } }
        public float mWaitTime;
    }
}
