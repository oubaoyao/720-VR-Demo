using MTFrame.MTEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MTFrame
{
    /// <summary>
    /// 程序状态基类
    /// </summary>
    public abstract class BaseState : IListenerMessage
    {

        /// <summary>
        /// 监听信息所有ID
        /// </summary>
        public abstract string[] ListenerMessageID { get; set;}


        private static BaseTask currentTask;
        /// <summary>
        /// 当前的任务
        /// </summary>
        public BaseTask CurrentTask {
            get
            {
                if (currentTask == null)
                    currentTask = new BaseTask(this);
                return currentTask;
            }
            set
            {
                currentTask = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseState()
        {
        }

        /// <summary>
        /// 进入State
        /// </summary>
        public virtual void Enter()
        {
                EventManager.AddListeners(GenericEventEnumType.Message, ListenerMessageID, OnListenerMessage);
            EventManager.AddUpdateListener(UpdateEventEnumType.Update, GetType().Name, OnUpdate);
        }
        /// <summary>
        /// 退出State
        /// </summary>
        public virtual void Exit()
        {
            if (CurrentTask != null)
            {
                CurrentTask.Exit();//退出当前任务
                CurrentTask = null;
            }

            EventManager.RemoveListeners(GenericEventEnumType.Message, ListenerMessageID, OnListenerMessage);
            EventManager.RemoveUpdateListener(UpdateEventEnumType.Update, GetType().Name, OnUpdate);
        }

        /// <summary>
        /// 信息监听
        /// </summary>
        /// <param name="parameteData"></param>
        public abstract void OnListenerMessage(EventParamete parameteData);

        /// <summary>
        /// 状态更新
        /// </summary>
        protected virtual void OnUpdate(float processTime)
        {
            if (CurrentTask != null) CurrentTask.OnUpdate();
        }
    }
}