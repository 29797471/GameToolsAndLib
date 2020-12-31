using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 词法分析器
    /// </summary>
    internal partial class CharParser : BaseParser<char>
    {
        /// <summary>
        /// 解析一个数字 (如:-1.501f ) 输出类型 int float<para/>
        /// 已支持科学计数法格式(-1.3e-2)
        /// </summary>
        bool TryParseNumber( out object data)
        {
            data = null;
            StringWriter sw = new StringWriter();

            int sign=1;//标记正负
            bool hasDot = false;//标记小数点

            if (Value == '-')
            {
                sign = -1;
                Next();
                if (!char.IsNumber(Value)) return false;
                sw.Write(Value);
                Next();
            }
            else if(!char.IsNumber(Value))
            {
                return false;
            }
            
            while (char.IsNumber(Value) || Value == '.')
            {
                if (Value == '.')
                {
                    if (hasDot) break;
                    hasDot = true;
                }

                sw.Write(Value);
                var bl=Next();
                if (bl) goto aa;
            }
            if (Value == 'f')
            {
                data = float.Parse(sw.ToString()) * sign;
                return true;
            }
            else if(Value=='e' || Value=='E')
            {
                sw.Write(Value);
                var bl = Next();
                if (bl) goto aa;
                if(char.IsNumber(Value) || Value == '+' || Value == '-')
                {
                    sw.Write(Value);
                    var _bl = Next();
                    if (_bl) goto aa;
                }
                while(char.IsNumber(Value))
                {
                    sw.Write(Value);
                    var _bl = Next();
                    if (_bl) goto aa;
                }
            }
            aa:
            int i;float f;long l;
            var result = sw.ToString();
            if ( int.TryParse(result, out i))
            {
                data = i * sign;
            }
            else if (long.TryParse(result, out l))
            {
                data = l * sign;
            }
            else if(float.TryParse(result, out f ))
            {
                data = f * sign;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
        
}
