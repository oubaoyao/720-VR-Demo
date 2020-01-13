using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInterTask : BaseTask
{
    public MultiInterTask(BaseState state) : base(state)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //UIManager.CreatePanel<AdvertisementPanel>(WindowTypeEnum.ForegroundScreen);
        UIManager.CreatePanel<MainPanel>(WindowTypeEnum.Screen);
        UIManager.CreatePanel<MainAdvertisementPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);       
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

}