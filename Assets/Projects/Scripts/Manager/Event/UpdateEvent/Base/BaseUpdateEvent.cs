using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 更新事件的基类
    /// </summary>
    public class BaseUpdateEvent : IUpdateEvent, IDisposable
    {
        public BaseUpdateEvent()
        {

        }

        /// <summary>
        /// 不可暂停事件对象
        /// </summary>
        protected Dictionary<string, List<UpdateEventObject>> eventObjects=new Dictionary<string, List<UpdateEventObject>>();

        /// <summary>
        /// 可暂停事件对象
        /// </summary>
        protected Dictionary<string, List<UpdateEventObject>> pauseEventObjects = new Dictionary<string, List<UpdateEventObject>>();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventCall"></param>
        public virtual UpdateEvent_CallBack AddListener(string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum)
        {
            switch (operationModeEnum)
            {
                case OperationModeEnumType.Pause:
                    if (pauseEventObjects.ContainsKey(eventName))
                    {
                        pauseEventObjects[eventName].Add(new UpdateEventObject(listener));
                    }
                    else
                    {
                        pauseEventObjects.Add(eventName, new List<UpdateEventObject>() { new UpdateEventObject(listener) });
                    }
                    break;
                case OperationModeEnumType.NoPause:
                    if (eventObjects.ContainsKey(eventName))
                    {
                        eventObjects[eventName].Add(new UpdateEventObject(listener));
                    }
                    else
                    {
                        eventObjects.Add(eventName, new List<UpdateEventObject>() { new UpdateEventObject(listener) });
                    }
                    break;
                default:
                    break;
            }
            return listener;
        }
        /// <summary>
        /// 去除事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public virtual void RemoveListener(string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum)
        {
            UpdateEventObject eventObject = default(UpdateEventObject);
            switch (operationModeEnum)
            {
                case OperationModeEnumType.Pause:
                    if (pauseEventObjects.ContainsKey(eventName))
                    {
                        eventObject = pauseEventObjects[eventName].Find(e => e.eventCall == listener);
                        pauseEventObjects[eventName].Remove(eventObject);
                    }
                    break;
                case OperationModeEnumType.NoPause:
                    if (eventObjects.ContainsKey(eventName))
                    {
                        eventObject = eventObjects[eventName].Find(e => e.eventCall == listener);
                        eventObjects[eventName].Remove(eventObject);
                    }
                    break;
                default:
                    break;
            }
            if (eventObject != null)
            {
                bool Exist = false;
                foreach (var item in eventObject.eventCall.GetInvocationList())
                {
                    if (item.Method == listener.Method)
                    {
                        Exist = true;
                    }
                }
                if (!Exist)
                {
                    Debug.Log(eventName + "==listener监听事件不存在字典中！移除监听参数不能为Lambda表达式。\n解决方法请参考添加监听方法 （AddListener）的返回值！！！");
                    return;
                }
            }
            else
            {
                Debug.Log("事件<" + eventName + ">不存在");
            }
        }
        /// <summary>
        /// 去除所有名字下事件
        /// </summary>
        /// <param name="eventName"></param>
        public virtual void RemoveAllListener(string eventName, OperationModeEnumType operationModeEnum)
        {
            UpdateEventObject eventObject = default(UpdateEventObject);
            switch (operationModeEnum)
            {
                case OperationModeEnumType.NoPause:
                    if (eventObjects.ContainsKey(eventName))
                        eventObjects.Remove(eventName);
                    break;
                case OperationModeEnumType.Pause:
                    if (pauseEventObjects.ContainsKey(eventName))
                        pauseEventObjects.Remove(eventName);
                    break;
                default:
                    break;
            }
            if (eventObject != null)
            {
                Debug.Log("事件<" + eventName + ">不存在");
            }
        }


        /// <summary>
        /// 暂停事件
        /// </summary>
        public virtual void PauseEvent(string eventName, UpdateEvent_CallBack listener)
        {
            if (pauseEventObjects.ContainsKey(eventName))
            {
                UpdateEventObject eventObject = pauseEventObjects[eventName].Find(e => e.eventCall == listener);
                if (eventObject != null)
                    eventObject.isUpdate = false;

            }
        }
        /// <summary>
        /// 暂停事件
        /// </summary>
        public virtual void PauseEvent(string eventName)
        {
            if (pauseEventObjects.ContainsKey(eventName))
                for (int i = 0; i < pauseEventObjects[eventName].Count; i++)
                {
                    pauseEventObjects[eventName][i].isUpdate = false;
                }
        }
        /// <summary>
        /// 暂停所有事件
        /// </summary>
        public virtual void PauseAllEvent()
        {
            foreach (var item in pauseEventObjects)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    item.Value[i].isUpdate = false;
                }
            }
        }


        /// <summary>
        /// 恢复暂停事件
        /// </summary>
        public virtual void RenewPauseEvent(string eventName, UpdateEvent_CallBack listener)
        {
            if (pauseEventObjects.ContainsKey(eventName))
            {
                UpdateEventObject eventObject = pauseEventObjects[eventName].Find(e => e.eventCall == listener);
                if (eventObject != null)
                    eventObject.isUpdate = true;

            }

        }
        /// <summary>
        /// 恢复暂停事件
        /// </summary>
        public virtual void RenewPauseEvent(string eventName)
        {
            if (pauseEventObjects.ContainsKey(eventName))
                for (int i = 0; i < pauseEventObjects[eventName].Count; i++)
                {
                    pauseEventObjects[eventName][i].isUpdate = true;
                }

        }
        /// <summary>
        /// 恢复所有暂停事件
        /// </summary>
        public virtual void RenewAllPauseEvent()
        {
            foreach (var item in pauseEventObjects)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    item.Value[i].isUpdate = true;
                }
            }
        }
        /// <summary>
        /// 清除所有事件
        /// </summary>
        public virtual void Clear(OperationModeEnumType operationModeEnum)
        {
            switch (operationModeEnum)
            {
                case OperationModeEnumType.NoPause:
                    eventObjects.Clear();
                    break;
                case OperationModeEnumType.Pause:
                    pauseEventObjects.Clear();
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {

        }
    }
}