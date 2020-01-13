using System;
using UnityEngine;

/// <summary>
/// Treen对象
/// </summary>
public class TweenObject
{
    //构造函数
    public TweenObject(TweenLogicBehavior logicBehavior = null)
    {
        tweenLogicBehavior = logicBehavior;
        Play();
    }

    /// <summary>
    /// 开始回调
    /// </summary>
    public Action onCommence;
    /// <summary>
    /// 更新回调
    /// </summary>
    public Action<float> onUpdate;
    /// <summary>
    /// 完成回调
    /// </summary>
    public Action onComplete;

    /// <summary>
    /// 进程
    /// </summary>
    public float process { get; private set; }
    /// <summary>
    /// 是否播放
    /// </summary>
    public bool isPlay { get; private set; }

    //逻辑行为
    private TweenLogicBehavior tweenLogicBehavior;
    
    //是否开始
    private bool isCommence;

    // Tween曲线
    private TweenType tweenType;



    /// <summary>
    /// 监听开始回调
    /// </summary>
    /// <param name="onCommence"></param>
    /// <returns></returns>
    public TweenObject OnCommence(Action onCommence)
    {
        this.onCommence += onCommence;
        return this;
    }
    /// <summary>
    /// 监听更新回调
    /// </summary>
    /// <param name="onCommence"></param>
    /// <returns></returns>
    public TweenObject OnUpdate(Action<float> onUpdate)
    {
        this.onUpdate += onUpdate;
        return this;
    }
    /// <summary>
    /// 监听完成回调
    /// </summary>
    /// <param name="onCommence"></param>
    /// <returns></returns>
    public TweenObject OnComplete(Action onComplete)
    {
        this.onComplete += onComplete;
        return this;
    }
    /// <summary>
    /// 更改动画类型
    /// </summary>
    /// <param name="tween"></param>
    /// <returns></returns>
    public TweenObject Ease(TweenType tween)
    {
        tweenType = tween;
        return this;
    }

    /// <summary>
    /// 更新逻辑
    /// </summary>
    public void UpdateLogic()
    {
        if (tweenLogicBehavior != null)
            try
            {
                process = tweenLogicBehavior.Invoke(this);
            }
            catch 
            {
                Debug.Log("tweenLogicBehavior==异常");
                Stop();
            }
        if (!isCommence)
        {
            isCommence = true;
               onCommence?.Invoke();
        }
        onUpdate?.Invoke(process);
        if (process >= 1||float.IsNaN(process))
        {
            Stop();
            onComplete?.Invoke();
        }
    }

    /// <summary>
    /// 获取动画曲线的偏差值；
    /// </summary>
    /// <returns></returns>
    public float GetAnimationCurveValue()
    {
       AnimationCurve animationCurve=  DOTween.Instance.GetEase(tweenType);
        if (!float.IsNaN(process))
            return animationCurve.Evaluate(process);
        else return 1;
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        if (!isPlay)
        {
            isPlay = true;
            DOTween.Instance.AddTween(this);
        }
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        if (isPlay)
        {
               isPlay = false;
            DOTween.Instance.RemoveTween(this);
            isCommence = false;
        }
    }
}
