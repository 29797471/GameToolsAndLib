using Aspose.Cells;
using System.Collections.Generic;

namespace CqBehavior.Task
{
    [Editor("生成单词")]
    [MenuItemPath("添加/行为节点/文件操作/生成单词")]
    public class MakeWord : CqBehaviorNode
    {
        [Priority(5, 2)]
        [Button, Click("OpenFile")]
        public string Btn1 { get { return "打开文件"; } }

        public void OpenFile(object obj)
        {
            FileOpr.RunByRelativePath(SrcPath);
        }
        [Priority(9)]
        [Button, Click("SortPart")]
        public string Btn2 { get { return "词根词缀排序去重"; } }
        public void SortPart(object obj)
        {
            var ce = new Workbook(SrcPath);
            var sheet = ce.Worksheets[1];
            var group = GetPartGroup( sheet);
            group.infix = group.infix.MakeListByRemoveEqualItem();
            group.infix.Sort();

            group.prefixs = group.prefixs.MakeListByRemoveEqualItem();
            group.prefixs.Sort();

            group.suffixs = group.suffixs.MakeListByRemoveEqualItem();
            group.suffixs.Sort();
            
            for (var i = 1; i < sheet.Cells.MaxDataRow; i++)
            {
                if(i-1<group.infix.Count)
                {
                    sheet.Cells[i,0].Value=( group.infix[i-1]);
                }
                else
                {
                    sheet.Cells[i,0].Value=("");
                }

                if (i-1 < group.prefixs.Count)
                {
                    sheet.Cells[i,1].Value=(group.prefixs[i-1]);
                }
                else
                {
                    sheet.Cells[i,1].Value=("");
                }
                if (i-1 < group.suffixs.Count)
                {
                    sheet.Cells[i,2].Value=(group.suffixs[i-1]);
                }
                else
                {
                    sheet.Cells[i,2].Value=("");
                }
            }
            ce.Save(ce.AbsolutePath);
            EventMgr.MsgPrint.Notify("排序去重成功", 5);
        }
        [Priority(10)]
        [Button, Click("SpritAllWord")]
        public string Btn3 { get { return "按词根词缀拆分单词"; } }

        [Priority(11)]
        [Button, Click("MakeJson")]
        public string Btn4{ get { return "导出json"; } }

        [Priority(12)]
        [Button, Click("TestSplit")]
        public string Btn5 { get { return "测试拆分"; } }

        public void TestSplit(object obj)
        {
            var ce = new Workbook(SrcPath);
            var partGroup = GetPartGroup(ce.Worksheets[1]);
            var result=SplitWord(Content, partGroup);
            EventMgr.MsgPrint.Notify(result, 5);
        }
        public void MakeJson(object obj)
        {
            var list = new List<EnClass>();
            var ce = new Workbook(SrcPath);
            var sheet = ce.Worksheets[0];
            EnClass enClass=null;
            EnUnit enUnit=null;
            for (var i = 1; i < sheet.Cells.MaxDataRow; i++)
            {
                var title = sheet.Cells[i,0].StringValue;
                var word = sheet.Cells[i,1].StringValue;

                if(word.IsNullOrEmpty())
                {
                    if (!title.Contains("年级"))
                    {
                        enUnit = new EnUnit();
                        enUnit.name = title;
                        enUnit.words = new List<Word>();
                        enClass.enUnits.Add(enUnit);
                    }
                    else
                    {
                        enClass = new EnClass();
                        enClass.name = title;
                        enClass.enUnits = new List<EnUnit>();
                        list.Add(enClass);
                    }
                }
                else
                {
                    var parts = sheet.Cells[i,2].StringValue;
                    if (!parts.IsNullOrEmpty())
                    {
                        var w = new Word();
                        w.name = word;
                        w.parts = parts.Split(",");
                        w.cn = sheet.Cells[i,0].StringValue;
                        enUnit.words.Add(w);
                    }
                    
                }
            }
            FileOpr.SaveFile(WordPath, JsonX.Serialize(list));

            FileOpr.SaveFile(PartPath, JsonX.Serialize(GetPartGroup(ce.Worksheets[1])));
            EventMgr.MsgPrint.Notify("导出成功", 5);
        }
        PartGroup GetPartGroup(Worksheet sheet)
        {
            var partGroup = new PartGroup();
            partGroup.infix = new List<string>();
            partGroup.prefixs = new List<string>();
            partGroup.suffixs = new List<string>();
            for (int i = 1; i < sheet.Cells.MaxDataRow; i++)
            {
                var part = sheet.Cells[i,0].StringValue.Trim();
                if (!part.IsNullOrEmpty())
                {
                    partGroup.infix.Add(part);
                }
                part = sheet.Cells[i,1].StringValue.Trim();
                if (!part.IsNullOrEmpty())
                {
                    partGroup.prefixs.Add(part);
                }
                part = sheet.Cells[i,2].StringValue.Trim();
                if (!part.IsNullOrEmpty())
                {
                    partGroup.suffixs.Add(part);
                }
            }
            partGroup.infix.Sort(SortFun);
            partGroup.prefixs.Sort(SortFun);
            partGroup.suffixs.Sort(SortFun);
            int SortFun(string x, string y)
            {
                if (x.Length != y.Length)
                {
                    return x.Length.CompareTo(y.Length);
                }
                else
                {
                    return x.CompareTo(y);
                }
            }
            return partGroup;
        }
        public class PartGroup
        {
            /// <summary>
            /// 前缀
            /// </summary>
            public List<string> prefixs;

            /// <summary>
            /// 后缀
            /// </summary>
            public List<string> suffixs;

            /// <summary>
            /// 词根,中缀
            /// </summary>
            public List<string> infix;
        }
        public class EnClass
        {
            public string name;
            public List<EnUnit> enUnits;
        }
        public class EnUnit
        {
            public string name;
            public List<Word> words;
        }
        public class Word
        {
            public string name;
            public string cn;
            public string[] parts;
        }
        public void SpritAllWord(object obj)
        {
            var ce = new Workbook(SrcPath);
            var sheet = ce.Worksheets[0];

            var partGroup = GetPartGroup(ce.Worksheets[1]);
            var regEnglish = new System.Text.RegularExpressions.Regex(@"^[a-z]+$");
            for (var i = 1; i < sheet.Cells.MaxDataRow; i++)
            {
                var word = sheet.Cells[i,1].StringValue;

                if (!word.IsNullOrEmpty())
                {
                    if (regEnglish.IsMatch(word))
                    {
                        sheet.Cells[i,2].Value=(SplitWord(word,partGroup));
                    }
                    else
                    {
                        sheet.Cells[i,2].Value=("");
                    }
                }
            }
            ce.Save(ce.AbsolutePath);
            EventMgr.MsgPrint.Notify("保存成功", 5);

        }
        public string SplitWord(string word,PartGroup partGroup)
        {
            if (word.Length <= 3)
            {
                return word;
            }
            else
            {
                var format1 = SplitPrefix(ref word);
                if (word == null)
                {
                    return format1;
                }
                var format2 = SplitSuffix(ref word);
                if (word == null)
                {
                    return string.Format(format1, format2);
                }
                return string.Format(format1, string.Format(format2, SplitAffix(word)));
            }
            //拆分前缀
            string SplitPrefix(ref string short_word)
            {
                for (int i = partGroup.prefixs.Count - 1; i >= 0; i--)
                {
                    var part = partGroup.prefixs[i];
                    if (short_word.StartsWith(part))
                    {
                        if(short_word==part)
                        {
                            short_word = null;
                            return part;
                        }
                        short_word = short_word.Substring(part.Length);
                        return part+",{0}";
                    }
                }
                return "{0}";
            }
            //拆分后缀
            string SplitSuffix(ref string short_word)
            {
                for (int i = partGroup.suffixs.Count - 1; i >= 0; i--)
                {
                    var part = partGroup.suffixs[i];
                    if (short_word.EndsWith(part))
                    {
                        if (short_word == part)
                        {
                            short_word = null;
                            return part;
                        }
                        short_word = short_word.SubstringEx(0, part.Length);
                        return  "{0},"+ part;
                    }
                }
                return "{0}";
            }
            //拆分词根
            string SplitAffix(string short_word)
            {
                for (int i = partGroup.infix.Count - 1; i >= 0; i--)
                {
                    var part = partGroup.infix[i];
                    var index = short_word.IndexOf(part);
                    if (index > 0 && index + part.Length < short_word.Length)
                    {
                        if (short_word[index + part.Length] != ',')
                        {
                            short_word = short_word.Insert(index + part.Length, ",");
                        }
                        if (short_word[index - 1] != ',')
                        {
                            short_word = short_word.Insert(index, ",");
                        }
                    }
                }
                return short_word;
            }
        }

        

        [Priority(1)]
        [FilePath("文件路径", true), MinWidth(350)]
        public string SrcPath { get { return mSrcPath; } set { mSrcPath = value; Update("SrcPath"); } }
        public string mSrcPath;

        [Priority(2)]
        [FilePath("导出单词", true), MinWidth(350)]
        public string WordPath { get { return mWordPath; } set { mWordPath = value; Update("WordPath"); } }
        public string mWordPath;

        [Priority(3)]
        [FilePath("导出词根词缀", true), MinWidth(350)]
        public string PartPath { get { return mPartPath; } set { mPartPath = value; Update("PartPath"); } }
        public string mPartPath;

        [Priority(4)]
        [MinWidth(350)]
        [TextBox("测试拆分单词")]
        public string Content
        {
            get { return mContent; }
            set { mContent = value; Update("Content"); }
        }
        public string mContent;
    }
}