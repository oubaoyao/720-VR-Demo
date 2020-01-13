using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拓展CanvasGroup方法
/// </summary>
public static class DOCanvasGroup
{
    //暂存的数据用于避免重复开启
    private static Dictionary<CanvasGroup,TweenObject> canvasGroupDic= new Dictionary<CanvasGroup, TweenObject>();
    /// <summary>
    /// 拓展CanvasGroup方法
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="value"></param>
    /// <param name="needTime"></param>
    public static TweenObject DOFillAlpha(this CanvasGroup canvasGroup, float value, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        if (value > 1)
            value = 1;
        else if (value < 0)
            value = 0;
        if (canvasGroupDic.ContainsKey(canvasGroup))
        {
            canvasGroupDic[canvasGroup].Stop();//暂停上一个
            canvasGroupDic.Remove(canvasGroup);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        float alpha = canvasGroup.alpha;//记录最初角度数据
        float startAlpha = canvasGroup.alpha;//记录开始角度数据
        float alphaSpace = Mathf.Abs(canvasGroup.alpha - value);//记录最初间隔
        TweenObject tweenOgject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;//直接更新时间差
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;//与上一帧的时间差
                    break;
                default:
                    break;
            }
            
            alpha = Mathf.MoveTowards(alpha, value, fpsTimeDir * alphaSpace / needTime);//没有运用动画曲线正常的值

            float f =1-(Mathf.Abs(alpha - value) / alphaSpace);//通过正常值得出当前进度
            if (canvasGroup)
            {
                canvasGroup.alpha = Mathf.LerpUnclamped(startAlpha, value, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值
            }
            else
            {
                tween.Stop();
            }
            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间
            return f;
        });

        tweenOgject.OnComplete(() => { canvasGroup.alpha = value; });

        canvasGroupDic.Add(canvasGroup, tweenOgject);

        return tweenOgject;
    }
}