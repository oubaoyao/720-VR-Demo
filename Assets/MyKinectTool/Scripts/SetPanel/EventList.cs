using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace MTFrame.MTKinect
{
    public class EventList : MainBehavior, ISerializeButton
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

        public RectTransform Content;
        public RectTransform Detailed;
        public Text value;
        public ButtonBase EndButton, OkButton;
        public List<Event> events = new List<Event>();

        private EventWindows eventWindows;
        private PlayerGestureEventInfo playerGestureEventInfo;
        public List<Event> _events = new List<Event>();


        // Use this for initialization
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
            Content = FindTool.FindChildComponent<RectTransform>(transform, "Scroll View/Viewport/Content");
            Detailed = FindTool.FindChildComponent<RectTransform>(transform, "Detailed");
            value = FindTool.FindChildComponent<Text>(Detailed, "Value");
            EndButton = FindTool.FindChildComponent<ButtonBase>(Detailed, "End");
            OkButton = FindTool.FindChildComponent<ButtonBase>(Detailed, "Ok");

            Detailed.transform.localScale = Vector3.zero;
        }
        private void InitEvent()
        {
            EndButton.OnClick.AddListener(() =>
            {
                Detailed.transform.localScale = Vector3.zero;
            });
            OkButton.OnClick.AddListener(() =>
            {
                Detailed.transform.localScale = Vector3.zero;
                eventWindows.AddEvent(playerGestureEventInfo);
                Recovery();
            });
        }

        /// <summary>
        /// 恢复事件按钮信息
        /// </summary>
        /// <param name="event"></param>
        public void ReturnEventButton(Event @event)
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i] == @event)
                    continue;
                events[i].eventButton.Init();
            }
        }
        /// <summary>
        /// 设置面板
        /// </summary>
        /// <param name="_eventWindows"></param>
        public void SetPanel(EventWindows _eventWindows)
        {
            transform.localScale = Vector3.one;
            eventWindows = _eventWindows;
            foreach (var item in PlayerManager.Instance.gestureEvents)
            {
                bool isF = false;
                for (int i = 0; i < eventWindows.events.Count; i++)
                {
                    if (eventWindows.events[i].playerGestureEventInfo.eventName == item.eventName && eventWindows.events[i].playerGestureEventInfo.eventDetailed == item.eventDetailed)
                    {
                        isF = true;
                        break;
                    }
                }
                if (isF)
                    continue;
                Event @event;
                if (_events.Count > 0)
                {
                    @event = _events[0];
                    @event.transform.parent = Content;
                    @event.transform.localScale = Vector3.one;
                    _events.Remove(@event);
                }
                else
                {
                    @event = SourcesManager.LoadSources<Event>("Event", Content.transform);
                }
                @event.Init(item, eventWindows);
                @event.eventButton.OnClick.AddListener(() =>
                {
                    Detailed.transform.localScale = Vector3.one;
                    value.text = @event.playerGestureEventInfo.eventDetailed;
                    playerGestureEventInfo = @event.playerGestureEventInfo;

                    ReturnEventButton(@event);
                });
                events.Add(@event);
            }
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recovery()
        {
            transform.localScale = Vector3.zero;

            for (int i = 0; i < events.Count; i++)
            {
                events[i].Recovery();
                _events.Add(events[i]);
            }
            events.Clear();
        }
    }
}
