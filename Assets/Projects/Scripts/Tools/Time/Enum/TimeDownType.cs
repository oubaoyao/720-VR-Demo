using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时类型
/// </summary>
public enum TimeDownType
{
    /// <summary>
    /// 受到unity时间线的影响
    /// </summary>
    UnityTimeLineImpact,
    /// <summary>
    /// 不受到unity时间线的影响
    /// </summary>
    NoUnityTimeLineImpact,
    /// <summary>
    /// 线程
    /// </summary>
    Thread,
    /// <summary>
    /// 异步
    /// </summary>
    Async
}
/// <summary>
/// 计时模式
/// </summary>
public enum TimeDownMode
{
    /// <summary>
    /// 可暂停
    /// </summary>
    Pause,
    /// <summary>
    /// 不可暂停
    /// </summary>
    NoPause,


}