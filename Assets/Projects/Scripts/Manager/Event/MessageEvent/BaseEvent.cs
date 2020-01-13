using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public class BaseEvent : IGenericEvent
    {
        /// <summary>
        /// 事件对象
        /// </summary>
        protected List<EventObject> eventObjects=new List<EventObject>();

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
        }
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventCall"></param>
        public virtual GenericEvent_CallBack AddListener(string eventName, GenericEvent_CallBack listener)
        {
            EventObject eventObject = eventObjects.Find(e => e.eventName == eventName);
            if (eventObject != null)
            {
                eventObject.eventCall += listener;
            }
            else
            {
                eventObject = new EventObject(eventName, listener);
                eventObjects.Add(eventObject);
            }
            return listener;
        }
        /// <summary>
        /// 去除事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public virtual void RemoveListener(string eventName, GenericEvent_CallBack listener)
        {
            EventObject eventObject = eventObjects.Find(e => e.eventName == eventName);
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
                    Debug.Log("listener监听事件不存在字典中！移除监听参数不能为Lambda表达式。\n解决方法请参考添加监听方法 （AddListener）的返回值！！！");
                    return;
                }

                eventObject.eventCall -= listener;//移除监听  
                if (eventObject.eventCall == null)//该类型是否还有回调，如果没有，移除  
                    eventObjects.Remove(eventObject);
            }
            else
            {
                Debug.Log(string.Concat("事件《｛0｝》不存在", eventName));
            }
        }
        /// <summary>
        /// 去除所有名字下事件
        /// </summary>
        /// <param name="eventName"></param>
        public virtual void RemoveAllListener(string eventName)
        {
            EventObject eventObject = eventObjects.Find(e => e.eventName == eventName);
            if (eventObject != null)
            {
                eventObjects.Remove(eventObject);
            }
            else
            {
                Debug.Log(string.Concat("事件《｛0｝》不存在", eventName));
            }
        }

        /// <summary>
        /// 清除所有事件
        /// </summary>
        public virtual void Clear()
        {
            eventObjects.Clear();
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName"></param>
        public virtual void TriggerEvent(string eventName, EventParamete parameteData=null)
        {
            EventObject eventObject = eventObjects.Find(e => e.eventName == eventName);
            if (eventObject == null)
            {
                Debug.Log(string.Concat("事件《｛0｝》不存在", eventName));
            }
            else
            {
                if (parameteData == null)
                    parameteData = new EventParamete();
                parameteData.EvendName = eventName;
                eventObject.eventCall(parameteData);//传入参数，执行回调  
            }
        }

    }
}