using System;

namespace CqCore
{
    /// <summary>
    /// 事件打印
    /// </summary>
    
    public class CqEventAttribute : ObjectAttribute
    {
        public string name;
        public bool print;
        public CqEventAttribute(string name,bool print=false)
        {
            this.name = name;
            this.print = print;
        }
        public void Print()
        {
            if (print && Target!=null ) CqDebug.Log(string.Format("{0}({1})\n{2}", name, Target.ToString(), Torsion.Serialize(Target)));
        }
    }

}
