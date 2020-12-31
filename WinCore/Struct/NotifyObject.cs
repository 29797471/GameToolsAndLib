using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;


/// <summary>
/// 绑定控件的数据结构基类
/// </summary>
[System.Serializable]
public class NotifyObject : INotifyPropertyChanged
{
    public void Update(string propertyName)
    {
        if(PropertyChanged!=null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public int GetProCount
    {
        get
        {
            if (PropertyChanged == null) return 0;
            return PropertyChanged.GetInvocationList().Length;
        }
    }
}

/// <summary>
/// 绑定控件的数据结构基类
/// </summary>
[System.Serializable]
public class NotifyList : INotifyCollectionChanged
{
    public void UpdateCollection(IList list)
    {
        if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, list));
    }
    public event NotifyCollectionChangedEventHandler CollectionChanged;
}