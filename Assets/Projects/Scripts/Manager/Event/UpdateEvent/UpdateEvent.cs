using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MTFrame.MTEvent
{
    /// <summary>
    /// 更新时间
    /// </summary>
    public class UpdateEvent : BaseUpdateEvent, IMainUpdate
    {
        public UpdateEvent()
        {
            UpdateSystem.Instance?.Add(this);
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void OnUpdate()
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