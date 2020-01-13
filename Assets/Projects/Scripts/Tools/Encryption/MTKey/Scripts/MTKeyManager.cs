using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MT.Key
{
    /// <summary>
    /// 网络加密
    /// </summary>
    public class MTKeyManager : MonoBehaviour
    {
        private static MTKeyManager instance;
        private static bool isMTKeyManager = false;

        public static MTKeyManager Instance
        {
            get
            {
                if (isMTKeyManager)
                    return null;
                if (instance == null)
                {
                    GameObject obj = new GameObject("[MTKeyManager]");
                    instance = obj.AddComponent<MTKeyManager>();

                    GameObject.DontDestroyOnLoad(obj);
                }
                return instance;
            }
        }

        public Action<MTKeyStateType> OnConnected;

        private EncryptionManager encryptionManager;//网络加密
        private CheckNumberInfo checkNumberInfo;
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="ServiceNumber"></param>
        /// <param name="ProjectName"></param>
        /// <param name="isUseCheckNumber"></param>
        /// <param name="action"></param>
        public void OnAddLisenter(string ServiceNumber, string ProjectName, bool isUseCheckNumber = true, System.Action<MTKeyStateType> action = null)
        {
            if (isUseCheckNumber)
            {
                OnConnected = action;
                checkNumberInfo = new CheckNumberInfo() { ServiceNumber = ServiceNumber, ProjectName = ProjectName };
                CreatCheckNumber(checkNumberInfo);
            }
        }

        /// <summary>
        /// 创建网络加密
        /// </summary>
        /// <param name="num"></param>
        /// <param name="projectName"></param>
        private void CreatCheckNumber(CheckNumberInfo checkNumberInfo)
        {
            if (encryptionManager == null)
            {
                GameObject obj = new GameObject("[CheckNumber]");
                encryptionManager = obj.AddComponent<EncryptionManager>();
            }

            if (checkNumberInfo != null && encryptionManager != null)
            {
                encryptionManager.Init(checkNumberInfo.ProjectName, checkNumberInfo.ServiceNumber);
            }
            else
            {
                checkNumberInfo = new CheckNumberInfo();
                Debug.Log(checkNumberInfo.ProjectName + " 222: " + checkNumberInfo.ServiceNumber);
                encryptionManager.Init(checkNumberInfo.ProjectName, checkNumberInfo.ServiceNumber);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        internal void Close()
        {
            isMTKeyManager = true;
        }

        private void OnDestroy()
        {
            instance = null;
            Close();
        }
        private void OnApplicationQuit()
        {
            instance = null;
            Close();
        }
    }
}

