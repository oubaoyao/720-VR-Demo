using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MTFrame
{
    /// <summary>
    /// Panel显示隐藏动画
    /// </summary>
    public abstract class UIBasePanelTween : IPanelTween
    {
        /// <summary>
        /// 绑定的Panel
        /// </summary>
        protected BasePanel panel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public UIBasePanelTween(BasePanel panel)
        {
            this.panel = panel;
        }

        /// <summary>
        ///  把Panel重置到该动画的初始状态
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 把该动画修改过的Panel的状态重置回来
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="tweenTime"></param>
        public virtual void Open(float tweenTime)
        {
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="tweenTime"></param>
        public virtual void Hide(float tweenTime)
        {
        }

    }
}