using System;
using UnityEngine;

namespace MTFrame
{
    /// <summary>
    /// 按钮基类
    /// </summary>
    public abstract class BaseButton : UIBehavior
    {
        //点击
        [HideInInspector]
        public Action<BaseButton> OnClick;
        //弹起
        [HideInInspector]
        public Action<BaseButton> OnDown;
        //弹起
        [HideInInspector]
        public Action<BaseButton> OnUp;
        //进入
        [HideInInspector]
        public Action<BaseButton> OnEnter;
        //出去
        [HideInInspector]
        public Action<BaseButton> OnExit;

        /// <summary>
        /// 触发点击
        /// </summary>
        public virtual void TriggerClick()
        {
            OnClick?.Invoke(this );
        }
        /// <summary>
        /// 触发弹起
        /// </summary>
        public virtual void TriggerDown()
        {
            OnDown?.Invoke(this);
        }
        /// <summary>
        /// 触发弹起
        /// </summary>
        public virtual void TriggerUp()
        {
            OnUp?.Invoke(this);
        }
        /// <summary>
        /// 触发进入
        /// </summary>
        public virtual void TriggerEnter()
        {
            OnEnter?.Invoke(this);
        }
        /// <summary>
        /// 触发离开
        /// </summary>
        public virtual void TriggerExit()
        {
            OnExit?.Invoke(this);
        }

    }
}