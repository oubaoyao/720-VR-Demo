using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTKinect
{
    public delegate void OnAddGestureEvent(PlayerGestureEventInfo playerGestureEvent);
    public class PlayerManager : MainBehavior, ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>
                {
                    "Init",
                    "InitGesture"

                };
            }
        }
        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>
                {
                    Init,
                    InitGesture
                };
            }
        }

        private void InitGesture()
        {
            throw new NotImplementedException();
        }

        private void Init()
        {
            name = "[PlayerManager]";
        }

        public static PlayerManager Instance;
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            KINECTManager.Instance.FindUser += AddPlayer;
            KINECTManager.Instance.LoseUser += RemovePlayer;
            TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 1f / FPS, OnJointUpdate, true);
            PlayerGestureManager.Instance.onPlayerGestureEvent += OnUpdatePlayerGestureEvent;
            KINECTManager.Instance.UserCount = UserCount;
        }
        protected override void OnDisable()
        {
            KINECTManager.Instance.FindUser -= AddPlayer;
            KINECTManager.Instance.LoseUser -= RemovePlayer;
            TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, OnJointUpdate);
            PlayerGestureManager.Instance.onPlayerGestureEvent -= OnUpdatePlayerGestureEvent;
            base.OnDisable();
        }


        /// <summary>
        /// 用户数量
        /// </summary>
        [Header("用户数量")]
        public int UserCount = 6;
        /// <summary>
        /// 更新频率
        /// </summary>
        [Header("更新频率")]
        public int FPS = 30;
        /// <summary>
        /// 屏幕像素
        /// </summary>
        [Header("屏幕像素")]
        public Vector2 ScreenSize = new Vector2(1920, 1080);
        /// <summary>
        /// 是否开启识别手势
        /// </summary>
        [Header("是否开启识别手势")]
        public bool isGestureRecognitionAdd;
        /// <summary>
        /// 识别手势名字
        /// </summary>
        [Header("识别手势名字")]
        public string RecognitionGestureName;

        //手势回调事件
        public event PlayerGestureEvent onPlayerGestureEvent;
        /// <summary>
        /// 发现玩家
        /// </summary>
        public Action<Player> onFindPlayerEvent;
        /// <summary>
        /// 丢失玩家
        /// </summary>
        public Action<Player> onLosePlayerEvent;

        /// <summary>
        /// 添加主要玩家时
        /// </summary>
        public Action<Player> onFindPrimarilyPlayerEvent;
        /// <summary>
        /// 丢失主要玩家时
        /// </summary>
        public Action<Player> onLosePrimarilyPlayerEvent;


        public event OnAddGestureEvent onAddGestureEvent;

        //主要的玩家
        private Player primarily;
        //玩家字典
        private List<Player> players = new List<Player>();
        //物体对象跟踪事件字典
        private Dictionary<JointTrackType, Dictionary<int, Dictionary<KinectInterop.JointType, List<Transform>>>> TrackEvent = new Dictionary<JointTrackType, Dictionary<int, Dictionary<KinectInterop.JointType, List<Transform>>>>();
        //玩家手势事件
        public List<PlayerGestureEventInfo> gestureEvents = new List<PlayerGestureEventInfo>();



        #region Player 常规处理
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        private void AddPlayer(long userID)
        {
            if (players.Count >= UserCount)
                return;
            Player player = new Player(userID, players.Count);
            players.Add(player);
            onFindPlayerEvent?.Invoke(player);
            if (!isGestureRecognitionAdd)
                player.ActivatePlayer();
            Debug.Log("添加玩家==" + userID);
        }
        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        public void RemovePlayer(long userID)
        {
            Player player = players.Find(p => p.UserID == userID);
            if (player != null)
            {
                players.Remove(player);
                onLosePlayerEvent?.Invoke(player);
                Debug.Log("移除玩家==" + userID);
                if (primarily != null)
                    if (primarily == player)
                    {
                        onLosePrimarilyPlayerEvent?.Invoke(primarily);
                        primarily = null;
                    }
                SetPlauer();
            }
        }

        /// <summary>
        /// 关闭玩家
        /// </summary>
        /// <param name="userid"></param>
        public void ClosePlayer(long userID)
        {
            Player player = players.Find(p => p.UserID == userID);
            if (player != null)
            {
                player.StopPlayer();
            }
        }
        /// <summary>
        /// 关闭所有玩家
        /// </summary>
        public void CloseAllPlayer()
        {
            foreach (var item in players)
            {
                item.StopPlayer();
            }
        }

        public void SetPlauer()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].PlayerIndex = i;
            }
        }

        /// <summary>
        /// 获取主要玩家
        /// </summary>
        /// <returns></returns>
        public Player GetPrimaryPlay()
        {
            if (primarily == null)
            {
                if (!isGestureRecognitionAdd)
                    if (players.Count > 0)
                        primarily = players[0];
            }
            return primarily;
        }

        /// <summary>
        /// 根据索引获取玩家
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Player GetPlayer(int index)
        {
            if (players.Count > index)
                return players[index];
            else
                return null;
        }

        #endregion


        #region 物体跟踪事件处理
        /// <summary>
        ///添加跟踪事件
        /// </summary>
        /// <param name="playerIndex">玩家索引</param>
        /// <param name="jointType">跟踪关节</param>
        /// <param name="trackObj">物体</param>
        public void AddTrackEvent(JointTrackType jointTrackType, int playerIndex, KinectInterop.JointType jointType, Transform trackObj)
        {
            if (TrackEvent.ContainsKey(jointTrackType))
            {
                if (TrackEvent[jointTrackType].ContainsKey(playerIndex))
                {
                    if (TrackEvent[jointTrackType][playerIndex].ContainsKey(jointType))
                    {
                        TrackEvent[jointTrackType][playerIndex][jointType].Add(trackObj);
                    }
                    else
                    {
                        List<Transform> list_T = new List<Transform>();
                        list_T.Add(trackObj);
                        TrackEvent[jointTrackType][playerIndex].Add(jointType, list_T);
                    }
                }
                else
                {
                    Dictionary<KinectInterop.JointType, List<Transform>> dic_J = new Dictionary<KinectInterop.JointType, List<Transform>>();
                    List<Transform> list_T = new List<Transform>();
                    list_T.Add(trackObj);
                    dic_J.Add(jointType, list_T);
                    TrackEvent[jointTrackType].Add(playerIndex, dic_J);
                }
            }
            else
            {
                Dictionary<int, Dictionary<KinectInterop.JointType, List<Transform>>> dic_I = new Dictionary<int, Dictionary<KinectInterop.JointType, List<Transform>>>();
                Dictionary<KinectInterop.JointType, List<Transform>> dic_J = new Dictionary<KinectInterop.JointType, List<Transform>>();
                List<Transform> list_T = new List<Transform>();
                list_T.Add(trackObj);
                dic_J.Add(jointType, list_T);
                dic_I.Add(playerIndex, dic_J);
                TrackEvent.Add(jointTrackType, dic_I);
                Debug.Log(TrackEvent.ContainsKey(JointTrackType._3D));
            }
        }
        /// <summary>
        ///删除跟踪事件
        /// </summary>
        /// <param name="index">玩家索引</param>
        /// <param name="jointType">跟踪关节</param>
        /// <param name="transform">物体</param>
        public void RemoveTrackEvent(JointTrackType jointTrackType, int playerIndex, KinectInterop.JointType jointType, Transform trackObj)
        {
            if (TrackEvent.ContainsKey(jointTrackType))
            {
                if (TrackEvent[jointTrackType].ContainsKey(playerIndex))
                {
                    if (TrackEvent[jointTrackType][playerIndex].ContainsKey(jointType))
                    {
                        TrackEvent[jointTrackType][playerIndex][jointType].Remove(trackObj);
                    }
                }
            }
        }
        /// <summary>
        ///删除玩家关节下所有跟踪事件
        /// <param name="index">玩家索引</param>
        /// <param name="jointType">跟踪关节</param>
        public void RemoveTrackEvent(JointTrackType jointTrackType, int playerIndex, KinectInterop.JointType jointType)
        {
            if (TrackEvent.ContainsKey(jointTrackType))
            {
                if (TrackEvent[jointTrackType].ContainsKey(playerIndex))
                {
                    if (TrackEvent[jointTrackType][playerIndex].ContainsKey(jointType))
                    {
                        TrackEvent[jointTrackType][playerIndex].Remove(jointType);
                    }
                }
            }
        }
        /// <summary>
        ///删除玩家索引下所有跟踪事件
        /// </summary>
        /// <param name="index">玩家索引</param>
        public void RemoveTrackEvent(JointTrackType jointTrackType, int playerIndex)
        {
            if (TrackEvent.ContainsKey(jointTrackType))
            {
                if (TrackEvent[jointTrackType].ContainsKey(playerIndex))
                {
                    TrackEvent[jointTrackType].Remove(playerIndex);
                }
            }
        }

        /// <summary>
        /// 关节更新
        /// </summary>
        /// <param name="fps"></param>
        private void OnJointUpdate()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].isActivate)
                    return;
                if (TrackEvent.ContainsKey(JointTrackType._2D))
                {
                    if (TrackEvent[JointTrackType._2D].ContainsKey(players[i].PlayerIndex))
                    {
                        foreach (var jointType in TrackEvent[JointTrackType._2D][players[i].PlayerIndex])
                        {
                            foreach (var trackObj in jointType.Value)
                            {
                                trackObj.localPosition = KINECTManager.Instance.GetUserIDJointPos2D(players[i].UserID, jointType.Key, Camera.main, new Rect(0, 0, ScreenSize.x, ScreenSize.y));
                            }
                        }
                    }
                }

                if (TrackEvent.ContainsKey(JointTrackType._3D))
                {
                    if (TrackEvent[JointTrackType._3D].ContainsKey(players[i].PlayerIndex))
                    {
                        foreach (var jointType in TrackEvent[JointTrackType._3D][players[i].PlayerIndex])
                        {
                            foreach (var trackObj in jointType.Value)
                            {
                                trackObj.localPosition = KINECTManager.Instance.GetUserIDJointPos(players[i].UserID, jointType.Key, Camera.main, new Rect(0, 0, ScreenSize.x, ScreenSize.y));
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region 手势管理
        /// <summary>
        /// 添加手势事件
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="gestureName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventDetailed"></param>
        /// <param name="gestureEvent"></param>
        public void AddPlayerGestureEvent(int playerIndex, string gestureName, string eventName, string eventDetailed, Action gestureEvent)
        {
            PlayerGestureEventInfo eventInfo = new PlayerGestureEventInfo();
            eventDetailed += string.Format("\n\n(初始玩家事件-{0})", playerIndex);
            eventInfo.gestureName = gestureName;
            eventInfo.playerIndex = playerIndex;
            eventInfo.eventName = eventName;
            eventInfo.eventDetailed = eventDetailed;
            eventInfo.gestureEvent = gestureEvent;
            gestureEvents.Add(eventInfo);

            onAddGestureEvent?.Invoke(eventInfo);
        }
        /// <summary>
        /// 删除手势事件
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="gestureName"></param>
        /// <param name="eventName"></param>
        public void RemovePlayerGestureEvent(int playerIndex, string gestureName, string eventName)
        {
            PlayerGestureEventInfo eventInfo = new PlayerGestureEventInfo();
            for (int i = 0; i < gestureEvents.Count; i++)
            {
                if (gestureEvents[i].playerIndex == playerIndex && gestureEvents[i].gestureName == gestureName)
                    eventInfo = gestureEvents[i];
            }
            gestureEvents.Remove(eventInfo);
        }
        /// <summary>
        /// 设置更新事件数据
        /// （注释：对应更新事件触发的玩家索引和手势名字）
        /// </summary>
        /// <param name="gestureEventInfo"></param>
        public void SetPlayerGestureEvent(PlayerGestureEventInfo gestureEventInfo)
        {
            for (int i = 0; i < gestureEvents.Count; i++)
            {
                if (gestureEvents[i].eventName == gestureEventInfo.eventName && gestureEvents[i].eventDetailed == gestureEventInfo.eventDetailed)
                {
                    gestureEvents[i].playerIndex = gestureEventInfo.playerIndex;
                    gestureEvents[i].gestureName = gestureEventInfo.gestureName;
                }
            }
        }
        /// <summary>
        /// 更新手势事件
        /// </summary>
        /// <param name="Gesture"></param>
        /// <param name="player"></param>
        public void OnUpdatePlayerGestureEvent(long userid, string Gesture)
        {
            onPlayerGestureEvent?.Invoke(userid, Gesture);
            print(userid + "==" + Gesture);
            Player player = players.Find(p => p.UserID == userid);


            if (player != null)
            {
                if (isGestureRecognitionAdd)
                {
                    if (Gesture == RecognitionGestureName)
                    {
                        if (!player.isActivate)
                        {
                            player.ActivatePlayer();
                            if (primarily == null)
                            {
                                primarily = player;
                                onFindPrimarilyPlayerEvent?.Invoke(primarily);
                            }
                        }
                    }
                }
                if (player.isActivate)
                {
                    for (int i = 0; i < gestureEvents.Count; i++)
                    {
                        if (gestureEvents[i].playerIndex == player.PlayerIndex)
                        {
                            if (gestureEvents[i].gestureName == Gesture)
                            {
                                gestureEvents[i].gestureEvent?.Invoke();
                            }

                        }
                    }
                }
            }
        }

        #endregion
    }
    public enum JointTrackType
    {
        _2D,
        _3D
    }
}