using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public static class FontUtil
    {
        static Font font;
        static Dictionary<char, int> dic;
        public static float tabLength = 100;
        public static int fontSize = 20;

        /// <summary>
        /// 给文本填充\t,保证总内容宽度相当于num个制表符
        /// </summary>
        public static string FillTbl(string str,int tabNum)
        {
            var len=GetContentLen(str);
            var count = Mathf.FloorToInt(len  / tabLength);
            return str + '\t'.Repeat(Mathf.Max(tabNum-count, 0));
        }

        /// <summary>
        /// 计算文本内容宽度
        /// </summary>
        public static int GetContentLen(string str)
        {
            if (font == null) font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            if (dic == null) dic = new Dictionary<char, int>();
            Action GetChrLength = null;
            for (int i = 0; i < str.Length; i++)
            {
                var chr = str[i];
                if (!dic.ContainsKey(chr))
                {
                    GetChrLength += () =>
                    {
                        CharacterInfo ch;
                        font.GetCharacterInfo(chr, out ch);
                        dic[chr] = ch.advance;
                    };
                }
            }
            if (GetChrLength != null)
            {
                font.RequestCharactersInTexture(str);
                GetChrLength();
            }

            int len = 0;
            for (int i = 0; i < str.Length; i++)
            {
                len += dic[str[i]];
            }
            return len;
        }
    }
}
