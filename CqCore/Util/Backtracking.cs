using System.Collections.Generic;

/// <summary>
/// 回溯
/// </summary>
public class Backtracking
{
    Stack<int> tagList;
    public Backtracking()
    {
        tagList = new Stack<int>();
    }
    public void Enter(int pos)
    {
        tagList.Push(pos);
    }

    /// <summary>
    /// 解析失败时回溯
    /// </summary>
    public int Back()
    {
        return tagList.Pop();
    }
}