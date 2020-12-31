using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 控制台配置数据
    /// </summary>
    public class ConsoleConfig
    {
        public const string defaultConsoleIP = "yaowan.cqqqq.net";
        public string ip = defaultConsoleIP;
        public int port = 7777;
        public int hierarchyNameTblCount = 5;
        static ConsoleConfig mData;
        public static ConsoleConfig Inst
        {
            get
            {
                if (mData == null)
                {
                    if (PlayerPrefs.HasKey(key))
                    {
                        mData = Torsion.Deserialize<ConsoleConfig>(PlayerPrefs.GetString(key));
                    }
                    else
                    {
                        mData = new ConsoleConfig();
                        PlayerPrefs.SetString(key, Torsion.Serialize(mData));
                    }
                }
                return mData;
            }
            set
            {
                mData = value;
                PlayerPrefs.SetString(key, Torsion.Serialize(mData));
            }
        }
        const string key = "CommandConfig";
    }
}
