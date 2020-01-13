
using UnityEngine;
/// <summary>
/// 作为场景物体的存储窗口（前景窗口）
/// </summary>
public class ForegroundScreenWindow : BaseWindow
{
    protected override void Start()
    {
        base.Start();
    }
    /// <summary>
    /// 窗口类型
    /// </summary>
    public override WindowTypeEnum Type
    {

        get
        {
            return WindowTypeEnum.ForegroundScreen;
        }
    }
}

