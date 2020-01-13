using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 计时对象
/// </summary>
public class TimeDownObject : IDisposable
{
    TimeDownType timeDownType;
    // 结束时间
    private float endTime;
    //名称
    public string Name;
    //结束回调
    public TimeCallEvent DelayedCall;
    private bool loop;
    private TimeTool timeTool;

    private bool isCompleteRun=true;


    private bool isRun;

    private float currentTime;
    public TimeDownObject(TimeDownType timeDownType, float endTime, TimeCallEvent DelayedCall, bool loop, TimeTool timeTool)
    {
        Play();
        this. timeDownType = timeDownType;
        this.endTime = endTime;
        this.DelayedCall = DelayedCall;
        this.loop = loop;
        this.timeTool = timeTool;
    }
    public bool InTime(float time)
    {
        if (!isRun||!isCompleteRun)
            return false;
        currentTime += time;
        if (currentTime >= endTime)
        {
            currentTime = 0;
            isCompleteRun = false;
            return true;
        }
        return false;
    }
    public void Run()
    {
        if (isRun)
            DelayedCall.Invoke();
        if (!loop)
            Stop();
        currentTime = 0;
        isCompleteRun = true;
    }

    public void Play()
    {
        isRun = true;
    }

    public void Pause()
    {
        isRun = false;
    }
    public void Stop()
    {
        isRun = false;
        timeTool.Remove(timeDownType, this);
    }

    ~TimeDownObject() { }
    /// <summary>
    /// 销毁对象
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

}

public enum ContrastType
{
    /// <summary>
    /// 大于
    /// </summary>
    Greater,
    /// <summary>
    /// 小于
    /// </summary>
    Less,
    /// <summary>
    /// 等于
    /// </summary>
    Equals,
    /// <summary>
    /// 大于或等于
    /// </summary>
    GreaterTOEquals,
    /// <summary>
    /// 小于或等于
    /// </summary>
   LessTOEquals
}