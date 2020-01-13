
/// <summary>
/// 作为场景物体的存储窗口（世界窗口）
/// </summary>
public class WorldWindow: BaseWindow
{
    /// <summary>
    /// 窗口类型
    /// </summary>
    public override WindowTypeEnum Type
    {
        get
        {
            return WindowTypeEnum.World;
        }
    }
}

