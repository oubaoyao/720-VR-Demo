using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTKinect
{
    public class EventWindows : MainBehavior,ISerializeButton
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
        public ButtonBase addButton, saveButton;
        public EventInfoWindows eventInfoWindows;
        public EventList eventList;
        /// <summary>
        /// 手势下的所有事件按钮
        /// </summary>
        public List<Event> events = new List<Event>();
        /// <summary>
        /// 当前手势
        /// </summary>
        public Gesture gesture;
        /// <summary>
        /// 关闭的所有事件按钮
        /// </summary>
        private List<Event> _events = new List<Event>();

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
            addButton = FindTool.FindChildComponent<ButtonBase>(transform, "Add");
            saveButton = FindTool.FindChildComponent<ButtonBase>(transform, "Save");
            eventList = FindTool.FindChildComponent<EventList>(transform, "EventList");
            eventInfoWindows = FindTool.FindChildComponent<EventInfoWindows>(transform, "EventInfoWindows");
        }
        private void InitEvent()
        {
            addButton.OnClick.AddListener(AddEvent);
            saveButton.OnClick.AddListener(SaveEvent);
            saveButton.OnClick.AddListener(Recovery);

            eventInfoWindows.Recovery();
            eventList.Recovery();
        }

        /// <summary>
        /// 恢复事件按钮信息
        /// </summary>
        /// <param name="event"></param>
        public void ReturnEventButton(Event @event)
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i]==@event)
                    continue;
                events[i].eventButton.Init();
            }
        }
        /// <summary>
        /// 封装的添加事件按钮
        /// </summary>
        /// <param name="_playerGestureEventInfo"></param>
        public void AddEvent(PlayerGestureEventInfo _playerGestureEventInfo)
        {
            PlayerGestureEventInfo playerGestureEventInfo = new PlayerGestureEventInfo();

            playerGestureEventInfo.eventDetailed = _playerGestureEventInfo.eventDetailed;
            playerGestureEventInfo.eventName = _playerGestureEventInfo.eventName;
            playerGestureEventInfo.gestureName = gesture.playerGestureData.playerGestureInfo.GestureName;
            //playerGestureEventInfo.isFindGesture = _playerGestureEventInfo.isFindGesture;
            playerGestureEventInfo.playerIndex = _playerGestureEventInfo.playerIndex;
            Event @event;
            if (_events.Count>0)
            {
                @event = _events[0];
                @event.transform.parent = Content;
                @event.transform.localScale = Vector3.one;
                _events.Remove(@event);
            }
            else
            {
                @event = SourcesManager.LoadSources<Event>("Event", Content);
            }
            @event.Init(playerGestureEventInfo, this);
            @event.eventButton.OffChoice.AddListener(() =>
            {
                eventInfoWindows.transform.localScale = Vector3.zero;
            });
            @event.eventButton.OnChoice.AddListener(() =>
            {
                eventInfoWindows.transform.localScale = Vector3.one;
                eventInfoWindows.SetPanel(@event, this);
            });
            events.Add(@event);
            PlayerGestureManager.Instance.ReturnGestureEvent(@event.playerGestureEventInfo);
            ReturnEventButton(@event);
            //for (int i = 0; i < events.Count; i++)
            //{

            //    Debug.Log(events[i].playerGestureEventInfo.playerIndex);
            //}
        }
        /// <summary>
        /// 封装删除事件按钮
        /// </summary>
        /// <param name="event"></param>
        public void RemoveEvent(Event @event)
        {
            @event.Recovery();
            events.Remove(@event);
            _events.Add(@event);
        }
        //添加事件
        private void AddEvent()
        {
            eventInfoWindows.Recovery();
            eventList.SetPanel(this);
        }
        //保存事件
        private void SaveEvent()
        {
            if (!gesture)
                return;
            gesture.playerGestureData.playerGestureEvents.Clear();
            for (int i = 0; i < events.Count; i++)
            {
                gesture.playerGestureData.playerGestureEvents.Add(events[i].playerGestureEventInfo);
                PlayerManager.Instance.SetPlayerGestureEvent(events[i].playerGestureEventInfo);
            }
            transform.localScale = Vector3.zero;
            gesture.gestureButton.Init();
            gesture.Init(gesture.playerGestureData, null);
            gesture = null;

            GC.Collect();
        }
        /// <summary>
        /// 设置面板数据
        /// </summary>
        /// <param name="_gesture"></param>
        public void SetPanel(Gesture _gesture)
        {
            transform.localScale = Vector3.one;
            gesture = _gesture;
            Debug.Log(gesture.playerGestureData.playerGestureEvents.Count);
            for (int i = 0; i < gesture.playerGestureData.playerGestureEvents.Count; i++)
            {
                AddEvent(gesture.playerGestureData.playerGestureEvents[i]);
            }
        }
        /// <summary>
        /// 恢复数据
        /// </summary>
        public void Recovery()
        {
            SaveEvent();
            transform.localScale = Vector3.zero;
            if (eventInfoWindows)
              eventInfoWindows.Recovery();
            if (eventList)
                eventList.Recovery();
            for (int i = 0; i < events.Count; i++)
            {
                events[i].Recovery();
                _events.Add(events[i]);
            }
            events.Clear();
        }
    }
}