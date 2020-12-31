using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCore
{
    /// <summary>
    /// 暂时未实现,难度较高,实现类似与正则表达式的状态机逻辑
    /// 解析效率相对固定流程较低,但是易于读写和修改
    ///
    /// 例解析一个类: n { loop{n=v;} }
    /// </summary>
    internal class ParserMatch : Singleton<ParserMatch>
    {
        TokenType MatchMap(char c)
        {
            switch (c)
            {
                case 'n':
                    return TokenType.VARIABLE;
                case ';':
                    return TokenType.SEMICOLON;
                case '{':
                    return TokenType.LEFT_BRACE;
                case '}':
                    return TokenType.RIGHT_BRACE;
            }
            return TokenType.KEYWORD;
        }

        public bool ParserObject(TokenParser list)
        {
            return ParserByMatch(list, "n{loop{n=v;}}");
        }
        public bool ParserArray(TokenParser list)
        {
            return ParserByMatch(list, "n[?v<,v>]");
        }
        public bool ParserValue(TokenParser list)
        {
            return ParserByMatch(list, "/oab/");
        }

        public bool ParserByMatch(TokenParser list, string match)
        {
            var matchList = match.Replace(" ", "").ToArray();

            int loopStart = -1;
            bool canFalse = false;
            bool result = true;
            for (int pos = 0; pos != matchList.Length; pos++)
            {
                char c = matchList[pos];
                result = true;
                switch (c)
                {
                    case 'b':
                        switch (list.Value.type)
                        {
                            case TokenType.NUMBER:
                            case TokenType.STRING:
                            case TokenType.KEYWORD:
                                break;
                            default:
                                result = false;
                                break;
                        }
                        break;
                    case '/'://匹配其中一个
                        break;
                    case '?'://可有可无
                        canFalse = true;
                        break;
                    case 'v':
                        result = ParserValue(list);
                        if (!canFalse && !result) return false;
                        canFalse = false;
                        break;
                    case '<':
                        loopStart = pos + 1;
                        break;
                    case '>':
                        if (list.Value.type == MatchMap(matchList[loopStart]))
                        {
                            pos = loopStart;
                        }
                        break;
                    default:
                        result = (list.Value.type == MatchMap(c));
                        if (!canFalse && !result) return false;
                        canFalse = false;
                        break;

                }

            }

            return true;
        }
        //bool TryParseObject(IntOutList_BaseToken par, ref object v)
        //{
        //    var token = par.ReadSkipComment();
        //    if (token.type != BaseTokenType.VARIABLE) return false;
        //    if (par.ReadSkipComment().type != (BaseTokenType.LEFT | BaseTokenType.BRACE)) return false;
        //    Type objType = GetTypeByName(token.value.ToString());
        //    var obj = Activator.CreateInstance(objType);
        //    Dictionary<string, FieldInfo> dic = GetFieldInfo(objType);
        //    while (par.PeekSkipComment().type != (BaseTokenType.RIGHT | BaseTokenType.BRACE))
        //    {
        //        if (par.PeekSkipComment().type != BaseTokenType.VARIABLE) return false;

        //        var expName = par.ReadSkipComment().value.ToString();

        //        if (par.ReadSkipComment().type != BaseTokenType.EQUAL) return false;

        //        if (!InjectParser(par, TryParseValue, o => { if (dic.ContainsKey(expName)) dic[expName].SetValue(obj, o); })) return false;

        //        if (par.ReadSkipComment().type != BaseTokenType.SEMICOLON) return false;
        //    }
        //    par.ReadSkipComment();
        //    v = obj;
        //    return true;
        //}
    }
}

