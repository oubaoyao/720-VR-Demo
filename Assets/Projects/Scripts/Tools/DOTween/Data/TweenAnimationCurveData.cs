using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 封装Twenn数据类
/// </summary>
public class TweenAnimationCurveData
{
    /// <summary>
    /// 类型
    /// </summary>
    public TweenType tweenType;
    /// <summary>
    /// AnimationCurve动画曲线的所有点数据
    /// </summary>
    public Keyframe[] keyframes;

}
public class TweenAnimationInfo
{
    public TweenType tweenTypeData;

    public AnimationCurve animationCurve;
}