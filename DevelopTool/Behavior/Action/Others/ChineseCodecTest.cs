using Business;
using CqCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqBehavior.Task
{
    [Editor("中文编解码测试")]
    [MenuItemPath("添加/其他/中文编解码测试")]
    public class ChineseCodecTest : CqBehaviorNode
    {
        [MinWidth(350), MinHeight(100)]
        [TextBox("内容")]
        [Priority(1)]
        public string Content
        {
            get { return mContent; }
            set
            {
                mContent = value;
                Update("Content");

                ResultStr = StringUtil.TransferEncoding(SrcEncoding.GetEncoding(), DstEncoding.GetEncoding(), value);
            }
        }
        public string mContent;

        [MinWidth(350)]
        [Priority(2)]
        [ComboBox("源编码"), SelectedValue("SrcEncoding"),DisplayMember("DisplayName"), Width(100)]
        public IOrderedEnumerable<EncodingInfo> TypeList
        {
            get
            {
                var list = Encoding.GetEncodings().ToList();
                var g=from it in list orderby it.Name select it;
                return g;
            }
        }

        public EncodingInfo SrcEncoding
        {
            get { return mSrcEncoding; }
            set { mSrcEncoding = value; Update("SrcEncoding"); }
        }
        EncodingInfo mSrcEncoding;



        [MinWidth(350)]
        [Priority(3)]
        [ComboBox("目标编码"), SelectedValue("DstEncoding"), DisplayMember("DisplayName"), Width(100)]
        public IOrderedEnumerable<EncodingInfo> TypeListx
        {
            get
            {
                var list = Encoding.GetEncodings().ToList();
                var g = from it in list orderby it.Name select it;
                return g;
            }
        }
        public EncodingInfo DstEncoding
        {
            get { return mDstEncoding; }
            set { mDstEncoding = value; Update("DstEncoding"); }
        }
        EncodingInfo mDstEncoding;

        [MinWidth(350), MinHeight(100)]
        [TextBox("结果")]
        [Priority(4)]
        public string ResultStr { get { return mResultStr; } set { mResultStr = value; Update("ResultStr"); } }
        public string mResultStr;

    }
}

