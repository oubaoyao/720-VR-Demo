using System.Collections;
using System.Collections.Generic;
using NetworkLocalFrame;
using NetworkCommonTools;
using NetworkCommonTools.LiteNetLibEngine;
using LiteNetLib;
using Proto;
using System.Net;
using NetworkLocalFrame.Engine;
using System.Net.Sockets;
using System.Threading.Tasks;

public class GameLocalClientEngineListener : GameLocalEngine
{
    /// <summary>
    /// 客户端
    /// </summary>
    public NetPeer ServerPeer { get; private set; }

    public GameLocalClientEngineListener(int port, string connectionKey) : base(port, connectionKey)
    {
        HandleManager.Instance.AddHandle(PacketType.System, new RoomSearchControlHandle());
        HandleManager.Instance.AddHandle(PacketType.System, new RoomUserControlHandle());
    }

    /// <summary>
    /// 发送消息到服务器
    /// </summary>
    /// <param name="operation"></param>
    public void SendData(OperationResponse operation)
    {
        if (ServerPeer != null)
        {
            this.Send(operation, this.ServerPeer);
        }
        else
        {
            Log.WriteLine("[连接已断开]");
        }
    }
    /// <summary>
    /// 发送消息到服务器
    /// </summary>
    /// <param name="operation"></param>
    public void SendDataToSystem(OperationResponse operation)
    {
        if (ServerPeer != null)
        {
            this.SendToSystem(operation, this.ServerPeer);
        }
        else
        {
            Log.WriteLine("[连接已断开]");
        }
    }

    /// <summary>
    /// 接收搜索到的服务器
    /// </summary>
    /// <param name="roomInfos"></param>
    protected override void OnFinishSearchListener(RoomInfo[] roomInfos)
    {
        base.OnFinishSearchListener(roomInfos);
        Log.WriteLine("[搜索成功]:" + roomInfos.Length);
        //如果搜索到服务器房间就连接这个房间，否则就一直搜索
        if (roomInfos.Length > 0)
        {
            string id = roomInfos[0].Initiator_ID;
            this.Connect(roomInfos[0][id].Address.IP);
        }    
        else
            this.Search();
    }

    protected override void OnRoomConnectedSuccessListener(NetPeer netPeer, LiteLocal liteLocal, RoomInfo roomInfo)
    {
        base.OnRoomConnectedSuccessListener(netPeer, liteLocal, roomInfo);
        ServerPeer = netPeer;
        Log.WriteLineTest("[Server]=" + ServerPeer.EndPoint.ToString());
    }

    protected override void OnRoomConnectedFailureListener(NetPeer netPeer, LiteLocal liteLocal)
    {
        base.OnRoomConnectedFailureListener(netPeer, liteLocal);
        ServerPeer = null;
    }

    protected override void OnRoomDisconnectedListener(NetPeer netPeer, LiteLocal liteLocal, DisconnectedType disconnectedType)
    {
        base.OnRoomDisconnectedListener(netPeer, liteLocal, disconnectedType);
        ServerPeer = null;
    }

    protected override void OnPlayerConnectedListener(User user, IPEndPoint iPEndPoint, LiteLocal liteLocal)
    {
        base.OnPlayerConnectedListener(user, iPEndPoint, liteLocal);
    }

    protected override void OnPlayerDisconnectedListener(User user, IPEndPoint iPEndPoint, LiteLocal liteLocal)
    {
        base.OnPlayerDisconnectedListener(user, iPEndPoint, liteLocal);
    }

    //IOS或者安卓休眠掉线重连
    protected override void OnReconnectedListener(NetPeer netPeer, LiteLocal liteLocal, int reconnectedCount)
    {
        base.OnReconnectedListener(netPeer, liteLocal, reconnectedCount);
        ServerPeer = null;
    }

    protected override void OnNetworkErrorListener( SocketError socketError)
    {
        base.OnNetworkErrorListener(socketError);
        //Log.WriteLine("[Error]:" + socketError);
        if (this.ConnectState == ConnectState.Connected) Reconnect();
    }

    private async void Reconnect()
    {
        this.ShutDown();
        await Task.Delay(1000);
        this.Search();
    }
}
