using CqCore;
using ParserCore;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 按类型解析
/// 1.基本数据类型
/// 2.枚举
/// 3.数组
/// 4.字典
/// 5.自定义类
/// </summary>
internal class JsonParser : TokenParser
{
    public JsonParser(List<Token> list) : base(list)
    {
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
        //解析到数字,字符串,常量,一定不是对象,列表和字典,因为它们都以符号或者变量名起始
        switch(Value.type)
        {
            case TokenType.NUMBER:
            case TokenType.STRING:
            case TokenType.CHAR:
            case TokenType.KEYWORD:
                v = ConvertUtil.ConvertType(Value.value, type);
                NextSkipComment();
                return true;
        }

        if (type.IsEnum)
        {
            v = Enum.Parse(type, Value.value.ToString());
            NextSkipComment();
            return true;
        }

        if (Value.IsNull())
        {
            v = Value.value;
            NextSkipComment();
            return true;
        }

        object parseType;
        if(TryParseLogic(TryParseType, null, out parseType))
        {
            type = (System.Type)parseType;
        }
        
        if (AssemblyUtil.IsList(type))
        {
            return TryParseLogic(TryParseList, type, out v);
        }
        else if(AssemblyUtil.IsArray(type))
        {
            return TryParseLogic(TryParseArray, type, out v);
        }
        else  if (AssemblyUtil.IsDictionary(type))
        {
            return TryParseLogic(TryParseDictionary, type, out v);
        }
        else
        {
            return TryParseLogic(TryParseObject, type, out v);
        }

    }

    /// <summary>
    /// 解析类型名,包含泛型,以及泛型嵌套类型<para/>
    /// 1.  List&lt;T&gt;<para/>
    /// 2. T() 数组<para/>
    /// 3.  Dictionary&lt;Tkey,Tvalue&gt;<para/>
    /// 4. T
    /// </summary>
    bool TryParseType(Type type, out object v)
    {
        v = null;
        if (Value.type != TokenType.VARIABLE) return false;

        var baseType= AssemblyUtil.GetType(Value.value.ToString());
        NextSkipComment();

        if (Value.type == (TokenType.LEFT_ANGLE))
        {
            NextSkipComment();
            List<Type> types = new List<Type>();
            while (Value.type != (TokenType.RIGHT_ANGLE))
            {
                object o = null;
                if (!TryParseLogic(TryParseType, null, out o)) return false;
                types.Add((Type)o );
                if (Value.type == TokenType.COMMA) NextSkipComment();
            }
            NextSkipComment();
            switch(types.Count)
            {
                case 1:
                    baseType=baseType.MakeGenericType(types[0]);
                    break;
                case 2:
                    baseType= baseType.MakeGenericType(types[0],types[1]);
                    break;
                default:
                    baseType= baseType.MakeGenericType(types.ToArray());
                    break;
            }
        }

        if (Value.type == (TokenType.LEFT_PARENTHESIS))
        {
            NextSkipComment();
            if (Value.type != (TokenType.RIGHT_PARENTHESIS)) return false;//throw new Exception("未找到匹配的右小括号");
            NextSkipComment();
            v= baseType.MakeArrayType();
            return true;
        }
        else
        {
            v= baseType;
            return true;
        }
    }

    /// <summary>
    /// 读取属性成员
    /// 解析对象(Type){ var=value;...}
    /// </summary>
    bool TryParseObject(Type type, out object v)
    {
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
            if (Value.type != TokenType.STRING) return false;
            var expName = Value.value.ToString();
            NextSkipComment();
            if (Value.type != TokenType.COLON) return false;
            NextSkipComment();
            object o = null;
            var mi = AssemblyUtil.GetMemberInfo(type, expName);
            if (mi!=null)
            {
                if (!TryParseLogic(TryParseValue, AssemblyUtil.GetMemberType(mi), out o)) return false;
                else
                {
                    AssemblyUtil.SetMemberValue(obj, expName, o);
                }
            }
            else//跳过没有定义的字段,的值的解析
            {
                int layer = 0;
                while (layer > 0 || Value.type != TokenType.COMMA)
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
                    if (layer < 0) break;
                    NextSkipComment();
                }
            }
            if (Value.type == TokenType.COMMA) NextSkipComment();
        }

        NextSkipComment();
        v = obj;
        return true;
    }

    //特殊处理原版list数据格式形如[2][{...},{...}]去掉[2]
    bool TryParseOldListHead(Type type, out object v)
    {
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

    /// <summary>
    /// 解析List(listType&lt;itemType&gt;)[ value,...]
    /// </summary>
    bool TryParseList(Type type, out object v)
    {
        v = null;

        Type itemType = type.GetGenericArguments()[0];

        var list = AssemblyUtil.CreateInstance<IList>(type);

        object oo;
        TryParseLogic(TryParseOldListHead, null, out oo);

        if (Value.type != (TokenType.LEFT_BRACKET)) return false;
        NextSkipComment();
        

        while (Value.type != (TokenType.RIGHT_BRACKET))
        {
            object o;
            if (!TryParseLogic(TryParseValue, itemType, out o)) return false;
            else
            {
                if(o.GetType().IsValueType)
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
        //先用List序列化,之后转数组
        v = null;
        var listType=typeof(List<>).MakeGenericType(type.GetElementType());
        object listData;
        if (!TryParseLogic(TryParseList,listType, out listData)) return false;
        var count = (listData as IList).Count;
        v = AssemblyUtil.CreateInstance(type, count);
        (listData as IList).CopyTo(v as System.Array, 0);
        return true;
    }

    /// <summary>
    /// 解析字典(dicType&lt;keyType,valueType&gt;)
    /// { value:value,value:value...} -->
    /// </summary>
    bool TryParseDictionary(Type type, out object v)
    {
        v = null;
        Type keyType = type.GetGenericArguments()[0];
        Type valueType = type.GetGenericArguments()[1];
        
        var dic = AssemblyUtil.CreateInstance<IDictionary>(type);

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
            else dic[key] = vv;
            if (Value.type == TokenType.COMMA) NextSkipComment();
        }
        NextSkipComment();
        v = dic;
        return true;
    }

}