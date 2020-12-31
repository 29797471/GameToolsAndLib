namespace CqCore
{
    [System.Flags]
    public enum MouseButton
    {
        //     鼠标左按钮。
        Left  = 1 << 0,
        //     鼠标右按钮。
        Right = 1<<1,
        //     鼠标中按钮
        Middle = 1<<2,
        //     第 1 个 XButton
        XButton1 = 1<<3,
        //     第 2 个 XButton
        XButton2 = 1<<4,
    }
}
