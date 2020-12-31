/// <summary>
/// 多行注释中的一行
/// </summary>
public class CommentPart
{
    public string mCommentPartName;
    [Export("%CommentPart%")]
    public string CommentPartName
    {
        get { return mCommentPartName; }
        set { mCommentPartName = value; }
    }
}
