namespace UnityCore
{
    /// <summary>
    /// 修饰一个组件成员,使这个成员的控件特性中可以获取到组件中的其它属性
    /// </summary>
    public class ComponentMemberAttribute : ControlPropertyAttribute
    {
        public ComponentMemberAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }

        protected override void OnInit(ControlAttribute ctl)
        {
            if (ctl is IGetComMemberAttribute)
            {
                //Debug.LogError("set:" + ctl);
                (ctl as IGetComMemberAttribute).GetOtherMemberValue = GetValue;
            }
        }
    }
}
