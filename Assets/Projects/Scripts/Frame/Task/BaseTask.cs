using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MTFrame
{
    public class BaseTask : IDisposable
    {
        /// <summary>
        /// 当前任务所属的程序状态
        /// </summary>
        protected BaseState parentState { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="state"></param>
        public BaseTask(BaseState state)
        {
            this.parentState = state;
        }


        /// <summary>
        /// 进入任务
        /// </summary>
        public virtual void Enter()
        {

        }
        /// <summary>
        /// 退出任务
        /// </summary>
        public virtual void Exit()
        {
        }
        /// <summary>
        /// 任务更新
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 切换任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ChangeTask(BaseTask task)
        {
            Exit();
            if (task != null)
            {
                parentState.CurrentTask = task;
                parentState.CurrentTask.Enter();
            }
        }
    }
}