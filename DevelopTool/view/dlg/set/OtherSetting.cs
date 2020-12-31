using System.Collections.ObjectModel;
using System.Windows.Media;

namespace DevelopTool
{
    [Priority(100)]
    [Editor("其它")]
    public class OtherSetting : Setting
    {

        [FilePath("文件对比工具", true, "执行程序|*.exe"), Priority(12), MinWidth(100)]
        public string ComparePath { get { return mComparePath; } set { mComparePath = value; Update("ComparePath"); } }
        public string mComparePath;

        [Priority(3)]
        [TextBox("泛型List模板"),MinWidth(200)]
        public string List
        {
            get { return mList; }
            set { mList = value; Update("List"); }
        }
        public string mList;
        #region 代码编辑器

        [Priority(4)]
        [ColorBox("代码编辑中的文字颜色")]
        public Color Foreground
        {
            get
            {
                return WinUtil.ToMediaColor(mForeground);
            }
            set
            {
                mForeground = value.ToString();
                Update("Foreground");
            }
        }
        public string mForeground = "#FF80FF";

        [Priority(5)]
        [ColorBox("代码编辑中的背景颜色")]
        public Color Background
        {
            get
            {
                return WinUtil.ToMediaColor(mBackground);
            }
            set
            {
                mBackground = value.ToString();
                Update("Background");
            }
        }
        public string mBackground = "#000000";

        [Priority(6)]
        [TextBox("字体大小"), MinWidth(100)]
        public int FontSize
        {
            get { return mFontSize; }
            set { mFontSize = value; Update("FontSize"); }
        }
        public int mFontSize=10;
        #endregion

        #region 动画编辑

        public ObservableCollection<KeyValue> mTipIconsList;

        [Priority(7)]
        [ListView("对齐样式-转译名称"),MinHeight(80)]
        public ObservableCollection<KeyValue> TipIconsList
        {
            get
            {
                if (mTipIconsList == null)
                {
                    mTipIconsList = new ObservableCollection<KeyValue>();
                }
                return mTipIconsList;
            }
            set { mTipIconsList = value; }
        }

        public ObservableCollection<KeyValue> mHorizontalDot;

        [Priority(8)]
        [ListView("水平描点->转译名称"), MinHeight(80)]
        public ObservableCollection<KeyValue> HorizontalDot
        {
            get
            {
                if (mHorizontalDot == null)
                {
                    mHorizontalDot = new ObservableCollection<KeyValue>();
                }
                return mHorizontalDot;
            }
            set { mHorizontalDot = value; }
        }

        public ObservableCollection<KeyValue> mVerticalDot;
        [ListView("垂直描点->转译名称"), MinHeight(80)]
        [Priority(9)]
        public ObservableCollection<KeyValue> VerticalDot
        {
            get
            {
                if (mVerticalDot == null)
                {
                    mVerticalDot = new ObservableCollection<KeyValue>();
                }
                return mVerticalDot;
            }
            set { mVerticalDot = value; }
        }

        #endregion

        [Priority(10)]
        [FolderPath("股票基金数据存放目录", true), MinWidth(100)]
        public string DataPath { get { return dataPath; } set { dataPath = value; Update("DataPath"); } }
        public string dataPath;
    }
}