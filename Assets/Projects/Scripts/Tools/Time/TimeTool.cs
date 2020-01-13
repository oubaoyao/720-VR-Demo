using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public delegate void TimeCallEvent();

/// <summary>
/// 时间工具
/// </summary>
public class TimeTool : IMainUpdate, IDisposable
{

    private static TimeTool instance;
    public static TimeTool Instance
    {
        get
        {
            if (instance == null) { instance = new TimeTool(); }
            return instance;
        }
    }
    public TimeTool()
    {
        UpdateSystem.Instance?.Add(this);
        isAsyncUpdate = true;

        timeNow = Time.realtimeSinceStartup;
    }

    //速否刷新
    private bool isAsyncUpdate;
    //事件差手术Time.timeScale影响
    private float timeNow;
    // 延时调用
    private Dictionary<TimeDownType, List<TimeDownObject>> delayedObjects = new Dictionary<TimeDownType, List<TimeDownObject>>();
    //延时调用队列
    private Queue<TimeDownObject> timeDownQueue = new Queue<TimeDownObject>();

    /// <summary>
    /// 添加延时调用
    /// </summary>
    /// <param name="endTime"></param>
    /// <param name="DelayedCall"></param>
    public TimeDownObject AddDelayed(TimeDownType timeDownType, float endTime, TimeCallEvent DelayedCall, bool loop = false)
    {
        TimeDownObject timeDownObject = new TimeDownObject(timeDownType, endTime, DelayedCall, loop, this);

        if (delayedObjects.ContainsKey(timeDownType))
        {
            delayedObjects[timeDownType].Add(timeDownObject);
        }
        else
        {
            delayedObjects.Add(timeDownType, new List<TimeDownObject>() { timeDownObject });
        }

        return timeDownObject;
    }


    /// <summary>
    /// 查找延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public TimeDownObject FindDelayed(TimeDownType timeDownType, TimeCallEvent DelayedCall)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            TimeDownObject timeDownObject = delayedObjects[timeDownType].Find(p => p.DelayedCall == DelayedCall);
            if (timeDownObject != null)
                return timeDownObject;
            else
                Debug.Log("延时调用不存在==" + DelayedCall);
        }
        return null;

    }

    /// <summary>
    /// 播放延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public void PlayDelayed(TimeDownType timeDownType, TimeCallEvent DelayedCall)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            TimeDownObject timeDownObject = delayedObjects[timeDownType].Find(p => p.DelayedCall == DelayedCall);
            if (timeDownObject != null)
                timeDownObject.Play();
            else
                Debug.Log("延时调用不存在==" + DelayedCall);
        }

    }
    /// <summary>
    /// 暂停延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public void PauseDelayed(TimeDownType timeDownType, TimeCallEvent DelayedCall)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            TimeDownObject timeDownObject = delayedObjects[timeDownType].Find(p => p.DelayedCall == DelayedCall);
            if (timeDownObject != null)
                timeDownObject.Pause();
            else
                Debug.Log("延时调用不存在==" + DelayedCall);
        }

    }
    /// <summary>
    /// 停止延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public void StopDelayed(TimeDownType timeDownType, TimeCallEvent DelayedCall)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            TimeDownObject timeDownObject = delayedObjects[timeDownType].Find(p => p.DelayedCall == DelayedCall);
            if (timeDownObject != null)
                timeDownObject.Stop();
            else
                Debug.Log("延时调用不存在==" + DelayedCall);
        }

    }


    /// <summary>
    /// 移除延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public void Remove(TimeDownType timeDownType, TimeCallEvent DelayedCall)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            TimeDownObject timeDownObject = delayedObjects[timeDownType].Find(p => p.DelayedCall == DelayedCall);
            if (timeDownObject != null)
                Remove(timeDownType, timeDownObject);
            else
                Debug.Log("延时调用不存在==" + DelayedCall);
        }

    }

    /// <summary>
    /// 移除延时对象
    /// </summary>
    /// <param name="timeDownType"></param>
    /// <param name="timeDown"></param>
    public void Remove(TimeDownType timeDownType, TimeDownObject timeDown)
    {
        if (delayedObjects.ContainsKey(timeDownType))
        {
            delayedObjects[timeDownType].Remove(timeDown);
        }

    }

    //开启更新
    public void OnUpdate()
    {
        if (delayedObjects.ContainsKey(TimeDownType.UnityTimeLineImpact))
        {
            for (int i = 0; i < delayedObjects[TimeDownType.UnityTimeLineImpact].Count; i++)
            {
                TimeDownObject timeDownObject = delayedObjects[TimeDownType.UnityTimeLineImpact][i];
                if (timeDownObject.InTime(Time.deltaTime))
                {
                    timeDownObject.Run();
                }
            }
        }


        float timeDir = Time.realtimeSinceStartup - timeNow;
        if (delayedObjects.ContainsKey(TimeDownType.NoUnityTimeLineImpact))
        {
            for (int i = 0; i < delayedObjects[TimeDownType.NoUnityTimeLineImpact].Count; i++)
            {
                TimeDownObject timeDownObject = delayedObjects[TimeDownType.NoUnityTimeLineImpact][i];
                if (timeDownObject.InTime(timeDir))
                {
                    timeDownObject.Run();
                }
            }
        }
        timeNow = Time.realtimeSinceStartup;


        if (delayedObjects.ContainsKey(TimeDownType.Async))
        {
            for (int i = 0; i < delayedObjects[TimeDownType.Async].Count; i++)
            {
                TimeDownObject timeDownObject = delayedObjects[TimeDownType.Async][i];
                bool b = timeDownObject.InTime(timeDir);
                if (b)
                {
                    AsyncUpdate(timeDownObject);
                }
            }
        }
        if (delayedObjects.ContainsKey(TimeDownType.Thread))
        {
            for (int i = 0; i < delayedObjects[TimeDownType.Thread].Count; i++)
            {
                TimeDownObject timeDownObject = delayedObjects[TimeDownType.Thread][i];
                if (timeDownObject.InTime(timeDir))
                {
                    ThreadUpdate(timeDownObject);
                }
            }
        }
    }
    // 异步更新
    private async void AsyncUpdate(TimeDownObject timeDownObject )
    {
        Task task = Task.Run(() =>
        {
            timeDownObject.Run();
        });
        await task;
    }

    public List<Thread> threads = new List<Thread>();

    Thread thread;
    // 线程更新
    private void ThreadUpdate(TimeDownObject timeDownObject)
    {
         thread = new Thread(new ThreadStart(() => 
        {
            Thread _thread = thread;
            timeDownObject.Run();
            threads.Remove(_thread);
        }));
        thread.Start();
        threads.Add(thread);
    }



    ~TimeTool() { }
    public void Dispose()
    {
        delayedObjects.Clear();
        isAsyncUpdate = false;
        UpdateSystem.Instance?.Remove(this);
        GC.SuppressFinalize(this);

        for (int i = 0; i < threads.Count; i++)
        {
            threads[i] .Abort();
        }
        threads.Clear();
    }
}
