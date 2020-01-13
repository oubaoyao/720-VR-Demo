
using System;
using UnityEngine;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 事件对象
    /// </summary>
    public class EventObject
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string eventName;
        /// <summary>
        /// 事件回调
        /// </summary>
        public GenericEvent_CallBack eventCall;

        //构造函数
        public EventObject(string eventName, GenericEvent_CallBack eventCall)
        {
            this.eventName = eventName;
            this.eventCall = eventCall;
        }
    }
}