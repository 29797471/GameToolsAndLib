using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

/// <summary>
/// 修饰树型结构
/// </summary>
public class TreeListViewAttribute : ControlPropertyAttribute
{
    public string types;
    public TreeListViewAttribute(string types, string name = null, float width = 0f) : base(name, width)
    {
        this.types = types;
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new TreeListView();
        ObjForTest root = new ObjForTest("root", "root", 0, "", 0);
        ObjForTest c1 = new ObjForTest("CEO", "Leo", 45, "M", 1);
        ObjForTest c2 = new ObjForTest("CFO", "Tami", 46, "FM", 1);
        ObjForTest c3 = new ObjForTest("COO", "Jack", 47, "M", 1);
        ObjForTest cc1 = new ObjForTest("Manager", "John", 30, "M", 2);
        ObjForTest cc2 = new ObjForTest("Manager", "Lee", 31, "FM", 2);
        ObjForTest cc3 = new ObjForTest("Manager", "Chris", 32, "M", 2);
        ObjForTest ccc1 = new ObjForTest("Worker", "Evan", 25, "FM", 3);
        root.Children.Add(c1);
        root.Children.Add(c2);
        root.Children.Add(c3);
        c1.Children.Add(cc1);
        c2.Children.Add(cc2);
        c3.Children.Add(cc3);
        cc1.Children.Add(ccc1);
        ctl.ItemsSource = root.Children;
        return ctl;
    }
}

public class ObjForTest : INotifyPropertyChanged
{
    public ObjForTest(string title, string name, int age, string sex, int level)
    {
        this._jobTitle = title;
        this._sex = sex;
        this._age = age;
        this._name = name;
        this._level = level;
    }
    private string _name;
    private int _age;
    private string _sex;
    private int _level;
    private string _jobTitle;

    public string Sex { get { return this._sex; } set { this._sex = value; } }
    public int Age { get { return this._age; } set { this._age = value; } }
    public int Level
    {
        get
        {
            return this._level;
        }
        set
        {
            _level = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Level"));
        }
    }
    public string JobTitle
    {
        get { return _jobTitle; }
        set
        {
            _jobTitle = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("JobTitle"));
        }
    }
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
        }
    }

    private ObservableCollection<ObjForTest> _children = new ObservableCollection<ObjForTest>();
    public ObservableCollection<ObjForTest> Children
    {
        get { return _children; }
    }


    public event PropertyChangedEventHandler PropertyChanged;
}