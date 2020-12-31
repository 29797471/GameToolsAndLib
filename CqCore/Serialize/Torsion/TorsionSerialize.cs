
using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

internal class TorsionSerialize
{
    /// <summary>
    ///  排除递归序列化(同一个自定义对象不会被重复序列化)
    /// </summary>
    public bool excludeRecursive;
    List<object> hasSerializeList;

    /// <summary>
    /// 检查是不是一个需要保证不重复序列化的类型
    /// </summary>
    public static bool IsRecordingType(Type type)
    {
        return !type.IsFundamental() && type.IsClass;
    }

    StringWriter sw = new StringWriter();

    /// <summary>
    /// 当前深度
    /// </summary>
    int tblNum;

    /// <summary>
    /// 序列化深度
    /// </summary>
    int depth;

    /// <summary>
    /// 序列化生成器
    /// </summary>
    /// <param name="excludeRecursive">排除递归序列化(同一个自定义对象不会被重复序列化)</param>
    /// <param name="depth">序列化深度</param>
    public TorsionSerialize(bool excludeRecursive=false, int depth = int.MaxValue)
    {
        this.depth = depth;
        this.excludeRecursive = excludeRecursive;
        if(excludeRecursive)
        {
            hasSerializeList = new List<object>();
        }
    }
    public string GetString()
    {
        return sw.ToString();
    }
    
    /// <summary>
    /// 序列化任意一个对象
    /// </summary>
    /// <param name="obj">准备序列化的对象</param>
    /// <param name="inputType">输入类型</param>
    /// <param name="inExpression">是否是表达式中的值,用于控制输出格式</param>
    public void SerializeValue(object obj,  Type inputType,  bool inExpression=false)
    {
        if (obj == null)
        {
            sw.Write("null");
            return;
        }

        var type = obj.GetType();
        if(excludeRecursive)
        {
            if (IsRecordingType(type))
            {
                if (!hasSerializeList.Contains(obj))
                {
                    hasSerializeList.Add(obj);
                }
            }
        }
        if (type.IsEnum) // Enum
        {
            sw.Write(obj.ToString());
        }
        else if (AssemblyUtil.IsArray(type))
        {
            if (inExpression) sw.WriteLine();
            SerializeArray(obj as Array, inputType);
        }
        else if (type.IsGenericType)//是一个泛型
        {
            if (AssemblyUtil.IsList(type))
            {
                if (inExpression) sw.WriteLine();
                SerializeList(obj as IList, inputType);
            }
            else if (AssemblyUtil.IsDictionary(type))//Dic<string,object>
            {
                if (inExpression) sw.WriteLine();
                SerializeDic(obj as IDictionary,  inputType);
            }
            else
            {
                if (inExpression) sw.WriteLine();
                SerializeObj(obj, inputType);
            }
        }
        else
        {
            switch(AssemblyUtil.GetName(type))
            {
                case "string":
                    {
                        if (!inExpression) WriteTbl();
                        var str = obj.ToString();
                        str = str.Replace(@"\", @"\\");
                        str = str.Replace("\"", "\\\"");

                        sw.Write(string.Format("\"{0}\"", str));
                    }
                    break;
                case "char":
                    {
                        if (!inExpression) WriteTbl();
                        var chr = (char)obj;
                        switch(chr)
                        {
                            case '\\':
                            case '\'':
                                sw.Write(string.Format("\'\\{0}\'", chr));
                                break;
                            default:
                                sw.Write(string.Format("\'{0}\'", chr));
                                break;
                        }
                    }
                    break;
                case "long":
                case "ulong":
                case "int":
                case "uint":
                case "bool":
                    {
                        if (!inExpression) WriteTbl();
                        sw.Write(obj.ToString().ToLower());
                    }
                    break;
                case "float"://由于 float转string 会生成形如(-1.3E+03)的文本 ,需要特殊处理
                    {
                        if (!inExpression) WriteTbl();
                        //isNaN
                        if (float.IsNaN((float)obj))
                        {
                            sw.Write("NaN");
                        }
                        else
                        {
                            sw.Write(Convert.ToSingle(obj).ToString().ToLower());
                        }
                        break;
                    }
                case "double":
                    {
                        if (!inExpression) WriteTbl();
                        //isNaN
                        if (double.IsNaN((double)obj))
                        {
                            sw.Write("NaN");
                        }
                        else
                        {
                            sw.Write(Convert.ToDecimal(obj).ToString().ToLower());
                        }
                        break;
                    }
                case "System.DateTime":
                    {
                        if (!inExpression) WriteTbl();
                        sw.Write(((DateTime)obj ).Ticks);
                    }
                    break;
                default:
                    {
                        if (inExpression) sw.WriteLine();
                        SerializeObj(obj,  inputType);
                    }
                    break;
            }
        }
    }
    void SerializeObj(object obj, Type inputType)
    {
        var type = obj.GetType();
        
        //当定义类型和实例不匹配时要序列化类型
        if(inputType!=type)
        {
            WriteTbl();
            sw.WriteLine( AssemblyUtil.GetName(type));
        }
        WriteTbl();
        sw.Write("{");
        if(tblNum<depth)
        {
            sw.WriteLine();
            tblNum++;

            var style = SerializeTypeUtil.GetStyle(type);

            var members = AssemblyUtil.GetMembers(type, style);
            foreach (var member in members)
            {
                var data = member.GetValue(obj);

                //属性不同时可读可写是不会作序列化
                if (member is System.Reflection.PropertyInfo)
                {
                    var pi = (System.Reflection.PropertyInfo)member;
                    if (!pi.CanRead || !pi.CanWrite) continue;
                }
                #region 基础类型的默认值不序列化
                if (data == null || data is Delegate) continue;

                var dataType = data.GetType();
                if (excludeRecursive)
                {
                    //跳过已经被序列化的对象
                    if (IsRecordingType(dataType) && hasSerializeList.Contains(data))
                    {
                        continue;
                    }
                }
                if (dataType.IsEnum) // Enum
                {

                }
                else if (AssemblyUtil.IsList(dataType))
                {
                }
                else if (AssemblyUtil.IsArray(dataType))
                {
                }
                else if (AssemblyUtil.IsDictionary(dataType))
                {
                }
                else
                {
                    if (AssemblyUtil.IsRegType(dataType))
                    {
                        //跳过默认值的序列化
                        switch (AssemblyUtil.GetName(dataType))
                        {
                            case "int":
                                {
                                    if (data.ToString() == "0") continue;
                                }
                                break;
                        }
                    }
                }
                #endregion
                WriteTbl();
                sw.Write(string.Format("{0} = ", member.Name));
                SerializeValue(data, AssemblyUtil.GetMemberType(member), true);
                sw.WriteLine(";");
            }
            tblNum--;
            WriteTbl();
        }
        sw.Write("}");
    }
    void SerializeType(Type type)
    {
        if (type.IsGenericType)
        {
            sw.Write(AssemblyUtil.GetName(type.GetGenericTypeDefinition()));
            var args = type.GetGenericArguments();
            sw.Write("<");
            
            for(int i = 0;i<args.Length-1;i++ )
            {
                SerializeType(args[i]);
                sw.Write(",");
            }
            SerializeType(args[args.Length-1]);
            sw.Write(">");
        }
        else
        {
            sw.Write(AssemblyUtil.GetName(type));
        }
    }
    void SerializeList(IList obj,  Type inputType)
    {
        var type = obj.GetType();
        Type itemType = type.GetGenericArguments()[0];
        if (type!= inputType)//当inputType是接口,或者基类时,需要序列化出类名,才能反序列化回来
        {
            WriteTbl();
            sw.WriteLine();
            SerializeType(type);
        }
        WriteTbl();
        sw.Write('[');
        if (tblNum < depth)
        {
            sw.WriteLine();
            int count = obj.Count;
            int i = 0;

            tblNum++;
            foreach (var item in obj)
            {
                //sw.Write(string.Format("\t{0}", tab));
                SerializeValue(item, itemType);
                if (i != count - 1) sw.WriteLine(',');
                else sw.WriteLine();
                i++;
            }
            tblNum--;
            WriteTbl();
        }
        sw.Write(']');
    }
    void SerializeArray(Array obj, Type inputType)
    {
        var type = obj.GetType();
        Type itemType = type.GetElementType();

        if (type != inputType)
        {
            WriteTbl();
            sw.WriteLine( AssemblyUtil.GetName(itemType)+"()" );
        }
        WriteTbl();
        sw.Write('[');
        if (tblNum < depth)
        {
            sw.WriteLine();
            switch (obj.Rank)
            {
                case 1:
                    {
                        int count = obj.Length;
                        int i = 0;
                        tblNum++;
                        foreach (var item in obj)
                        {
                            //sw.Write(string.Format("\t{0}", tab));
                            SerializeValue(item, itemType);
                            if (i != count - 1) sw.WriteLine(',');
                            else sw.WriteLine();
                            i++;
                        }
                        tblNum--;
                    }
                    break;
                case 2:
                    {
                        var len0 = obj.GetLength(0);
                        var len1 = obj.GetLength(1);
                        int i = 0, j = 0;
                        tblNum++;
                        foreach (var item in obj)
                        {
                            if (j == 0)
                            {
                                WriteTbl();
                                sw.Write("[");
                            }
                            SerializeValue(item, itemType);
                            j++;
                            if (j == len1)
                            {
                                j = 0; i++;
                                sw.Write("]");
                                if (i != len0) sw.Write(",");
                                sw.WriteLine();
                            }
                            else
                            {
                                sw.Write(',');
                            }
                        }
                        tblNum--;
                    }
                    break;
            }
            WriteTbl();
        }
        sw.Write( ']');
    }
    void SerializeDic(IDictionary o,  Type inputType)
    {
        var type = o.GetType();
        var keyType = type.GetGenericArguments()[0];
        var valueType = type.GetGenericArguments()[1];
        if (inputType!=type)
        {
            WriteTbl();
            sw.WriteLine();
            SerializeType(type);
        }
        WriteTbl();
        sw.Write('{');
        if (tblNum < depth)
        {
            sw.WriteLine();
            int i = 0;
            tblNum++;
            foreach (var key in o.Keys)
            {
                var data = o[key];
                //WriteTbl();
                SerializeValue(key, keyType);
                sw.Write(":");
                SerializeValue(data, valueType, true);
                if (i != o.Keys.Count - 1) sw.WriteLine(",");
                else sw.WriteLine();
                i++;
            }
            tblNum--;
            WriteTbl();
        }
        sw.Write( "}");
    }
    void WriteTbl()
    {
        sw.Write(GetTbls.Call(tblNum));
    }
    /// <summary>
    /// 获取多个制表符
    /// </summary>
    static Causality<int, string> GetTbls = new Causality<int, string>((tblNum) => '\t'.Repeat(tblNum));
}