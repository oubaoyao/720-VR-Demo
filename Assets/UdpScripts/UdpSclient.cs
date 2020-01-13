using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommonTools;
using System.IO;

//****Udp客户端****
//****数据接收在GameHandle脚本接收****
//****要在PlayerSettings->otherSettings里面将Api Compatibility Level设置为.net4.x 如果没有则无法使用

public class UdpSclient : MonoBehaviour
{
    public static UdpSclient Instance;
    public GameLocalClientEngineListener localClientEngine;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //打印debug信息，如果不需要可以注释掉
        Log.Init(new UnityDebug(), false);
        Log.LogIsDebug[Log.LogType.Normal] = true;
        Log.WriteLine("开始");

        UserManager.Instance.LocalUser = new User() { ID = "002", nickname = "aa" };
        //不同端口号和房间名就不会连接
        localClientEngine = new GameLocalClientEngineListener(9999, "Test4");
        localClientEngine.Search();
    }

    // Update is called once per frame
    void Update()
    {
        //测试用
        if (Input.GetKeyDown(KeyCode.D))
        {
            SendMassageToSever("你好，服务器端！");
        }
    }

    //public void SendMsgsToState(string msg,string EventName)
    //{
    //    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    //    eventParamete.AddParameter(msg);
    //    EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, EventName, eventParamete);
    //}

    /// <summary>
    /// 此函数发送的信息类型只限string
    /// </summary>
    /// <param name="msg"></param>
    public void SendMassageToSever(string msg)
    {

        OperationResponse response = OperationResponseExtend.GetOperationResponse((byte)OperateCodes.Game);
        response.AddParemater((byte)ParmaterCodes.index, msg);
        localClientEngine.SendData(response);
    }

    private void OnDestroy()
    {
        localClientEngine.ShutDown();
    }
}
