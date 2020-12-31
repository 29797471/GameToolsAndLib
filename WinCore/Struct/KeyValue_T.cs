[Editor("键值对")]
public class KeyValue<T>:NotifyObject
{
    [Priority(1)]
    [MinWidth(100)]
    [TextBox("键")]
    [GridViewColumn("键")]
    public string Key { get { return mKey; } set {mKey=value;Update("Key"); } }
    public string mKey;

    [Priority(2)]
    [MinWidth(200)]
    [TextBox("值")]
    [GridViewColumn("值")]
    public T Value { get { return mValue; } set { mValue = value; Update("Value"); } }
    public T mValue;
}