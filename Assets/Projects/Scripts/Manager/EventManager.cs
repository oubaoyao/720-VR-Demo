using MTFrame.MTEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public delegate void GenericEvent_CallBack( EventParamete parameteData);
public delegate void UpdateEvent_CallBack(float timeProcess);

/// <summary>
/// 事件管理器
/// </summary>
public class EventManager
{
    //常用事件
    private static GenericEvent genericEvent=new GenericEvent();
    //信息事件
    private static MessageEvent messageEvent=new MessageEvent();

    //固定更新事件
    private static FixedUpdateEvent fixedUpdateEvent=new FixedUpdateEvent();
    //更新事件
    private static UpdateEvent updateEvent=new UpdateEvent();
    //最后更新事件
    private static LateUpdateEvent lateUpdateEvent=new LateUpdateEvent();

    #region Generi通用

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    /// <param name="eventCall"></param>
    public static GenericEvent_CallBack AddListener(GenericEventEnumType eventEnumType, string eventName, GenericEvent_CallBack listener)
    {
        GenericEvent_CallBack _listener= listener;
        switch (eventEnumType)
        {
            case GenericEventEnumType.Generic:
                if (genericEvent != null)
                    _listener =genericEvent.AddListener(eventName, listener);
                break;
            case GenericEventEnumType.Message:
                if(messageEvent!=null )
                    _listener = messageEvent.AddListener(eventName, listener);
                break;
            default:
                break;
        }

        return _listener;
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventNames"></param>
    /// <param name="listener"></param>
    public static void AddListeners(GenericEventEnumType eventEnumType,string[] eventNames, GenericEvent_CallBack listener)
    {
        foreach (string eventName in eventNames)
        {
            AddListener(eventEnumType, eventName, listener);
        }
    }

    /// <summary>
    /// 去除事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void RemoveListener(GenericEventEnumType eventEnumType, string eventName, GenericEvent_CallBack listener)
    {
        switch (eventEnumType)
        {
            case GenericEventEnumType.Generic:
                if (genericEvent != null)
                    genericEvent.RemoveListener(eventName, listener);
                break;
            case GenericEventEnumType.Message:
                if (messageEvent != null)
                    messageEvent.RemoveListener(eventName, listener);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 去除事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventNames"></param>
    /// <param name="listener"></param>
    public static void RemoveListeners(GenericEventEnumType eventEnumType, string[] eventNames, GenericEvent_CallBack listener)
    {
        foreach (string eventName in eventNames)
        {
            RemoveListener(eventEnumType, eventName, listener);
        }
    }
    /// <summary>
    /// 去除所有名字下事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    public static void RemoveAllListener(GenericEventEnumType eventEnumType, string eventName)
    {
        switch (eventEnumType)
        {
            case GenericEventEnumType.Generic:
                if (genericEvent != null)
                    genericEvent.RemoveAllListener(eventName);
                break;
            case GenericEventEnumType.Message:
                if (messageEvent != null)
                    messageEvent.RemoveAllListener(eventName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 清除所有事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    public static void Clear(GenericEventEnumType eventEnumType)
    {
        switch (eventEnumType)
        {
            case GenericEventEnumType.Generic:
                if (genericEvent != null)
                    genericEvent.Clear();
                break;
            case GenericEventEnumType.Message:
                if (messageEvent != null)
                    messageEvent.Clear();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    public static void TriggerEvent(GenericEventEnumType eventEnumType,string eventName, EventParamete sender)
    {
        switch (eventEnumType)
        {
            case GenericEventEnumType.Generic:
                if (genericEvent != null)
                    genericEvent.TriggerEvent(eventName, sender);
                break;
            case GenericEventEnumType.Message:
                if (messageEvent != null)
                    messageEvent.TriggerEvent(eventName, sender);
                break;
            default:
                break;
        }
    }

    #endregion

    #region Update事件更新
    /// <summary>
    /// 添加Update更新监听
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static UpdateEvent_CallBack AddUpdateListener(UpdateEventEnumType eventEnumType, string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum = OperationModeEnumType.NoPause)
    {
        UpdateEvent_CallBack _listener = listener;
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    _listener = fixedUpdateEvent.AddListener(eventName, listener, operationModeEnum);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    _listener = updateEvent.AddListener(eventName, listener, operationModeEnum);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    _listener = lateUpdateEvent.AddListener(eventName, listener, operationModeEnum);
                break;
            default:
                break;
        }
        return _listener;
    }


    /// <summary>
    ///去除Update更新监听
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void RemoveUpdateListener(UpdateEventEnumType eventEnumType, string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum = OperationModeEnumType.NoPause)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.RemoveListener(eventName, listener,operationModeEnum);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.RemoveListener(eventName, listener, operationModeEnum);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.RemoveListener(eventName, listener, operationModeEnum);
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///去除对应项Update更新所有监听
    /// </summary>
    /// <param name="eventEnumType"></param>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void RemoveUpdateAllListener(UpdateEventEnumType eventEnumType, string eventName, OperationModeEnumType operationModeEnum = OperationModeEnumType.NoPause)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.RemoveAllListener(eventName, operationModeEnum);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.RemoveAllListener(eventName, operationModeEnum);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.RemoveAllListener(eventName, operationModeEnum);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 暂停事件
    /// </summary>
    public static void PauseAllEvent(UpdateEventEnumType eventEnumType, string eventName, UpdateEvent_CallBack listener)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.PauseEvent(eventName, listener);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.PauseEvent(eventName, listener);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.PauseEvent(eventName, listener);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 暂停事件
    /// </summary>
    public static void PauseAllEvent(UpdateEventEnumType eventEnumType, string eventName)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.PauseEvent(eventName);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.PauseEvent(eventName);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.PauseEvent(eventName);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 暂停所有事件
    /// </summary>
    public static void PauseAllEvent(UpdateEventEnumType eventEnumType)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.PauseAllEvent();
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.PauseAllEvent();
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.PauseAllEvent();
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 恢复暂停事件
    /// </summary>
    public static void RenewPauseEvent(UpdateEventEnumType eventEnumType, string eventName, UpdateEvent_CallBack listener)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.RenewPauseEvent(eventName, listener);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.RenewPauseEvent(eventName, listener);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.RenewPauseEvent(eventName, listener);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 恢复暂停事件
    /// </summary>
    public static void RenewPauseEvent(UpdateEventEnumType eventEnumType, string eventName)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.RenewPauseEvent(eventName);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.RenewPauseEvent(eventName);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.RenewPauseEvent(eventName);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 恢复所有暂停事件
    /// </summary>
    public static void RenewAllPauseEvent(UpdateEventEnumType eventEnumType)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.RenewAllPauseEvent();
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.RenewAllPauseEvent();
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.RenewAllPauseEvent();
                break;
            default:
                break;
        }
    }



    /// <summary>
    /// 清除所有事件
    /// </summary>
    /// <param name="eventEnumType"></param>
    public static void Clear(UpdateEventEnumType eventEnumType,OperationModeEnumType operationModeEnum = OperationModeEnumType.NoPause)
    {
        switch (eventEnumType)
        {
            case UpdateEventEnumType.FixedUpdate:
                if (fixedUpdateEvent != null)
                    fixedUpdateEvent.Clear(operationModeEnum);
                break;
            case UpdateEventEnumType.Update:
                if (updateEvent != null)
                    updateEvent.Clear(operationModeEnum);
                break;
            case UpdateEventEnumType.LateUpdate:
                if (lateUpdateEvent != null)
                    lateUpdateEvent.Clear(operationModeEnum);
                break;
            default:
                break;
        }
    }


    #endregion
}