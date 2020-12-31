using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


public class FilePathAttribute : ControlPropertyAttribute
{
    
    public bool isRelativePath;
    public string filter;
    public FilePathAttribute(string name = null, bool isRelativePath = false, string filter = "文件|*.*", float width = 0f) : base(name, width)
    {
        this.isRelativePath = isRelativePath;
        this.filter = filter;
    }
    

    public override FrameworkElement CreateFrameworkElement()
    {
        var xx = new FilePathData();
        xx.isRelativePath = isRelativePath;
        xx.filter = filter;
        xx.SetV = (o)=>Target=o;
        xx.Path =  (string)Target;
        return WinUtil.DrawPanel(xx);
    }

    [Editor(null,null,Orientation.Horizontal)]
    public class FilePathData : NotifyObject
    {
        public bool isRelativePath;
        public string filter;
        public string mPath;

        [Priority(1)]
        [TextBox(), MinWidth(100)]
        public string Path
        {
            get { return mPath; }
            set { mPath=value; Update("Path");SetV?.Invoke(value); }
        }
        public Action<string> SetV;

        [Priority(2)]
        [Button,Click("Blow")]
        public string View
        {
            get
            {
                return "浏览";
            }
        }
        public void Blow(object obj)
        {
            string str=WinUtil.GetFileByOpenDialog(Path, filter, "请选择文件");
            if(str!=null)
            {
                if (isRelativePath)
                {
                    Path = FileOpr.ToRelativePath(str);
                }
                else
                {
                    Path = str;
                }
            }
        }
    }
}

