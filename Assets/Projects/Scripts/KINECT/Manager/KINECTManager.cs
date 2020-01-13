
using System;
using UnityEngine;
using static InteractionManager;

namespace MTFrame.MTKinect
{
    /// <summary>
    /// 奥比中关管理器
    /// </summary>
    public class KINECTManager
    {
        private static KINECTManager instance;
        public static KINECTManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new KINECTManager();
                return instance;
            }
        }

        public Action<long> FindUser;
        public Action<long> LoseUser;

        public int UserCount = 6;

        private BackgroundRemovalManager backManager;
        private Texture2D texture2D = new Texture2D(0, 0);
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            KinectManager kinectManager = new GameObject("[KinectManager]").AddComponent<KinectManager>();
            kinectManager.gameObject.AddComponent<PortraitBackground>();
            kinectManager.gameObject.AddComponent<InteractionManager>(); 
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            KinectManager.Instance?.OpenKinect();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            KinectManager.Instance?.CloseKinect();
        }


        public HandEventType GetHandState(HandType handType)
        {
            HandEventType handEventType= HandEventType.None;
            if (!InteractionManager.Instance.IsInteractionInited())
                return handEventType;
            else
            {
                switch (handType)
                {
                    case HandType.Left:
                        handEventType = InteractionManager.Instance.GetLastLeftHandEvent();
                        break;
                    case HandType.Right:
                        handEventType = InteractionManager.Instance.GetLastRightHandEvent();
                        break;
                    default:
                        break;
                }
            }
            return handEventType;
        }


        #region 公开调用方法

        /// <summary>
        /// 获取实时图像
        /// </summary>
        /// <param name="orbbecImageType"></param>
        /// <returns></returns>
        public Texture2D GetOrbbecImage(KinectImageType orbbecImageType)
        {
            switch (orbbecImageType)
            {
                case KinectImageType.colour:
                    if (KinectManager.Instance.IsInitialized())
                        return KinectManager.Instance.GetUsersClrTex();
                    break;
                case KinectImageType.depthmask:
                    if (KinectManager.Instance.IsInitialized())
                        return KinectManager.Instance.GetUsersLblTex();
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// 获取实时图像
        /// </summary>
        /// <param name="orbbecImageType"></param>
        /// <returns></returns>
        public Texture GetOrbbecImage()
        {
            if (backManager == null)
                backManager = KinectManager.Instance.gameObject.AddComponent<BackgroundRemovalManager>();
            return backManager.GetForegroundTex();
        }


        #region 获取关节位置

        /// <summary>
        /// 根据用户索引获取关节位置
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetIndexJointPos(int userIndex, KinectInterop.JointType jointType)
        {
            if (!KinectManager.Instance.IsInitialized())
                return Vector3.zero;
            if (!KinectManager.Instance.IsUserDetected())
                return Vector3.zero;

            long userID = KinectManager.Instance.GetUserIdByIndex(userIndex);
            Vector3 pos = KinectManager.Instance.GetJointColorMapPos(userID, (int)jointType);
            return pos;
        }

        /// <summary>
        /// 根据用户索引获取关节位置
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetIndexJointPos(int userIndex, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            if (!KinectManager.Instance.IsInitialized())
                return Vector3.zero;
            if (!KinectManager.Instance.IsUserDetected())
                return Vector3.zero;
            long userID = KinectManager.Instance.GetUserIdByIndex(userIndex);
            Vector3 pos = KinectManager.Instance.GetJointPosColorOverlay(userID, (int)jointType, camera, offsetRect);
            return pos;
        }

        /// <summary>
        /// 根据用户ID获取关节位置
        /// </summary>
        /// <param name="userIndex"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetUserIDJointPos(long userID, KinectInterop.JointType jointType)
        {
            if (!KinectManager.Instance.IsInitialized())
                return Vector3.zero;
            if (!KinectManager.Instance.IsUserDetected())
                return Vector3.zero;

            Vector3 pos = KinectManager.Instance.GetJointColorMapPos(userID, (int)jointType);
            return pos;
        }
        /// <summary>
        /// 根据用户ID获取关节位置
        /// </summary>
        /// <param name="userIndex"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector3 GetUserIDJointPos(long userID, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            if (!KinectManager.Instance.IsInitialized())
                return Vector3.zero;
            if (!KinectManager.Instance.IsUserDetected())
                return Vector3.zero;
            Vector3 pos = KinectManager.Instance.GetJointPosColorOverlay(userID, (int)jointType, camera, offsetRect);
            return pos;
        }


        /// <summary>
        /// 根据用户ID获取关节2D位置
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetIndexJointPos2D(int userIndex, KinectInterop.JointType jointType)
        {
            Vector3 pos = GetIndexJointPos(userIndex, jointType);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户ID获取关节2D位置
        /// </summary>
        /// <param name="userIndex">限制0~5</param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetIndexJointPos2D(int userIndex, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            Vector3 pos = GetIndexJointPos(userIndex, jointType, camera, offsetRect);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户ID获取关节2D位置
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetUserIDJointPos2D(long userID, KinectInterop.JointType jointType)
        {
            Vector3 pos = GetUserIDJointPos(userID, jointType);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }

        /// <summary>
        /// 根据用户ID获取关节2D位置
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="jointType"></param>
        /// <returns></returns>
        public Vector2 GetUserIDJointPos2D(long userID, KinectInterop.JointType jointType, Camera camera, Rect offsetRect)
        {
            Vector3 pos = GetUserIDJointPos(userID, jointType, camera, offsetRect);
            Vector2 pos2D = Camera.main.WorldToScreenPoint(pos);
            return pos2D;
        }
        #endregion


        /// <summary>
        /// 根据索引获取用户ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetUserIdByIndex(int userIndex)
        {
            long userID = KinectManager.Instance.GetUserIdByIndex(userIndex);
            return userID;
        }

        /// <summary>
        /// 获取主要用户ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetMainUserIdByIndex()
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            return userID;
        }

        #endregion


    }

    /// <summary>
    /// 奥比图像类型
    /// </summary>
    public enum KinectImageType
    {
        /// <summary>
        /// 彩色
        /// </summary>
        colour,
        /// <summary>
        /// 深度遮罩
        /// </summary>
        depthmask,
    }

    public enum HandType
    {
        Left,
        Right
    }

}