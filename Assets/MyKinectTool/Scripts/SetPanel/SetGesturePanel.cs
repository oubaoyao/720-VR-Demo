using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class SetGesturePanel : MainBehavior, ISerializeButton
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
        public GestureWindows gestureWindows;
        public EventWindows eventWindows;
        public SelectionWindow selectionWindow;

        /// <summary>
        /// 所有手势按钮
        /// </summary>
        public List<Gesture> gestures = new List<Gesture>();

        // Use this for initialization
       protected  override void Start()
        {
            Init();
            FindObj();
            InitEvent();
        }

        private void Init()
        {
            transform.localScale = Vector3.zero;
        }
        private void FindObj()
        {
            Content =  FindTool.FindChildComponent<RectTransform>(transform, "Scroll View/Viewport/Content");
            addButton =  FindTool.FindChildComponent<ButtonBase>(transform, "Add");
            saveButton =  FindTool.FindChildComponent<ButtonBase>(transform, "Save");
            gestureWindows =  FindTool.FindChildComponent<GestureWindows>(transform, "GestureWindows");
            eventWindows =  FindTool.FindChildComponent<EventWindows>(transform, "EventWindows");
            selectionWindow =  FindTool.FindChildComponent<SelectionWindow>(transform, "SelectionWindow");
        }
        private void InitEvent()
        {
            addButton.OnClick.AddListener(AddGesture);
            saveButton.OnClick.AddListener(SaveGesture);

            gestureWindows.Recovery();
            eventWindows.Recovery();
            selectionWindow.Recovery();
            InitGestureData();
        }


        /// <summary>
        /// 恢复手势按钮信息
        /// </summary>
        /// <param name="gesture"></param>
        public void ReturnGestureButton(Gesture gesture)
        {
            for (int i = 0; i < gestures.Count; i++)
            {
                if (gestures[i] == gesture)
                    continue;
                gestures[i].gestureButton.Init();
            }
        }
        //初始化手势数据
        private void InitGestureData()
        {
            List <PlayerGestureData> gestureDatas = PlayerGestureManager.Instance.playerGestureDatas;
            for (int i = 0; i < gestureDatas.Count; i++)
            {
                Gesture g= AddGesture(gestureDatas[i]);//添加所有读到的手势
                if (g.playerGestureData.playerGestureInfo.isFindGesture)
                   PlayerManager.Instance.RecognitionGestureName = g.playerGestureData.playerGestureInfo.GestureName;//更新用于识别玩家的手势名字
                foreach (var item in gestureDatas[i].playerGestureEvents)
                {
                    PlayerManager.Instance.SetPlayerGestureEvent(item); //更新所有已添加的事件数据
                }
            }
            PlayerManager.Instance.onAddGestureEvent += (@event) =>
            {
                for (int i = 0; i < gestureDatas.Count; i++)
                {
                    foreach (var item in gestureDatas[i].playerGestureEvents)
                    {
                        if(item.eventName== @event.eventName&& item.eventDetailed == @event.eventDetailed)
                          PlayerManager.Instance.SetPlayerGestureEvent(item);//从查找当前手势是否存在自定义手势事件数组中
                    }
                }
            };//当添加手势事件时触发
        }

        //封装手势按钮天机方法，返回当前按钮
        private Gesture AddGesture(PlayerGestureData playerGesture)
        {
            Gesture gesture = SourcesManager.LoadSources<Gesture>("Gesture", Content);//在对象池中生成
            gestures.Add(gesture);
            gesture.Init(playerGesture, this);//初始化按钮
            gesture.gestureButton.OffChoice.AddListener(() =>
            {
                gestureWindows.Recovery();
                selectionWindow.Recovery();
                eventWindows.Recovery();
            });//添加按钮事件
            gesture.gestureButton.OnChoice.AddListener(() =>
            {
                gestureWindows.Recovery();
                eventWindows.Recovery();
                selectionWindow.SetPanel(gesture);
            });
            return gesture;
        }
        //保存手势数据
        private void SaveGesture()
        {
            transform.localScale = Vector3.zero;
            gestureWindows.Recovery();
            eventWindows.Recovery();
            selectionWindow.Recovery();
            isOn =false;

            PlayerGestureManager.Instance.SaveData();
        }
        //添加手势
        private void AddGesture()
        {
            selectionWindow.Recovery();
            eventWindows.Recovery();

            PlayerGestureData playerGesture = new PlayerGestureData();//实例化一个手势数据对象
            playerGesture.playerGestureInfo.GestureName = "GestureName"+gestures.Count;

            Gesture gesture= AddGesture(playerGesture);
            gesture.gestureButton.isChoice=true;
            ReturnGestureButton(gesture);
            gestureWindows.SetPanel(gesture,this);

            PlayerGestureManager.Instance.AddPlayerGestureData(playerGesture);
        }
        /// <summary>
        /// 删除手势
        /// </summary>
        /// <param name="gesture"></param>
        public void RemoveGesture(Gesture gesture)
        {
            PlayerGestureManager.Instance.RemovePlayerGestureData(gesture.playerGestureData);
            gestures.Remove(gesture);
            gesture.Recovery();
        }

        private bool isOn;
        // Update is called once per frame
        void Update()
        {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isOn = !isOn;
                    if (isOn)
                        transform.localScale = Vector3.one* 0.8191951f;
                    else
                    {
                        SaveGesture();
                        transform.localScale = Vector3.zero;
                    }
                }
        }
    }
}