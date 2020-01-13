using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class EventInfoWindows : MainBehavior, ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>()
                {
                    "FindObj",
                };
            }
        }

        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>()
                {
                    FindObj,
                };
            }
        }

        public Text EventName,Detailed;
        public InputField playerIndex;
        public ButtonBase Remove, Save;

        private Event @event;

        private EventWindows eventWindows;
        private PlayerGestureEventInfo playerGestureEventInfo;
      protected override void Start()
        {
            Init();
            FindObj();
            InitEvent();
        }

        private void Init()
        {
        }
        private void FindObj()
        {
            EventName = FindTool.FindChildComponent<Text>(transform, "EventName");
            Detailed = FindTool.FindChildComponent<Text>(transform, "Detailed");
            playerIndex = FindTool.FindChildComponent<InputField>(transform, "PlayerIndex/InputField");
            Remove = FindTool.FindChildComponent<ButtonBase>(transform, "Remove");
            Save = FindTool.FindChildComponent<ButtonBase>(transform, "Save");
        }
        private void InitEvent()
        {

            Remove.OnClick.AddListener(() =>
            {
                Recovery();
                eventWindows.RemoveEvent(@event);
            });
            Save.OnClick.AddListener(() =>
            {
                Recovery();
                try
                {
                    @event.playerGestureEventInfo.playerIndex = int.Parse(playerIndex.text);
                }
                catch
                {

                }
            });
        }

        /// <summary>
        /// 设置面板
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_eventWindows"></param>
        public void SetPanel( Event @_event, EventWindows _eventWindows)
        {
            @event = @_event;
            playerGestureEventInfo = @event.playerGestureEventInfo;
            eventWindows = _eventWindows;
            EventName.text = @event.playerGestureEventInfo.eventName;
            Detailed.text = @event.playerGestureEventInfo.eventDetailed;
            playerIndex.text = @event.playerGestureEventInfo.playerIndex.ToString();
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recovery()
        {
            transform.localScale = Vector3.zero;
            if (@event) 
            @event.eventButton.Init();
        }
    }
}