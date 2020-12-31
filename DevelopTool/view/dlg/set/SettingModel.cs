using DevelopTool;
using DevelopTool.Properties;
using System.Linq;

public class SettingModel : Singleton<SettingModel>
{
    public AllSetting setting;

    public OtherSetting OtherSet
    {
        get
        {
            if (mOtherSet == null)
            {
                mOtherSet = GetSetting<OtherSetting>();
            }
            return mOtherSet;
        }
    }
    DevelopTool.OtherSetting mOtherSet;
    public SettingModel()
    {
        string str = FileOpr.ReadFile(Resources.setPath);
        if(str.IsNullOrEmpty())
        {
            setting = new AllSetting();
        }
        else
        {
            setting = Torsion.Deserialize<AllSetting>(str);
        }
    }

    public void Save()
    {
        FileOpr.SaveFile(Resources.setPath, Torsion.Serialize(setting));
    }

    public T GetSetting<T>() where T : Setting
    {
        return (T)setting.List.ToList().Find(x => x is T);
    }
}