using System;

namespace ParserCore
{
    [Flags]
    public enum TokenType
    {
        /// <summary>
        /// 分号
        /// </summary>
        [TokenEnumElement("分号",";")]
        SEMICOLON = 1 << 1,

        /// <summary>
        /// 左大括号
        /// </summary>
        [TokenEnumElement("左大括号","{")]
        LEFT_BRACE = 1 << 2,


        /// <summary>
        /// 右大括号
        /// </summary>
        [TokenEnumElement("右大括号","}")]
        RIGHT_BRACE = 1 << 3,

        /// <summary>
        /// 左中括号
        /// </summary>
        [TokenEnumElement("左中括号","[")]
        LEFT_BRACKET = 1 << 4,

        /// <summary>
        /// 右中括号
        /// </summary>
        [TokenEnumElement("右中括号","]")]
        RIGHT_BRACKET = 1 << 5,

        /// <summary>
        /// 左小括号
        /// </summary>
        [TokenEnumElement("左小括号","(")]
        LEFT_PARENTHESIS = 1 << 6,

        /// <summary>
        /// 右小括号
        /// </summary>
        [TokenEnumElement("右小括号",")")]
        RIGHT_PARENTHESIS = 1 << 7,

        /// <summary>
        /// 左尖括号
        /// </summary>
        [TokenEnumElement("左尖括号","<")]
        LEFT_ANGLE = 1 << 8,

        /// <summary>
        /// 右尖括号
        /// </summary>
        [TokenEnumElement("右尖括号",">")]
        RIGHT_ANGLE = 1 << 9,

        /// <summary>
        /// 等号
        /// </summary>
        [TokenEnumElement("等号","=")]
        EQUAL = 1 << 10,

        /// <summary>
        /// 冒号
        /// </summary>
        [TokenEnumElement("冒号",":")]
        COLON = 1 << 11,

        /// <summary>
        /// 百分号
        /// </summary>
        [TokenEnumElement("百分号","%")]
        PERCENT = 1 << 12,

#if 正斜杠不单独存在识别作为一个注释或者字符串内容
        /// <summary>
        /// 正斜杠 /
        /// </summary>
        [EnumLabel("正斜杠")]
        SLASH = 1 << 13,
#endif
        /// <summary>
        /// 反斜杠 
        /// </summary>
        [TokenEnumElement("反斜杠",@"\")] 
        BACKSLASH = 1 << 14,

        /// <summary>
        /// 点
        /// </summary>
        [TokenEnumElement("点",".")]
        DOT = 1 << 16,

        /// <summary>
        /// 注释
        /// </summary>
        [TokenEnumElement("注释")]
        COMMENT = 1 << 17,
        /// <summary>
        /// "数字和小数点和负号组成";
        /// </summary>
        [TokenEnumElement("数字和小数点和负号组成")]
        NUMBER = 1 << 18,

        /// <summary>
        /// 双引号包含的字符串
        /// </summary>
        [TokenEnumElement("双引号包含的字符串")]
        STRING = 1 << 19,

        /// <summary>
        /// "变量名";//由英文字母开头,数字、26个大小写英文字母或者下划线或者小数点,组成的变量名
        /// </summary>
        [TokenEnumElement("变量名")]
        VARIABLE = 1 << 20,
        /// <summary>
        /// 未知
        /// </summary>
        [TokenEnumElement("未知")]
        UNKNOWN = 1 << 21,

        /// <summary>
        /// 关键字<para/>
        /// null,true,false
        /// </summary>
        [TokenEnumElement("关键字")]
        KEYWORD = 1 << 22,
        
        /// <summary>
        /// 文件末尾
        /// </summary>
        [TokenEnumElement("文件末尾")]
        END = 1 << 25,
        /// <summary>
        /// 忽略的字符
        /// </summary>
        [TokenEnumElement("忽略的字符"," \n\r\t")]
        IGNORE =1<<26,
        /// <summary>
        /// 字符
        /// </summary>
        [TokenEnumElement("单引号包含的字符")]
        CHAR = 1 << 27,
        /// <summary>
        /// 逗号
        /// </summary>
        [TokenEnumElement("逗号",",")]
        COMMA = 1 << 28,
        /*
        //[TokenEnumElement("运算符",'+','-','*')]
        /// <summary>
        /// 运算符(+-/*)除法和注释有冲突还未处理
        /// </summary>
        //OPERATOR = 1 << 29,
        */
    }

}
