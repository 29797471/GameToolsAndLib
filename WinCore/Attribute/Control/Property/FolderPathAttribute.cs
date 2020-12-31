using System;
using System.Windows;
using System.Windows.Controls;


public class FolderPathAttribute : ControlPropertyAttribute
{
    public bool isRelativePath;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isRelativePath">相对路径</param>
    /// <param name="width"></param>
    public FolderPathAttribute(string name=null,bool isRelativePath=false,float width=0f) :base(name,width)
    {
        this.isRelativePath = isRelativePath;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var xx = new FolderPathData();
        xx.isRelativePath = isRelativePath;
        xx.SetV = (o) => Target = o;
        xx.Path = (string)Target;
        return WinUtil.DrawPanel(xx);
    }

    [Editor(null,null,Orientation.Horizontal)]
    public class FolderPathData : NotifyObject
    {
        public bool isRelativePath;
        public string filter;
        public string mPath;

        [Priority(0)]
        [TextBox(), MinWidth(100)]
        public string Path
        {
            get { return mPath; }
            set { mPath=value; Update("Path");SetV?.Invoke(value); }
        }
        public Action<string> SetV;

        [Priority(1)]
        [Button,Click("Blow")]
        public string View
        {
            get { return "浏览"; }
        }
        public void Blow(object obj)
        {
            string str = (string)Path;
            if (WinUtil.GetFolderByOpenDialog(ref str,  "请选择文件夹"))
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

