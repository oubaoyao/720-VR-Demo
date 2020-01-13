using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MT.Key
{

    /// <summary>
    /// 加密管理
    /// </summary>
    public class EncryptionManager : MonoBehaviour
    {

        public bool useLockDog = true;

        private CheckNumber checkNumber = new CheckNumber();

        private bool curDogStatus = false;
        private bool updateChecked = false;
        private float lockdogTiemr;
        // Use this for initialization
        //初始化
        public void Init(string projectName, string serviceNumber)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);//不被销毁

            if (!checkNumber.IsCheck && !checkNumber.IsConnected)
            {
                checkNumber.Init(projectName, serviceNumber);
                checkNumber.ConnectService();
            }
        }

        public void OnDestroy()
        {
            checkNumber.DisposeConnect();
        }

        public void OnApplicationQuit()
        {
            checkNumber.DisposeConnect();
        }
    }
}
