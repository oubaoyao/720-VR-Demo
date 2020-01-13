using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using System.Threading;
using System;
using MT.Key;

/// <summary>
/// 读取网络加密数据
/// </summary>
public class CheckNumber
{

    public float autoCloseTime = 15;
    public bool IsCheck { get; set; }
    public bool IsConnected { get; set; }
    public string ServiceNumber { get; private set; }
    public string ProjectName { get; private set; }

    WebSocket webSocket;
    private string serviceIp = "120.76.121.209";
    private int port = 7272;

    private bool isOnLine = false;
    private bool isExit = false;
    private float timer = 0;
    private int times;

    private Timer udpTimer;
    
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="projectName"></param>
    /// <param name="serviceNumber"></param>
    public void Init(string projectName, string serviceNumber)
    {
        ProjectName = projectName;
        ServiceNumber = serviceNumber;

        webSocket = new WebSocket("ws://" + serviceIp + ":" + port.ToString());
        webSocket.OnOpen += WebSocket_OnOpen;
        webSocket.OnError += WebSocket_OnError;
        webSocket.OnMessage += WebSocket_OnMessage;
        webSocket.OnClose += WebSocket_OnClose;
        udpTimer = new Timer(ping, null, 1000, 3000);
        //Main.Instance.OnEventUpdate += UpdateData;
    }

    //ping
    private void ping(object state)
    {

        if (webSocket != null)
        {
            if (webSocket.ReadyState == WebSocketState.Open)
                webSocket.Send("ping");
            else if (webSocket.ReadyState != WebSocketState.Open && webSocket.ReadyState != WebSocketState.Connecting)
            {
                webSocket.Connect();
            }
        }
    }
    //连接
    public void ConnectService()
    {
        Debug.Log("链接服务器");
        IsCheck = true;
        webSocket.ConnectAsync();    
    }

    //关闭
    private void WebSocket_OnClose(object sender, CloseEventArgs e)
    {
        if (IsCheck)
        {
            Debug.Log("Close");
            isExit = true;
            MTKeyManager.Instance?.OnConnected?.Invoke( MTKeyStateType.网络断开);
        }
        else
        {
            Debug.Log("Service Leave");
            IsConnected = false;
            MTKeyManager.Instance?.OnConnected?.Invoke( MTKeyStateType.网络断开);
        }
    }
    //信息回调
    private void WebSocket_OnMessage(object sender, MessageEventArgs e)
    {
        if (!IsCheck) return;
        string jsonData = System.Text.Encoding.UTF8.GetString(e.RawData);
        Dictionary<string, object> json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
        if (json == null) return;
        if (!json.ContainsKey("state")) return;
        int index;
        if (int.TryParse(json["state"].ToString(), out index))
        {
            StateType type = (StateType)index;
            switch (type)
            {
                case StateType.login:
                    IsConnected = true;
                    Debug.Log("网络加密成功");
                    MTKeyManager.Instance?.OnConnected?.Invoke( MTKeyStateType.网络加密成功);
                    break;
                case StateType.error:
                case StateType.quit:
                    Debug.Log("网络加密失败");
                    MTKeyManager.Instance?.OnConnected?.Invoke( MTKeyStateType.网络加密失败);
                    isExit = true;
                    break;
            }
        }
    }
    //错误
    private void WebSocket_OnError(object sender, ErrorEventArgs e)
    {
        isExit = true;
        Debug.Log("cuo:" + e.Message);
        MTKeyManager.Instance?.OnConnected?.Invoke(MTKeyStateType.网络出错);
    }
    //打开
    private void WebSocket_OnOpen(object sender, System.EventArgs e)
    {
        Register();
    }

    //断开连接
    public void DisposeConnect()
    {
        IsCheck = false;
        
        if (webSocket.IsAlive)
            webSocket.CloseAsync();
        udpTimer?.Dispose();
    }

    /// <summary>
    /// 接收
    /// </summary>
    private void Register()
    {
        Dictionary<string, object> json = new Dictionary<string, object>();
        json.Add("type", "login");
        json.Add("project", ProjectName);
        json.Add("client_name", ServiceNumber);
        string t = Newtonsoft.Json.JsonConvert.SerializeObject(json);
        webSocket.Send(t);
    }

 /// <summary>
 /// 状态
 /// </summary>
    enum StateType
    {
        login = 1,
        error,
        quit
    }
}
/// <summary>
/// 加密状态类型
/// </summary>
public enum MTKeyStateType
{
    网络加密成功,
    网络加密失败,
    网络出错,
    网络断开
}