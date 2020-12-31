using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static partial class CqSerialize
{
    internal class Serializer
    {
        /// <summary>
        /// 已经序列化过的单位
        /// </summary>
        Dictionary<object,int> serializeDic;

        SerializeFormat format;
        StringWriter sw;
        /// <summary>
        /// 当前深度
        /// </summary>
        int currentDepth;

        /// <summary>
        /// 当是class修饰的非基础数据类型时,该类型的实例可以被重复引用.<para/>
        /// 所以当开启排除递归后,在序列化这个对象时需要记录hashId
        /// </summary>
        public static bool IsRecordingType(Type type)
        {
            return !type.IsFundamental() && type.IsClass;
        }

        public Serializer(SerializeFormat format)
        {
            this.format = format;
            
            if(format.withHashId)
            {
                serializeDic = new Dictionary<object, int>();
            }
            sw = new StringWriter();
            
        }
        public string GetString()
        {
            return sw.ToString();
        }
        void WriteTbl()
        {
            sw.Write(format.GetTbls(currentDepth));
        }
        void WriteLine()
        {
            sw.Write(format.Enter);
        }

        /// <summary>
        /// 序列化任意一个对象
        /// </summary>
        /// <param name="obj">准备序列化的对象</param>
        /// <param name="inputType">输入类型</param>
        /// <param name="inExpression">是否是表达式中的值,用于控制输出格式</param>
        public void SerializeValue(object obj, Type inputType, bool inExpression = false)
        {
            if (obj == null)
            {
                sw.Write("null");
                return;
            }

            var type = obj.GetType();

            
            if(type!=inputType)
            {
                //WriteTbl();
                //当inputType是接口,或者基类时,需要序列化出类名,才能反序列化回来
                SerializeType(type);
                //WriteLine();
            }

            //当不确定对象是否有被其它对象引用时给对象添加id导出
            if (format.withHashId && IsRecordingType(type))
            {
                if (!serializeDic.ContainsKey(obj))
                {
                    serializeDic[obj] = obj.GetHashCode();
                }
                sw.Write(string.Format("({0})", obj.GetHashCode()));
            }

            if (type.IsEnum) // Enum
            {
                sw.Write(obj.ToString());
            }
            else if (AssemblyUtil.IsArray(type))
            {
                if (inExpression) WriteLine();
                SerializeArray(obj as Array, inputType);
            }
            else if (type.IsGenericType)//是一个泛型
            {
                if (AssemblyUtil.IsList(type))
                {
                    if (inExpression) WriteLine();
                    SerializeList(obj as IList, inputType);
                }
                else if (AssemblyUtil.IsDictionary(type))//Dic<string,object>
                {
                    if (inExpression) WriteLine();
                    SerializeDic(obj as IDictionary, inputType);
                }
                else
                {
                    if (inExpression) WriteLine();
                    SerializeObj(obj, inputType);
                }
            }
            else
            {
                switch (AssemblyUtil.GetName(type))
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
                            switch (chr)
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
                            sw.Write(((DateTime)obj).Ticks);
                        }
                        break;
                    default:
                        {
                            if (inExpression) WriteLine();
                            SerializeObj(obj, inputType);
                        }
                        break;
                }
            }

            
        }
        void SerializeObj(object obj, Type inputType)
        {
            var type = obj.GetType();

            //当定义类型和实例不匹配时要序列化类型
            if (inputType != type)
            {
                WriteTbl();
                sw.Write(AssemblyUtil.GetName(type));
                WriteLine();
            }
            WriteTbl();
            sw.Write("{");
            if (currentDepth < format.depthMax)
            {
                WriteLine();
                currentDepth++;

                var style = SerializeTypeUtil.GetStyle(type,format.serializeObjByProperty);

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
                    sw.Write(format.ExpStart);
                    sw.Write(member.Name);
                    sw.Write(format.ExpStart);
                    sw.Write(format.ExpSeparator);

                    //跳过已经被序列化的对象
                    if (format.withHashId && IsRecordingType(dataType) && serializeDic.ContainsKey(data))
                    {
                        sw.Write(string.Format("ref({0})", serializeDic[data]));
                    }
                    else
                    {
                        SerializeValue(data, AssemblyUtil.GetMemberType(member), true);
                    }
                    
                    sw.Write(format.ExpEnd);
                    WriteLine();
                }
                currentDepth--;
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

                for (int i = 0; i < args.Length - 1; i++)
                {
                    SerializeType(args[i]);
                    sw.Write(",");
                }
                SerializeType(args[args.Length - 1]);
                sw.Write(">");
            }
            else if(type.IsArray)
            {
                sw.Write(AssemblyUtil.GetName(type.GetElementType())+"["+','.Repeat(type.GetArrayRank() - 1)+"]");
            }
            else
            {
                sw.Write(AssemblyUtil.GetName(type));
            }
        }
        void SerializeList(IList obj, Type inputType)
        {
            var type = obj.GetType();
            Type itemType = type.GetGenericArguments()[0];
            
            WriteTbl();
            sw.Write('[');
            if (currentDepth < format.depthMax)
            {
                WriteLine();
                int count = obj.Count;
                int i = 0;

                currentDepth++;
                foreach (var item in obj)
                {
                    //sw.Write(string.Format("\t{0}", tab));
                    SerializeValue(item, itemType);
                    if (i != count - 1)
                    {
                        sw.Write(',');
                        WriteLine();
                    } 
                    else WriteLine();
                    i++;
                }
                currentDepth--;
                WriteTbl();
            }
            sw.Write(']');
        }
        void SerializeArray(Array obj, Type inputType)
        {
            var type = obj.GetType();
            Type itemType = type.GetElementType();

            WriteTbl();
            sw.Write('[');
            if (currentDepth < format.depthMax)
            {
                WriteLine();
                switch (obj.Rank)
                {
                    case 1:
                        {
                            int count = obj.Length;
                            int i = 0;
                            currentDepth++;
                            foreach (var item in obj)
                            {
                                //sw.Write(string.Format("\t{0}", tab));
                                SerializeValue(item, itemType);
                                if (i != count - 1)
                                { 
                                    sw.Write(',');
                                    WriteLine();
                                } 
                                else WriteLine();
                                i++;
                            }
                            currentDepth--;
                        }
                        break;
                    case 2:
                        {
                            var len0 = obj.GetLength(0);
                            var len1 = obj.GetLength(1);
                            int i = 0, j = 0;
                            currentDepth++;
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
                                    WriteLine();
                                }
                                else
                                {
                                    sw.Write(',');
                                }
                            }
                            currentDepth--;
                        }
                        break;
                }
                WriteTbl();
            }
            sw.Write(']');
        }
        void SerializeDic(IDictionary o, Type inputType)
        {
            var type = o.GetType();
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            
            WriteTbl();
            sw.Write('{');
            if (currentDepth < format.depthMax)
            {
                WriteLine();
                int i = 0;
                currentDepth++;
                foreach (var key in o.Keys)
                {
                    var data = o[key];
                    //WriteTbl();
                    SerializeValue(key, keyType);
                    sw.Write(":");
                    SerializeValue(data, valueType, true);
                    if (i != o.Keys.Count - 1)
                    { 
                        sw.Write(",");
                        WriteLine();
                    } 
                    else WriteLine();
                    i++;
                }
                currentDepth--;
                WriteTbl();
            }
            sw.Write("}");
        }
        
    }
}
