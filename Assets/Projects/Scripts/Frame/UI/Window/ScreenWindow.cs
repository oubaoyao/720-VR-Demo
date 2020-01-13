

/// <summary>
/// 作为场景物体的存储窗口（屏幕窗口）
/// </summary>
public class ScreenWindow : BaseWindow
{
    /// <summary>
    /// 窗口类型
    /// </summary>
    public override WindowTypeEnum Type
    {
        get
        {
            return WindowTypeEnum.Screen;
        }
    }
}

