using CqCore;

namespace ParserCore
{
    public class TokenEnumElementAttribute: EnumLabelAttribute
    {
        public char[] args;
        public TokenEnumElementAttribute(string name,string args=null):base(name)
        {
            if (args == null) this.args = new char[0];
            else this.args = args.ToCharArray();
        }
    }
}
