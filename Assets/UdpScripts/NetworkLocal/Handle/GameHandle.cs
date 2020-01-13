using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using Proto;
using NetworkCommonTools.LiteNetLibEngine;

public class GameHandle : HandleBase
{
    string index;
    MTFrame.MTEvent.EventParamete eventParamete = new MTFrame.MTEvent.EventParamete();
    public override byte OperateHandleCode => (byte)OperateCodes.Game;

    public override void OnReceiveProcess(INetEngine netEngine, NetPeer netPeer, OperationResponse operation)
    {
        //这里获取相应数据类型的数据，这里获取的string类型数据
        index = operation.GetParemater<string>(ParmaterCodes.index);
        Log.WriteLine("[index]=" + index);
        //UdpSclient.Instance.SendMsgsToState(index, "ThreadMsg");
        eventParamete.AddParameter(index);
        EventManager.TriggerEvent(MTFrame.MTEvent.GenericEventEnumType.Message, "aaa", eventParamete);
    }

    public override void OnUnconnectedRequestProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }

    public override void OnUnconnectedResponseProcess(INetEngine netEngine, IPEndPoint endPoint, OperationResponse operation)
    {

    }
}