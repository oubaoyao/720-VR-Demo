
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
     public class Event : MainBehavior
    {
        public Text GestureName;
        public ButtonBase eventButton;
        public PlayerGestureEventInfo playerGestureEventInfo=new PlayerGestureEventInfo();
        private EventWindows eventWindows;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_playerGestureEventInfo"></param>
        /// <param name="_eventWindows"></param>
        public void Init(PlayerGestureEventInfo _playerGestureEventInfo, EventWindows _eventWindows)
        {
            playerGestureEventInfo = _playerGestureEventInfo;

            GestureName.text = playerGestureEventInfo.eventName;
            eventWindows = _eventWindows;
            eventButton.OnClick.AddListener(() =>
            {
                eventWindows.ReturnEventButton(this);
            });
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recovery()
        {
           transform.parent = eventWindows.transform;
           transform.localScale = Vector3.zero;
           eventButton.InitEvent();
        }
    }
}
