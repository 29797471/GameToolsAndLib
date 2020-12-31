using CqCore;

public class EncryptionStr : BaseTreeNotifyObject
{
    public string Content { get { return mContent; } set { mContent = value; Update("Content");Update("OutPut"); } }
    public string mContent;

    public string OutPut { get { return StringUtil.Md5Sum(mContent); } }

    public int Number { get { return mNumber; } set { mNumber = value; Update("Number"); } }
    public int mNumber;
}

