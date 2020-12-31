using CqCore;

/// <summary>
/// 自动化窗口菜单,修饰静态方法
/// 该方法所在的类需包含在名称空间Automation下
/// </summary>
public class AutoActionMenu : MemberAttribute
{
    public string menu;
    public int priority;

    AutoAction mAutoAction;
    AutoAction AutoAction
    {
        get
        {
            if (mAutoAction == null)
            {
                mAutoAction = AssemblyUtil.GetMemberAttribute<AutoAction>(Info);
            }
            return mAutoAction;
        }
    }

    public string Name
    {
        get
        {
            return AutoAction.name + string.Format("({0})", Info.Name);
        }
    }

    public AutoActionMenu(string menu, int priority)
    {
        this.menu = menu;
        this.priority = priority;
    }
    public void Invoke()
    {
        AutoAction.Invoke();
    }
}
