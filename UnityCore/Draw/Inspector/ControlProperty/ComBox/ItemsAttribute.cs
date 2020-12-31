namespace UnityCore
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ItemsAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 列表
        /// </summary>
        public ItemsAttribute(string[] data) : base(data)
        {

        }
        public ItemsAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {

        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            if(ctl is ComBoxAttribute)
            {
                var cb = ctl as ComBoxAttribute;
                var v = (System.Collections.IList)GetValue();
                if(v!=null)
                {
                    cb.Items =v;
                }
            }
        }
    }
}
