//定义序列化过程输出打印
//#define Print_Parser

using CqCore;
using ParserCore;
using System;
using System.Collections;
using System.Collections.Generic;

public static partial class CqSerialize
{
    /// <summary>
    /// 按类型解析
    /// 1.基本数据类型
    /// 2.枚举
    /// 3.数组
    /// 4.字典
    /// 5.自定义类
    /// </summary>
    internal class Parser : TokenParser
    {
        ParserFormat format;
        Dictionary<int, object> refObjDic;
        Dictionary<int, Action<object>> refActionDic;
        public void RefObject(int hashId,Action<object> SetValue)
        {
            if(refObjDic.ContainsKey(hashId))
            {
                SetValue(refObjDic[hashId]);
            }
            else
            {
                if (!refActionDic.ContainsKey(hashId)) refActionDic[hashId] = null;
                refActionDic[hashId] += SetValue;
            }
        }
        public void DefineObject(int hashId, object v)
        {
            if (refActionDic.ContainsKey(hashId))
            {
                refActionDic[hashId](v);
                refActionDic.Remove(hashId);
            }

            refObjDic[hashId] = v;
        }
        public Parser(ParserFormat format,List<Token> list) : base(list)
        {
            this.format = format;
            refObjDic = new Dictionary<int, object>();
            refActionDic = new Dictionary<int, Action<object>>();
        }
        public object ParseObject()
        {
            object v;
            var bl = TryParseObject(null, out v);
#if Print_Parser
        if(bl)CqCore.CqDebug.Log("解析成功"+v);
        else CqCore.CqDebug.Log("解析失败");
#endif
            if (bl)
            {
                return v;
            }
            else return null;
        }
        public object ParseValue(Type type)
        {
            object v;
            var bl = TryParseValue(type, out v);
            if (bl)
            {
                return v;
            }
            else return null;
        }
        /// <summary>
        /// 输出类型
        /// 1.自定义类型
        /// 2.整形数字为int
        /// 3.浮点数字为float
        /// 4.字符串为string
        /// 5.布尔为bool
        /// </summary>
        public bool TryParseValue(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseValue type:"+type+ " Value.type:"+ Value.type+ "Value.value" + Value.value);
#endif
            //解析到数字,字符串,常量,一定不是对象,列表和字典,因为它们都以符号或者变量名起始
            switch (Value.type)
            {
                case TokenType.NUMBER:
                    {
                        if (type == typeof(DateTime)) v = new DateTime((long)Value.value);
                        else v = ConvertUtil.ConvertType(Value.value, type);
                        NextSkipComment();
                        return true;
                    }
                case TokenType.STRING:
                case TokenType.CHAR:
                case TokenType.KEYWORD:
                    {
                        if (Value.value.ToString() == "ref")
                        {
                            v = null;
                            return false;
                        }
                        else
                        {
                            v = ConvertUtil.ConvertType(Value.value, type);
                            NextSkipComment();
                        }
                        return true;
                    }
            }

            if (type.IsEnum)
            {
                //读枚举值
                string enumValue = Value.value.ToString();
                NextSkipComment();
                while (Value.type == TokenType.COMMA)
                {
                    enumValue += ",";
                    NextSkipComment();
                    enumValue += Value.value.ToString();
                    NextSkipComment();
                }
                v = Enum.Parse(type, enumValue);

                return true;
            }

            if (Value.IsNull())
            {
                v = Value.value;
                NextSkipComment();
                return true;
            }

            object parseType;
            if (TryParseLogic(TryParseType, type, out parseType))
            {
                type = (Type)parseType;
            }
            object hashId=null;
            if(type.IsClass)
            {
                if(TryParseLogic(TryParseHashId, type, out hashId))
                {
                }
            }
            bool bl;
            if (AssemblyUtil.IsList(type))
            {
                bl= TryParseLogic(TryParseList, type, out v);
            }
            else if (AssemblyUtil.IsArray(type))
            {
                bl= TryParseLogic(TryParseArray, type, out v);
            }
            else if (AssemblyUtil.IsDictionary(type))
            {
                bl= TryParseLogic(TryParseDictionary, type, out v);
            }
            else
            {
                bl= TryParseLogic(TryParseObject, type, out v);
            }
            if(hashId!=null)
            {
                DefineObject((int)hashId, v);
            }
            return bl;
        }

        bool TryParseHashId(Type type,out object v)
        {
            v = null;
            if (Value.type != TokenType.LEFT_PARENTHESIS) return false;
            NextSkipComment();
            if (Value.type != TokenType.NUMBER) return false;
            var hashId = (int)Value.value;
            NextSkipComment();
            if (Value.type != TokenType.RIGHT_PARENTHESIS) return false;
            NextSkipComment();
            v = hashId;
            return true;
        }

        /// <summary>
        /// 解析类型名,包含泛型,以及泛型嵌套类型
        /// 1.  List&lt;T&gt;
        /// 2. T() 数组
        /// 3.  Dictionary&lt;Tkey,Tvalue&gt;
        /// 4. T
        /// </summary>
        bool TryParseType(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseType");
#endif
            v = null;
            if (Value.type != TokenType.VARIABLE) return false;

            var baseType = AssemblyUtil.GetType(Value.value.ToString());
            if (baseType == null)
            {
                throw new Exception("反序列化时找不到类型" + Value.value);
            }
            NextSkipComment();

            if (Value.type == (TokenType.LEFT_ANGLE))
            {
                NextSkipComment();
                List<Type> types = new List<Type>();
                while (Value.type != (TokenType.RIGHT_ANGLE))
                {
                    object o = null;
                    if (!TryParseLogic(TryParseType, null, out o))
                    {
                        return false;
                    }
                    types.Add((Type)o);
                    if (Value.type == TokenType.COMMA) NextSkipComment();
                }
                NextSkipComment();
                switch (types.Count)
                {
                    case 1:
                        v = baseType.MakeGenericType(types[0]);
                        break;
                    case 2:
                        v = baseType.MakeGenericType(types[0], types[1]);
                        break;
                    default:
                        v = baseType.MakeGenericType(types.ToArray());
                        break;
                }
                return true;
            }
            else if (Value.type == (TokenType.LEFT_BRACKET))
            {
                int rank = 1;
                NextSkipComment();
                while(Value.type== TokenType.COMMA)
                {
                    rank++;
                    NextSkipComment();
                }
                if (Value.type != (TokenType.RIGHT_BRACKET)) return false;//throw new Exception("未找到匹配的右中括号");
                NextSkipComment();
                v = baseType.MakeArrayType(rank);
                return true;
            }
            else
            {
                v = baseType;
                return true;
            }
        }



        /// <summary>
        /// 读取字段成员
        /// 解析对象(Type){ var=value;...}
        /// </summary>
        bool TryParseObject(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseObject");
#endif
            v = null;
            if (Value.type == TokenType.VARIABLE)
            {
                type = AssemblyUtil.GetType(Value.value.ToString());
                NextSkipComment();
            }

            if (Value.type != (TokenType.LEFT_BRACE)) return false;
            var obj = AssemblyUtil.CreateInstance(type);

            NextSkipComment();
            while (Value.type != (TokenType.RIGHT_BRACE))
            {
                if (Value.type != format.ExpStart) return false;
                var expName = Value.value.ToString();
                NextSkipComment();
                if (Value.type != format.ExpSeparator) return false;
                NextSkipComment();
                object o = null;
                var mi = AssemblyUtil.GetMemberInfo(type, expName);
                if (mi != null)
                {
                    if(Value.type== TokenType.KEYWORD && Value.value.ToString() == "ref")
                    {
                        NextSkipComment();
                        object ref_hashId = null;
                        if (TryParseLogic(TryParseHashId, type, out ref_hashId))
                        {
                            RefObject((int)ref_hashId, (xx) => AssemblyUtil.SetMemberValue(obj, expName, xx));
                        }
                        else return false;
                    }
                    else if (!TryParseLogic(TryParseValue, mi.GetMemberType(), out o))
                    {
                        return false;
                    }
                    else
                    {
                        AssemblyUtil.SetMemberValue(obj, expName, o);
                    }
                }
                else//跳过没有定义的字段,的值的解析
                {
                    int layer = 0;
                    while (layer != 0 || Value.type != format.ExpEnd)
                    {
                        switch (Value.type)
                        {
                            case TokenType.LEFT_ANGLE:
                            case TokenType.LEFT_BRACE:
                            case TokenType.LEFT_BRACKET:
                            case TokenType.LEFT_PARENTHESIS:
                                layer++;
                                break;
                            case TokenType.RIGHT_ANGLE:
                            case TokenType.RIGHT_BRACE:
                            case TokenType.RIGHT_BRACKET:
                            case TokenType.RIGHT_PARENTHESIS:
                                layer--;
                                break;
                        }
                        NextSkipComment();
                    }
                }
                if (Value.type != format.ExpEnd) return false;
                NextSkipComment();
            }

            NextSkipComment();
            OnDeserialize(obj as ICqSerialize);
            v = obj;

            return true;
        }
        void OnDeserialize(ICqSerialize obj)
        {
            if (obj != null)
            {
#if Print_Parser
            CqCore.CqDebug.Log("OnDeserialize");
#endif
                obj.OnDeserialize();
            }
        }
        /*
        //特殊处理原版list数据格式形如[2][{...},{...}]去掉[2]
        bool TryParseOldListHead(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseOldListHead");
#endif
            v = null;
            if (Value.type != (TokenType.LEFT_BRACKET)) return false;
            NextSkipComment();
            if (Value.type != (TokenType.NUMBER)) return false;
            NextSkipComment();
            if (Value.type != (TokenType.RIGHT_BRACKET)) return false;
            NextSkipComment();
            if (Value.type != (TokenType.LEFT_BRACKET)) return false;
            return true;
        }
        */

        /// <summary>
        /// 解析List(listType&lt;itemType&gt;)[ value,...]
        /// </summary>
        bool TryParseList(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseList");
#endif
            v = null;

            Type itemType = type.GetGenericArguments()[0];

            var list = AssemblyUtil.CreateInstance<IList>(type);

            //object oo;
            //TryParseLogic(TryParseOldListHead, null, out oo);

            if (Value.type != (TokenType.LEFT_BRACKET)) return false;
            NextSkipComment();


            while (Value.type != (TokenType.RIGHT_BRACKET))
            {
                object o;
                if (!TryParseLogic(TryParseValue, itemType, out o)) return false;
                else
                {
                    if (o.GetType().IsValueType)
                    {
                        list.Add(Convert.ChangeType(o, itemType));
                    }
                    else
                    {
                        list.Add(o);
                    }
                }
                if (Value.type == TokenType.COMMA) NextSkipComment();
            }
            NextSkipComment();
            v = list;
            return true;
        }

        /// <summary>
        /// 解析数组 T()[ value,...]
        /// </summary>
        bool TryParseArray(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseArray");
#endif
            //先用List序列化,之后转数组
            v = null;
            Type elementType = null;
            switch (type.GetArrayRank())
            {
                case 1:
                    elementType = type.GetElementType();
                    break;
                case 2:
                    elementType = type.GetElementType().MakeArrayType();
                    break;
            }
            var listType = typeof(List<>).MakeGenericType(elementType);
            object listData;
            if (!TryParseLogic(TryParseList, listType, out listData)) return false;
            var count = (listData as IList).Count;
            switch (type.GetArrayRank())
            {
                case 1:
                    v = AssemblyUtil.CreateInstance(type, count);
                    (listData as IList).CopyTo(v as System.Array, 0);
                    break;
                case 2:
                    var ld = (listData as IList);
                    var len1 = (ld[0] as IList).Count;
                    v = AssemblyUtil.CreateInstance(type, count, len1) as IList;
                    for (int i = 0; i < count; i++)
                    {
                        var it = ld[i] as IList;
                        for (int j = 0; j < len1; j++)
                        {
                            (v as Array).SetValue(it[j], i, j);
                        }
                    }
                    break;
            }


            return true;
        }

        /// <summary>
        /// 解析字典(dicType&lt;keyType,valueType&gt;)
        /// { value:value,value:value...} -->
        /// </summary>
        bool TryParseDictionary(Type type, out object v)
        {
#if Print_Parser
        CqCore.CqDebug.Log("TryParseDictionary");
#endif
            v = null;
            Type keyType = type.GetGenericArguments()[0];
            Type valueType = type.GetGenericArguments()[1];

            var dic = AssemblyUtil.CreateInstance<IDictionary>(type);
#if Print_Parser
        CqCore.CqDebug.Log("dic:"+dic);
#endif
            if (Value.type != (TokenType.LEFT_BRACE)) return false;
            NextSkipComment();
            while (Value.type != (TokenType.RIGHT_BRACE))
            {
                object key = null;
                if (!TryParseLogic(TryParseValue, keyType, out key)) return false;
                if (Value.type != TokenType.COLON) return false;
                NextSkipComment();
                object vv = null;
                if (!TryParseLogic(TryParseValue, valueType, out vv)) return false;
                else
                {
#if Print_Parser
                CqCore.CqDebug.Log("key:" + key + " v:" + vv);
#endif
                    dic[key] = vv;
                }
                if (Value.type == TokenType.COMMA) NextSkipComment();
            }
            NextSkipComment();
            v = dic;
            return true;
        }

    }
}