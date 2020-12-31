namespace CqCore
{
    public enum SerializeFormatStyle
    {
        Json,
        Torsion,
    }
    /// <summary>
    /// 序列化样式
    /// </summary>
    public class SerializeFormat
    {
        static SerializeFormat mJson;
        public static SerializeFormat Json
        {
            get
            {
                if (mJson == null) mJson = new SerializeFormat(SerializeFormatStyle.Json);
                return mJson;
            }
        }

        static SerializeFormat mTorsion;
        public static SerializeFormat Torsion
        {
            get
            {
                if (mTorsion == null) mTorsion = new SerializeFormat(SerializeFormatStyle.Torsion);
                return mTorsion;
            }
        }

        /// <summary>
        /// 序列化深度
        /// </summary>
        public int depthMax = 100;

        /// <summary>
        /// 序列化内容以类型起始(这样在反序列化时不用传递类型)
        /// </summary>
        public bool startWithType;

        /// <summary>
        /// 当不确定序列化的对象内部成员一定没有相互引用时,开启这个变量,排除对同一对象的重复序列化
        /// </summary>
        public bool withHashId { get; private set; }

        /// <summary>
        /// 对普通对象的默认序列化方式
        /// true:属性
        /// false:字段
        /// </summary>
        public bool serializeObjByProperty { get; private set; }

        /// <summary>
        /// 表达式分隔符
        /// </summary>
        public string ExpSeparator { get; private set; }

        /// <summary>
        /// 表达式结束符
        /// </summary>
        public char ExpEnd { get; private set; }

        /// <summary>
        /// 表达式起始
        /// </summary>
        public string ExpStart { get; private set; }

        /// <summary>
        /// 回车符
        /// </summary>
        public string Enter { get; private set; }

        /// <summary>
        /// 制表符
        /// </summary>
        char Tab = '\t';

        /// <summary>
        /// 获取多个制表符
        /// </summary>
        Causality<int, string> _GetTbls;

        public string GetTbls(int number)
        {
            if (_GetTbls == null) return null;
            return _GetTbls.Call(number);
        }

        /// <summary>
        /// 序列化样式
        /// </summary>
        /// <param name="se_style">序列化格式</param>
        /// <param name="withOutFormatChar">不包含格式控制符(换行或者制表符等)</param>
        /// <param name="serializeObjByProperty">不包含格式控制符(换行或者制表符等)</param>
        /// <param name="withHashId">当不确定序列化的对象内部成员一定没有相互引用时,开启记录对象的id,保证同一对象不被重复序列化</param>
        public SerializeFormat(SerializeFormatStyle se_style = SerializeFormatStyle.Torsion,
            bool withOutFormatChar=false,bool serializeObjByProperty=false,
            bool withHashId=false)
        {
            this.serializeObjByProperty = serializeObjByProperty;
            this.withHashId = withHashId;
            
            switch (se_style)
            {
                case SerializeFormatStyle.Json:
                    {
                        ExpSeparator = ":";
                        ExpEnd = ',';
                        ExpStart="\"";
                        break;
                    }
                case SerializeFormatStyle.Torsion:
                    {
                        ExpSeparator = "=";
                        ExpEnd = ';';
                        break;
                    }
            }
            
            if (!withOutFormatChar)
            {
                ExpSeparator = " "+ ExpSeparator + " ";
                Enter = "\r\n";
                _GetTbls = new Causality<int, string>((tblNum) => Tab.Repeat(tblNum));
            }
            else
            {
                Enter = null;
            }
        }

    }
}
