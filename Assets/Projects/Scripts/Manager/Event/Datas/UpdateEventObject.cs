
using System;
using UnityEngine;
namespace MTFrame.MTEvent
{
    /// <summary>
    /// 更新事件对象
    /// </summary>
    public class UpdateEventObject
    {
        /// <summary>
        /// 事件回调            
        /// </summary>
        public UpdateEvent_CallBack eventCall;
        /// <summary>
        /// 是否更新
        /// </summary>
        public bool isUpdate = true;
        //构造函数
        public UpdateEventObject(UpdateEvent_CallBack eventCall)
        {
            this.eventCall = eventCall;
        }
        //事件进程
        private float timeProcess = 0;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Run(float deltaTime)
        {
            timeProcess += deltaTime;
            if (isUpdate)
                eventCall(timeProcess);
        }
    }
}