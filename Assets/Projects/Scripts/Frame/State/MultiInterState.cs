using MTFrame;
using MTFrame.MTEvent;
using MTFrame.MTKinect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 场景切换用法
/// </summary>
public class MultiInterState : BaseState
{
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[] 
            {

            };
        }set {}
    }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadSceneAsync(ScenePath.MultiInter, MTFrame.MTScene.LoadingModeType.UnityLocal,null,null,()=>
        {
            CurrentTask.ChangeTask(new MultiInterTask(this));
            SceneManager.UnloadScene(ScenePath.Main, MTFrame.MTScene.LoadingModeType.UnityLocal);
            KINECTManager.Instance.Open();

        }, MTFrame.MTScene.LoadingType.Additive);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {

    }

    protected override void OnUpdate(float processTime)
    {
        base.OnUpdate(processTime);
    }
}