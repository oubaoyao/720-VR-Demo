using System.Net;
using NetworkLocalFrame;
using NetworkCommonTools;
using LiteNetLib;
using Proto;

public class GameLocalServerEngineListener : GameLocalEngine
{
    //本机终端
    private NetPeer serverPeer;
    //连接到本机的客户端终端，如果是多个就使用链表保存
    private NetPeer client;
    public GameLocalServerEngineListener(int port, string connectionKey) : base(port, connectionKey)
    {

    }

    public void SendData(OperationResponse operationResponse)
    {
        //发送给特定终端
        this.Send(operationResponse, client);
        //发送给所有连接进来的终端
        //this.SendToAll(null, this.serverPeer);
    }

    //这个函数会在每个客户端连接进来的时候调用一次
    protected override void OnPlayerConnectedListener(User user, IPEndPoint iPEndPoint, LiteLocal liteLocal)
    {
        base.OnPlayerConnectedListener(user, iPEndPoint, liteLocal);
        //查找连接进来的终端，然后保存
        client = this.FindPeer(iPEndPoint);

    }

    //客户端断开连接的时候调用这个函数，清除该终端
    protected override void OnPlayerDisconnectedListener(User user, IPEndPoint iPEndPoint, LiteLocal liteLocal)
    {
        base.OnPlayerDisconnectedListener(user, iPEndPoint, liteLocal);
        //如果是一个客户端就使其为空，如果是链表就删除
        if (client.EndPoint == iPEndPoint)
        {
            client = null;
        }
    }

    //获取本机的终端
    protected override void OnRoomConnectedSuccessListener(NetPeer netPeer, LiteLocal liteLocal, RoomInfo roomInfo)
    {
        base.OnRoomConnectedSuccessListener(netPeer, liteLocal, roomInfo);
        this.serverPeer = netPeer;
    }
}