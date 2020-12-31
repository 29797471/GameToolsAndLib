
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Proto
{
    /// <summary>
    /// 形如:syntax = "proto2";
    /// </summary>
    [EditorAttribute]       
    public class ProtoSyntax : BaseTreeNotifyObject
    {
        [PriorityAttribute(1)]
        [TextBox("Proto版本")]
        public string Syntax { get { return mSyntax; } set { mSyntax = value; Update("Syntax"); } }
        public string mSyntax;


    }
}

