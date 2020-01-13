using MTFrame.MTKinect;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTKinect
{
    [System.Serializable]
    public class PlayerGesture
    {
        public PlayerGestureData[] playerGestureDatas;
        public long userID { get; internal set; }
        public int index { get; internal set; }

        public GestureJudgeData[] gestureJudgeData;

        public void Init(List<PlayerGestureData> _playerGestureDatas)
        {
            playerGestureDatas = new PlayerGestureData[_playerGestureDatas.Count];
            Array.Copy(_playerGestureDatas.ToArray(), playerGestureDatas, _playerGestureDatas.Count);

            gestureJudgeData = new GestureJudgeData[_playerGestureDatas.Count];
            for (int i = 0; i < gestureJudgeData.Length; i++)
            {
                gestureJudgeData[i] = new GestureJudgeData();
            }
        }

        private Vector3 LastOnePos;
        public void OnUpdect(
            KinectInterop. JointType[] FootLeftJoints,
             KinectInterop.JointType[] FootRightJoints,
             KinectInterop.JointType[] HandLeftJoints,
           KinectInterop.JointType[] HandRightJoints,
            Action<PlayerGestureData> OnCall)
        {
            Vector3 pos = KINECTManager.Instance.GetUserIDJointPos(userID, KinectInterop.JointType.Head,Camera.main, new Rect(0, 0, 1920, 1080));
            float dis = Vector3.Distance(LastOnePos, pos);
            if (dis > 0.15f || dis == 0)
            {
                LastOnePos = pos;
                return;
            }

            for (int i = 0; i < playerGestureDatas.Length; i++)//循环所有手势
            {
                List<int> bs = new List<int>();
                if (playerGestureDatas[i].playerGestureInfo.isOnLeftFoot)
                {
                    if (gestureJudgeData[i].isComplete)
                    {
                        if (!playerGestureDatas[i].playerGestureInfo.isLeftFootMain)
                            bs.Add(1);
                        else
                            bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], FootLeftJoints));
                    }
                    else
                        bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], FootLeftJoints));
                }

                if (playerGestureDatas[i].playerGestureInfo.isOnRightFoot)
                {

                    if (gestureJudgeData[i].isComplete)
                    {
                        if (!playerGestureDatas[i].playerGestureInfo.isRightFootMain)
                            bs.Add(1);
                        else
                            bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], FootRightJoints));
                    }
                    else
                        bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], FootRightJoints));
                }

                if (playerGestureDatas[i].playerGestureInfo.isOnLeftHand)
                {
                    if (gestureJudgeData[i].isComplete)
                    {
                        if (!playerGestureDatas[i].playerGestureInfo.isLeftHandMain)
                            bs.Add(1);
                        else
                            bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], HandLeftJoints));
                    }
                    else
                        bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], HandLeftJoints));
                }

                if (playerGestureDatas[i].playerGestureInfo.isOnRightHand)
                {
                    if (gestureJudgeData[i].isComplete)
                    {
                        if (!playerGestureDatas[i].playerGestureInfo.isRightHandMain)
                            bs.Add(1);
                        else
                            bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], HandRightJoints));
                    }
                    else
                        bs.Add(GetJointPos(gestureJudgeData[i], playerGestureDatas[i], HandRightJoints));
                }

                int isValue = 0;

                if (bs.Contains(0))
                    isValue = 0;
                else if (!bs.Contains(0) && !bs.Contains(2))
                    isValue = 1;
                else if (!bs.Contains(0) && !bs.Contains(1))
                    isValue = 2;
                if (isValue == 0)
                {
                    gestureJudgeData[i].gestureTime = 0;
                    continue;
                }

                if (playerGestureDatas[i].playerGestureInfo.gestureActionType == GestureActionType.动态)
                {
                    if (isValue == 2 && !gestureJudgeData[i].isComplete)
                    {
                        gestureJudgeData[i].gestureTime += (1f / PlayerGestureManager.Instance.FPS);
                        if (gestureJudgeData[i].gestureTime <= playerGestureDatas[i].playerGestureInfo.timeGesture)
                        {
                            gestureJudgeData[i].isComplete = true;
                            OnCall?.Invoke(playerGestureDatas[i]);
                        }
                    }
                    else if (isValue == 1 && gestureJudgeData[i].isComplete)
                    {
                        gestureJudgeData[i].gestureTime = 0;
                        gestureJudgeData[i].isComplete = false;
                    }
                }
                else
                {
                    if (isValue == 2 && !gestureJudgeData[i].isComplete)
                    {
                        gestureJudgeData[i].gestureTime += (1f / PlayerGestureManager.Instance.FPS);
                        if (gestureJudgeData[i].gestureTime >= playerGestureDatas[i].playerGestureInfo.timeGesture)
                        {
                            gestureJudgeData[i].isComplete = true;
                            OnCall?.Invoke(playerGestureDatas[i]);
                        }
                    }
                    else if (isValue == 1 && gestureJudgeData[i].isComplete)
                    {
                        gestureJudgeData[i].gestureTime = 0;
                        gestureJudgeData[i].isComplete = false;
                    }
                }
            }


            LastOnePos = pos;
        }

        List<int> gesturesValue = new List<int>();
        private int GetJointPos(GestureJudgeData gestureJudge, PlayerGestureData playerGestureData, KinectInterop.JointType[] jointTypes)
        {
            for (int i = 0; i < jointTypes.Length; i++)
            {
                if (!KinectManager.Instance.IsInitialized())
                    return 0;
            }
            gesturesValue.Clear();
            for (int i = 0; i < 2; i++)
            {
                Vector3 pos1 = KINECTManager.Instance.GetUserIDJointPos(userID, jointTypes[i], Camera.main, new Rect(0, 0, 1920, 1080));//实时关节位置
                Vector3 pos2 = KINECTManager.Instance.GetUserIDJointPos(userID, jointTypes[i + 1], Camera.main, new Rect(0, 0, 1920, 1080));//实时关节位置

                _Vector3 dir = new _Vector3((pos2 - pos1).normalized);

                PlayerGestureJointData EndPlayerGestures = playerGestureData.playerGestureInfo.GetPlayerGestureJointData(
                    PlayerGestureInfo.GestureType.End, jointTypes[i]);                                                     //获取本地储存结束的关节数据
                PlayerGestureJointData StartPlayerGestures = playerGestureData.playerGestureInfo.GetPlayerGestureJointData(
                    PlayerGestureInfo.GestureType.Start, jointTypes[i]);                                                    //获取本地储存开始的关节数据

                float EndAngle = Vector3.Angle(EndPlayerGestures.direction.Get(), dir.Get());
                float StartAngle = Vector3.Angle(StartPlayerGestures.direction.Get(), dir.Get());

                if (EndAngle <= playerGestureData.playerGestureInfo.offset)
                {
                    gesturesValue.Add( 2);
                }
                else if (StartAngle <= playerGestureData.playerGestureInfo.offset)
                {
                    gesturesValue.Add(1);
                }
                else
                {
                    gesturesValue.Add(0);
                }
            }

            if (!gesturesValue.Contains(0) && !gesturesValue.Contains(1))
                return 2;
            else if (!gesturesValue.Contains(0) && !gesturesValue.Contains(2))
                return 1;
            else return 0;
        }
    }
    public class GestureJudgeData
    {
        public bool isComplete = false;
        public float gestureTime = 0;
    }
}