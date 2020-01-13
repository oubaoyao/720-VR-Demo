using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MTFrame.MTFile;
using System.IO;
using MTFrame.MTKinect;
using MTFrame.JsonTool;

namespace MTFrame.MTKinect
{
    // 委托
    public delegate void PlayerGestureEvent(long userid, string gesture);

    public class PlayerGestureManager : MainBehavior, ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>
                {
                    "读取",
                    "保存"
                };
            }
        }
        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>
                {
                    ReadData,
                    SaveData
                };
            }
        }

        public static PlayerGestureManager Instance;
        protected override void Awake()
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            ReadData();

           TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact,1f/FPS, OnGesturesUpdect);
        }
        
        protected override void OnEnable()
        {
            TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 1f / FPS, OnGesturesUpdect, true);
            KINECTManager.Instance.FindUser += AddPlayerGesture;
            KINECTManager.Instance.LoseUser += RemovePlayerGesture;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact,OnGesturesUpdect);
            KINECTManager.Instance.FindUser -= AddPlayerGesture;
            KINECTManager.Instance.LoseUser -= RemovePlayerGesture;
            base.OnDisable();
        }

        /// <summary>
        /// 更新频率
        /// </summary>
        [Header("更新频率")]
        public int FPS = 30;
        /// <summary>
        /// 是否开启设置面板
        /// </summary>
        [Header("是否开启设置面板")]
        [SerializeField]
        private bool isOpenSetGesturePanel;
        /// <summary>
        /// 手势事件回调事件
        /// </summary>
        public event PlayerGestureEvent onPlayerGestureEvent;
        /// <summary>
        /// 手势数据集合
        /// </summary>
        public List<PlayerGestureData> playerGestureDatas = new List<PlayerGestureData>();


        //本地骨骼数据地址
        public string path;

        //识别的所有玩家
        private Dictionary<long, PlayerGesture> playerGestures = new Dictionary<long, PlayerGesture>();
        public List<PlayerGesture> _playerGestures = new List<PlayerGesture>();


        #region 固定的四肢关节数组
        //固定的四肢关节数组
        private KinectInterop.JointType[] HandRightJoints = new KinectInterop.JointType[]
        {
             KinectInterop.  JointType.ShoulderRight,
             KinectInterop.  JointType.ElbowRight,
             KinectInterop.  JointType.HandRight,
        };
        private KinectInterop.JointType[] HandLeftJoints = new KinectInterop.JointType[]
        {
             KinectInterop.  JointType.ShoulderLeft,
            KinectInterop.   JointType.ElbowLeft,
              KinectInterop. JointType.HandLeft
        };
        private KinectInterop.JointType[] FootRightJoints = new KinectInterop.JointType[]
        {
              KinectInterop. JointType.HipRight,
              KinectInterop. JointType.KneeRight,
             KinectInterop.  JointType.FootRight
        };
        private KinectInterop.JointType[] FootLeftJoints = new KinectInterop.JointType[]
        {
            KinectInterop.JointType.HipLeft,
              KinectInterop. JointType.KneeLeft,
               KinectInterop.JointType.FootLeft
        };
        #endregion

        /// <summary>
        /// 读取数据
        /// </summary>
        public void ReadData()
        {
            path = Application.streamingAssetsPath + "/GestureData/GestureName/GestureName.txt";
           FileManager.ReadWeb(path,(fileObject) =>
           {
               SetInfo(fileObject);
           }, FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);
        }

        private void SetInfo(FileObject fileObject)
        {
            playerGestureDatas.Clear();
            string json = ASCIIEncoding.UTF8.GetString(fileObject.Buffet);
            if (JsonCheckTool.IsJson(json))
                playerGestureDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlayerGestureData>>(json);

            if (isOpenSetGesturePanel)
            {
                Canvas canvas = new GameObject("[SetCanvas]").AddComponent<Canvas>();
                GameObject.DontDestroyOnLoad(canvas.gameObject);
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<CanvasScaler>();
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                canvas.sortingOrder = 100;
                if (!FindObjectOfType<EventSystem>())
                {
                    EventSystem eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                    eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                }
                SourcesManager.LoadSources<SetGesturePanel>("SetGesturePanel", canvas.transform);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveData()
        {
            string data= Newtonsoft.Json.JsonConvert.SerializeObject(playerGestureDatas);
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(data);
            FileManager.Write(path, buffer, FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);

            foreach (var playerGesture in playerGestures)
            {
                playerGesture.Value.Init(playerGestureDatas);
            }
        }



        #region 手势识别

        //添加用户
        public void AddPlayerGesture(long userid)
        {
            if (playerGestures.Count >= PlayerManager.Instance. UserCount)
                return;
            if (playerGestures.ContainsKey(userid))
                return;

                PlayerGesture playerGesture = new PlayerGesture();
                playerGesture.userID = userid;
                playerGesture.index = playerGestures.Count;
                playerGesture.Init(playerGestureDatas);

                playerGestures.Add(userid, playerGesture);
                _playerGestures.Add(playerGesture); 
        }
        //删除用户
        public void RemovePlayerGesture(long userid)
        {
            if (playerGestures.ContainsKey(userid))
            {
                _playerGestures.Remove(playerGestures[userid]);
                playerGestures.Remove(userid);
            }
            GC.Collect();
        }
        //开启更新
        private void OnGesturesUpdect()
        {
            foreach (var playerGesture in playerGestures)
            {
                playerGesture.Value.OnUpdect( FootLeftJoints, FootRightJoints, HandLeftJoints, HandRightJoints, (playerGestureData) =>
                { onPlayerGestureEvent?.Invoke(playerGesture.Value.userID, playerGestureData.playerGestureInfo.GestureName); });//事件触发
            }
        }
        #endregion


        #region 手势信息录入

        /// <summary>
        ///录入关节信息
        /// </summary>
        /// <param name="playerGesture"></param>
        public void SetGestureJointData(PlayerGestureInfo.GestureType gestureType, PlayerGestureData playerGesture)
        {
            SetGestureJointPos(gestureType, playerGesture, HandRightJoints);
            SetGestureJointPos(gestureType, playerGesture, HandLeftJoints);
            SetGestureJointPos(gestureType, playerGesture, FootRightJoints);
            SetGestureJointPos(gestureType, playerGesture, FootLeftJoints);
            byte[] buff = KINECTManager.Instance.GetOrbbecImage(KinectImageType.colour).ScaleTexture(192,108).EncodeToJPG(10);

            Texture2D texture2D=  new Texture2D(0, 0);
            texture2D.LoadImage(buff);
            texture2D.Apply();

            switch (gestureType)
            {
                case PlayerGestureInfo.GestureType.End:
                    playerGesture.playerGestureInfo.intoPhotoBase64 =new _Texture2D(texture2D);//保存当前图像成Base64作为识别手势的标准
                    break;
                case PlayerGestureInfo.GestureType.Start:
                    playerGesture.playerGestureInfo.leavePhotoBase64 = new _Texture2D(texture2D);//保存当前图像成Base64作为识别手势的标准
                    break;
                default:
                    break;
            }

            GC.Collect();
            Resources.UnloadUnusedAssets();//内存释放
        }
        //封装设置关节方向方法，
        private void SetGestureJointPos(PlayerGestureInfo.GestureType gestureType, PlayerGestureData playerGesture, KinectInterop.JointType[] jointTypes)
        {
            for (int i = 0; i < jointTypes.Length - 1; i++)
            {
                Vector3 pos1 = KINECTManager.Instance.GetIndexJointPos(0, jointTypes[i], Camera.main,new Rect(0,0,1920,1080));
                Vector3 pos2 = KINECTManager.Instance.GetIndexJointPos(0, jointTypes[i + 1], Camera.main,new Rect(0, 0, 1920, 1080));
                _Vector3 dis = new _Vector3((pos2 - pos1).normalized);

                playerGesture.playerGestureInfo.SetPlayerGestureJointData(gestureType, jointTypes[i], dis);
            }
        }
        /// <summary>
        /// 添加手势数据
        /// </summary>
        /// <param name="playerGesture"></param>
        public void AddPlayerGestureData(PlayerGestureData playerGesture)
        {
            playerGestureDatas.Add(playerGesture);
        }

        /// <summary>
        /// 删除手势数据
        /// </summary>
        /// <param name="playerGesture"></param>
        public void RemovePlayerGestureData(PlayerGestureData playerGesture)
        {
            playerGestureDatas.Remove(playerGesture);
        }
        /// <summary>
        /// 恢复定义发现玩家手势选项，发现玩家只能用一种手势，但第一了其他手势为识别玩家时，其他的手势更新为默认False
        /// </summary>
        /// <param name="playerGesture"></param>
        public void ReturnFindGesture(PlayerGestureData playerGesture)
        {
            foreach (var item in playerGestureDatas)
            {
                if (item != playerGesture)
                    item.playerGestureInfo.isFindGesture = false;
            }
            PlayerManager.Instance.RecognitionGestureName = playerGesture.playerGestureInfo.GestureName;
        }

        /// <summary>
        /// 恢复手势关联的事件，每个事件只能在一个手势上出现，此方法就是更新手势触发的事件
        /// （注释：在手势添加事件时调用）
        /// </summary>
        /// <param name="playerGestureEventInfo"></param>
        public void ReturnGestureEvent(PlayerGestureEventInfo playerGestureEventInfo)
        {
            for (int i = 0; i < playerGestureDatas.Count; i++)
            {
                for (int j = 0; j < playerGestureDatas[i].playerGestureEvents.Count; j++)//循环整个手势数组上的所有事件
                {
                    PlayerGestureEventInfo playerGestureEvent = playerGestureDatas[i].playerGestureEvents[j];
                    if (playerGestureEvent.eventName == playerGestureEventInfo.eventName && playerGestureEvent.eventDetailed == playerGestureEventInfo.eventDetailed)
                    {
                        playerGestureDatas[i].playerGestureEvents.Remove(playerGestureEvent);
                        return;
                    }
                }
            }
        }
#endregion


    }

}