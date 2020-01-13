using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 固定更新事件
    /// </summary>
    public class FixedUpdateEvent : BaseUpdateEvent, IFixedUpdate
    {
        public FixedUpdateEvent()
        {
            UpdateSystem.Instance?.Add(this);
        }
        /// <summary>
        /// 固定更新（注释通过UpdateSystem调用更新）
        /// </summary>
        public void OnFixedUpdate()
        {
            if (eventObjects == null || pauseEventObjects == null) return;

            try
            {
                foreach (var item in eventObjects)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        item.Value[i].Run(Time.deltaTime);
                    }

                }
                foreach (var item in pauseEventObjects)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        item.Value[i].Run(Time.deltaTime);
                    }

                }
            }
            catch
            {
            }
        }
    }
}