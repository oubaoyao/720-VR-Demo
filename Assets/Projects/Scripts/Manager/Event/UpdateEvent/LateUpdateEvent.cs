using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 最后更新事件
    /// </summary>
    public class LateUpdateEvent : BaseUpdateEvent, ILateUpdate
    {
        public LateUpdateEvent()
        {
            UpdateSystem.Instance?.Add(this);
        }

        /// <summary>
        /// 最后更新（注释通过UpdateSystem调用更新）
        /// </summary>
        public void OnLateUpdate()
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
            catch { }

        }
    }
}