using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTKinect
{
    [System.Serializable]
    public class PlayerGestureEventInfo
    {
        /// <summary>
        /// 事件名字
        /// </summary>
        public string eventName;
        /// <summary>
        ///玩家索引
        /// </summary>
        public int playerIndex;
        /// <summary>
        /// 手势名字
        /// </summary>
        public string gestureName;
        /// <summary>
        /// 事件的详细说明
        /// </summary>
        public string eventDetailed;

        /// <summary>
        ///手势事件回调
        /// </summary>
        internal Action gestureEvent
        {
            get;
            set;
        }
    }


    #region 数据对象

    [System.Serializable]
    public class PlayerGestureData
    {
        public PlayerGestureInfo playerGestureInfo = new PlayerGestureInfo();
        public List<PlayerGestureEventInfo> playerGestureEvents = new List<PlayerGestureEventInfo>();
    }
    [System.Serializable]
    public class PlayerGestureInfo
    {
        //手势名字
        public string GestureName;
        //手势时间
        public float timeGesture = 0.1f;
        //手势手势偏移
        public float offset = 10;
        ////重置偏移
        //public int resetOffset = 30;
        //是否是发现触发的手势
        public bool isFindGesture = false;
        //手势类型
        public GestureActionType gestureActionType;

        //结束动作的图像
        [HideInInspector]
        public _Texture2D intoPhotoBase64;
        //开始动作的图像
        [HideInInspector]
        public _Texture2D leavePhotoBase64;

        //是否开启右手臂
        public bool isOnRightHand = false;
        //是否开启左手臂
        public bool isOnLeftHand = false;
        //是否开启右脚
        public bool isOnRightFoot = false;
        //是否开启左脚
        public bool isOnLeftFoot = false;

        //是否右手臂主要识别
        public bool isRightHandMain = false;
        //是否左手臂主要识别
        public bool isLeftHandMain = false;
        //是否右脚主要识别
        public bool isRightFootMain = false;
        //是否左脚主要识别
        public bool isLeftFootMain = false;

        //对应关节结束数据
        public Dictionary<KinectInterop.JointType, PlayerGestureJointData> EndJointDirection = new Dictionary<KinectInterop.JointType, PlayerGestureJointData>();
        //对应关节开始数据
        public Dictionary<KinectInterop.JointType, PlayerGestureJointData> StartJointDirection = new Dictionary<KinectInterop.JointType, PlayerGestureJointData>();

        //右手关节点枚举
        private KinectInterop.JointType[] jointTypes_RightHand = new KinectInterop.JointType[]
        {
          KinectInterop.  JointType.ShoulderRight,
          KinectInterop.  JointType.ElbowRight
        };
        //左手关节点枚举
        private KinectInterop.JointType[] jointTypes_LeftHand = new KinectInterop.JointType[]
        {
           KinectInterop. JointType.ShoulderLeft,
           KinectInterop. JointType.ElbowLeft
        };
        //右脚关节点枚举
        private KinectInterop.JointType[] jointTypes_RightFoot = new KinectInterop.JointType[]
        {
          KinectInterop.  JointType.HipRight,
           KinectInterop. JointType.KneeRight
        };
        //左脚关节点枚举
        private KinectInterop.JointType[] jointTypes_LeftFoot = new KinectInterop.JointType[]
        {
           KinectInterop. JointType.HipLeft,
            KinectInterop.JointType.KneeLeft
        };

        /// <summary>
        /// 获取关节信息
        /// </summary>
        /// <returns></returns>
        public PlayerGestureJointData GetPlayerGestureJointData(GestureType gestureType,KinectInterop. JointType jointType)
        {
            PlayerGestureJointData playerGestureJoints = new PlayerGestureJointData();
            if (EndJointDirection.ContainsKey(jointType))

                switch (gestureType)
                {
                    case GestureType.End:
                        if (EndJointDirection.ContainsKey(jointType))
                            playerGestureJoints = EndJointDirection[jointType];
                        break;
                    case GestureType.Start:
                        if (StartJointDirection.ContainsKey(jointType))
                            playerGestureJoints = StartJointDirection[jointType];
                        break;
                    default:
                        break;
                }

            return playerGestureJoints;
        }

        /// <summary>
        /// 记录关节信息
        /// </summary>
        /// <param name="jointType"></param>
        /// <param name="Dir"></param>
        public void SetPlayerGestureJointData(GestureType gestureType, KinectInterop.JointType jointType, _Vector3 Dir)
        {
            PlayerGestureJointData playerGestureJoints = new PlayerGestureJointData();
            playerGestureJoints.direction = Dir;
            playerGestureJoints.jointType = jointType;

            switch (gestureType)
            {
                case GestureType.End:
                    if (EndJointDirection.ContainsKey(jointType))
                    {
                        EndJointDirection[jointType] = playerGestureJoints;
                    }
                    else
                    {
                        EndJointDirection.Add(jointType, playerGestureJoints);
                    }
                    break;
                case GestureType.Start:
                    if (StartJointDirection.ContainsKey(jointType))
                    {
                        StartJointDirection[jointType] = playerGestureJoints;
                    }
                    else
                    {
                        StartJointDirection.Add(jointType, playerGestureJoints);
                    }
                    break;
                default:
                    break;
            }
        }


        public enum GestureType
        {
            End,
            Start
        }
    }
    //手势关节信息
    [System.Serializable]
    public class PlayerGestureJointData
    {
        public KinectInterop.JointType jointType;
        public _Vector3 direction = new _Vector3(Vector3.zero);
    }
    [System.Serializable]

    public enum GestureActionType
    {
        静态,
        动态
    }
    #endregion
}