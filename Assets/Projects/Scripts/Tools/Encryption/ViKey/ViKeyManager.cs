using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace MTFrame.Tool.ViKey
{
    /// <summary>
    /// 加密狗加密管理
    /// </summary>
    public class ViKeyManager
    {

        private static ViKeyManager instance;
        //单例
        public static ViKeyManager Instance { get { if (instance == null) instance = new ViKeyManager(); return instance; } }

        //信息回调事件
        public Action<bool> OnConnected;


        //加密狗后台程序路径
        private string path;
        //TCP客户端
        private TCPClient tCPClient;
        //加密狗数据
        private ViKeyData viKeyData = new ViKeyData();
        //本地加密狗加密信息
        private VikeyEncryptionInfo localVikeyInfo;
        //打开的加密狗工具窗口
        private Process process;
        private Timer timer;
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="vikeyInfo"></param>
        /// <param name="action"></param>
        public void OnAddLisenter(VikeyEncryptionInfo localVikeyInfo, System.Action<bool> action = null)
        {
            this.localVikeyInfo = localVikeyInfo;//赋值本地加密信息

            path = Application.streamingAssetsPath + "/a45+das8fjam95/hd4rt52dfg34+dfhlas/yyrth1df22121d331/VikeyTool.exe";

            try
            {
                foreach (Process item in Process.GetProcesses())//查找是否开窗口如果有就关闭;
                {
                    if (item.ProcessName == "VikeyTool")
                        item.Kill();
                }
            }catch
            { }

            process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = path;
            process.Start();

            tCPClient = new TCPClient();//实例化客户端
            tCPClient.OnReceiveCall += OnReceiveCall;


            OnConnected = action;

            if (timer == null)
                timer = new Timer(OnUpdate, null, 1000, 2000);
        }

        private int _VikeyType = -1;
        private int _LoginType = -1;
        private int _MsgData = -1;
        //更新
        private void OnUpdate(object o)
        {
            if (viKeyData != null)
            {
                if (_VikeyType != (int)viKeyData.vikeyType)
                {
                    _VikeyType = (int)viKeyData.vikeyType;
                    switch (viKeyData.vikeyType)
                    {
                        case VikeyType.没有加密狗:
                            UnityEngine.Debug.Log("没有加密狗");
                            OnConnected?.Invoke(false);
                            return;
                        case VikeyType.无效的加密狗:
                            UnityEngine.Debug.Log("无效的加密狗");
                            OnConnected?.Invoke(false);
                            return;
                        case VikeyType.有效的加密狗:
                            UnityEngine.Debug.Log("有效的加密狗");
                            break;
                        case VikeyType.系统出错:
                            UnityEngine.Debug.Log("机密狗系统出错");
                            OnConnected?.Invoke(false);
                            return;
                        default:
                            break;
                    }
                }
                if (viKeyData.vikeyType == VikeyType.有效的加密狗)
                {
                    if (_LoginType != (int)viKeyData.loginType)
                    {
                        _LoginType = (int)viKeyData.loginType;
                        switch (viKeyData.loginType)
                        {
                            case LoginType.Factory:
                                UnityEngine.Debug.Log("默认加密狗");
                                break;
                            case LoginType.MT:
                                UnityEngine.Debug.Log("MT加密狗");
                                break;
                            case LoginType.Unknow:
                                UnityEngine.Debug.Log("未知加密狗");
                                OnConnected?.Invoke(false);
                                return;
                            default:
                                break;
                        }
                    }
                    if (viKeyData.loginType != LoginType.Unknow)
                    {
                        if (viKeyData.vikeyEncryptionInfo.vikeyEncryptionType != localVikeyInfo.vikeyEncryptionType)
                        {
                            UnityEngine.Debug.Log("加密狗类型错误" + viKeyData.vikeyEncryptionInfo.vikeyEncryptionType + "=====" + localVikeyInfo.vikeyEncryptionType);
                            OnConnected?.Invoke(false);
                            return;
                        }
                        else if (viKeyData.vikeyEncryptionInfo.Key != localVikeyInfo.Key)
                        {
                            UnityEngine.Debug.Log("加密狗密钥错误" + viKeyData.vikeyEncryptionInfo.Key + "=====" + localVikeyInfo.Key);
                            OnConnected?.Invoke(false);
                            return;
                        }
                        else if (viKeyData.vikeyEncryptionInfo.ProjectName != localVikeyInfo.ProjectName)
                        {
                            UnityEngine.Debug.Log("加密狗项目名称错误" + viKeyData.vikeyEncryptionInfo.ProjectName + "=====" + localVikeyInfo.ProjectName);
                            OnConnected?.Invoke(false);
                            return;
                        }
                        else
                        {
                            OnConnected?.Invoke(true);
                        }
                    }
                }
            }
        }

        //监听TCP接收的回调
        private void OnReceiveCall(string obj)
        {
            if (obj.IsJson())
            {
                viKeyData = Newtonsoft.Json.JsonConvert.DeserializeObject<ViKeyData>(obj);
            }

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            UnityEngine.Debug.Log("关闭加密狗");
            if (process != null)
            {
                process.Kill();
                process = null;
            }
            if (tCPClient != null)
            {
                tCPClient.Close();
            }
            timer?.Dispose();
        }

    }
    /// <summary>
    /// 加密类型
    /// </summary>
    public enum VikeyEncryptionType
    {
        Null,
        App,
        Kinect,
        VR
    }

    /// <summary>
    /// 登陆状态
    /// </summary>
    public enum LoginType
    {
        Factory,
        MT,
        Unknow
    }
    /// <summary>
    /// 加密狗状态
    /// </summary>
    public enum VikeyType
    {
        没有加密狗,
        无效的加密狗,
        有效的加密狗,
        系统出错
    }

    /// <summary>
    /// 加密狗信息
    /// </summary>
    public class ViKeyData
    {
        /// <summary>
        /// 加密狗状态
        /// </summary>
        public VikeyType vikeyType = VikeyType.没有加密狗;
        /// <summary>
        /// 登陆状态
        /// </summary>
        public LoginType loginType = LoginType.Factory;
        /// <summary>
        /// 加密狗加密信息
        /// </summary>
        public VikeyEncryptionInfo vikeyEncryptionInfo = new VikeyEncryptionInfo();
    }
    /// <summary>
    /// 加密狗加密信息
    /// </summary>
    public class VikeyEncryptionInfo
    {
        /// <summary>
        /// 加密类型
        /// </summary>
        public VikeyEncryptionType vikeyEncryptionType = VikeyEncryptionType.Null;
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key = "";
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName = "";
    }
}