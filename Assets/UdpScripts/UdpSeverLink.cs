using NetworkCommonTools;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MTFrame.MTEvent;
using System;

//****Udp服务器端****
//****数据接收在GameHandle脚本接收****
//****要在PlayerSettings->otherSettings里面将Api Compatibility Level设置为.net4.x 如果没有则无法使用

public class UdpSeverLink : MonoBehaviour
{
    public static UdpSeverLink Instance;
    public GameLocalServerEngineListener localServerEngine;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //打印debug信息，如果不需要可以注释掉
        Log.Init(new UnityDebug(), true);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");

        UserManager.Instance.LocalUser = new User() { ID = "001", nickname = "xxx" };
        //设置房间信息
        RoomManager.Instance.LocalRoom = new Room(UserManager.Instance.LocalUser, 4);
        RoomManager.Instance.LocalRoom.RoomInfo.RoomName = "房间001";

        localServerEngine = new GameLocalServerEngineListener(9999, "Test4");
        localServerEngine.Creat();
    }

    private void Update()
    {
        //测试用
        if(Input.GetKeyDown(KeyCode.A))
        {
            SendMsgToClient("你好，客户端！");
        }
    }

    private void OnDestroy()
    {
        localServerEngine.ShutDown();
    }

    public void SendMsgToClient(string msg)
    {
        OperationResponse response = OperationResponseExtend.GetOperationResponse((byte)OperateCodes.Game);
        response.AddParemater((byte)ParmaterCodes.index,msg);
        localServerEngine?.SendData(response);
        Debug.Log("发送信息给客户端:" + msg);
    }

    ///// <summary>
    ///// 默认事件类型GenericEventEnumType.Message，发送信息给状态类，让状态类的监听函数进行相应的操作(发送的信息为字符串的时候才能使用该函数)
    ///// </summary>
    ///// <param name="msg"></param>
    ///// <param name="EventName"></param>
    ///// <param name="eventParamete"></param>
    //public void SendMsgsToState(string msg, string EventName)
    //{
    //    EventParamete eventParamete = new EventParamete();
    //    eventParamete.AddParameter(msg);
    //    EventManager.TriggerEvent(GenericEventEnumType.Message, EventName, eventParamete);
    //}
}


