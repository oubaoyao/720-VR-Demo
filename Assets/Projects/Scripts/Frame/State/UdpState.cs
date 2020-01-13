using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;
using System;

public class UdpState : BaseState
{
    //注意state一定要在get里面监听事件，没有的话就写成下面样子
    //这里一般用来监听Panel切换
    private Queue<string> GetVs = new Queue<string>();
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {
                //事件名string类型
                "aaa",
            };
        }
        set { }
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {

        //接收监听事件的数据，然后用swich判断做处理

        //除此之外，也可以在这里监听UDP传输的数据，但是接收的数据是子线程数据，要通过队列接收，
        //然后在update转换成主线程数据，才能对数据进行处理

        if(parameteData.EvendName == "aaa")
        {
            //获取数据parameteData.GetParameter<string>()[0]
            GetVs.Enqueue(parameteData.GetParameter<string>()[0]);
        }
    }

    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new UdpTask(this));
        EventManager.AddUpdateListener(UpdateEventEnumType.Update,"OnUpdate",Onupdate);
    }

    private void Onupdate(float timeProcess)
    {
        //数据在这里转换
        lock(GetVs)
        {
            if(GetVs.Count > 0)
            {
                string st = GetVs.Dequeue();
                Debug.Log("状态类里接收到的数据：" + st);
                EventParamete eventParamete = new EventParamete();
                eventParamete.AddParameter(st);
                EventManager.TriggerEvent(GenericEventEnumType.Message, "bbb", eventParamete);
                //在这里进行switch对数据进行处理
            }
        }
    }
}
