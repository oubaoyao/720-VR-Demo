using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTask : BaseTask
{
    public LoadingTask(BaseState state) : base(state)
    {
    }
    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<LoadDataPanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<LoadDataPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
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