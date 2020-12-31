using CqCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DevelopTool
{
    [MarkSerialize(SerializeTypeStyle.Property)]
    public class DD
    {
        public int aa { get; set; }
        public int bb { get; set; }

        public ABC abc { get; set; }
    }
    public class ABC
    {
        public IList a;
        public IDictionary c;
        public int b;
        public DD dd;
        public bool xx;
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>  
        [STAThread]
        static void Main(string[] args)
        {
            //var t1 = typeof(int[]);
            //var t2 = typeof(List<int>);
            //var t3 = new Dictionary<int, string>() { };
            //t3[3] = "33";
            //t3[2] = "aa";
            //var abc = new ABC() { a = new int[,] { { 3,2 }, { 33, 3 }, { 2, 33 } }, b = 4 ,xx=true,c=t3 };
            //var dd = new DD() { aa = 2, bb = 5 };
            //abc.dd = dd;
            //dd.abc = abc;
            
            //var format = new SerializeFormat(SerializeFormatStyle.Json, false, false, true);
            //var tt=CqSerialize.Serialize(abc, format);
            ////var tt = CqSerialize.Serialize(abc, SerializeFormat.Json);

            //Console.WriteLine(tt);
            //Console.ReadKey();
            //var abcX = CqSerialize.Parse<ABC>(tt, ParserFormat.Json);
            //Console.WriteLine(CqSerialize.Serialize(abcX, format));

            //Console.ReadKey();
            //return;
            //var ff= File.ReadAllText(@"d:\DevelopTool.csproj");

            //var xx=XmlUtil.XmlAnalysis( "Project",@"d:\DevelopTool.csproj");
            //var xxx = XmlUtil.XmlAnalysis("OutputPath",@"C:\Users\Administrator\Desktop\CSharp\DevelopTool\DevelopTool.csproj");
            StartLogic.InitModel();

            var totalTime = CqDebug.ExecFun(StartLogic.CMDMakeAll);
            Console.WriteLine(string.Format("总执行时间：{0}秒", totalTime.ToString("n5")));
            //Console.ReadLine();
        }
    }
}
