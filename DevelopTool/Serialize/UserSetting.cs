using System.Collections.Generic;

namespace DevelopTool
{
    /// <summary>
    /// 存储用户相关的设置
    /// </summary>
    public class UserSettingData
    {
        /// <summary>
        /// 上次选中的任务栏
        /// </summary>
        public int LastSelectItem
        {
            get
            {
                return mLastSelectItem;
            }
            set
            {
                mLastSelectItem = value;
                UserSetting.Save();
            }
        }
        public int mLastSelectItem;

        /// <summary>
        /// 上次选中的设置栏
        /// </summary>
        public int LastSelectSet
        {
            get
            {
                return mLastSelectSet;
            }
            set
            {
                mLastSelectSet = value;
                UserSetting.Save();
            }
        }
        public int mLastSelectSet;


        public Dictionary<string, bool> mPrintDic;
        Dictionary<string, bool> PrintDic
        {
            get
            {
                if (mPrintDic == null)
                {
                    mPrintDic = new Dictionary<string, bool>();
                }
                return mPrintDic;
            }
        }
        public bool this[string name]
        {
            get
            {
                if (PrintDic.ContainsKey(name)) return PrintDic[name];
                else return false;
            }
            set
            {
                if(value)
                {
                    PrintDic[name] = true;
                }
                else
                {
                    PrintDic.Remove(name);
                }
                UserSetting.Save();
            }
        }

        public OpenUnityBox openUnityBox=new OpenUnityBox();

    }
    internal static class UserSetting
    {
        static string file = System.Environment.UserName + ".dat";

        static UserSettingData mData;

        public static UserSettingData Data
        {
            get
            {
                if (mData == null)
                {
                    mData = Torsion.TryDeserialize<UserSettingData>(FileOpr.ReadFile(file));
                }
                return mData;
            }
        }

        internal static void Save()
        {
            FileOpr.SaveFile(file, Torsion.Serialize(mData));
        }
    }
}
