using MTFrame;
using MTFrame.MTEvent;
using MTFrame.MTKinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadingState : BaseState
{
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {

            };
        }
        set { }
    }

    public override void Enter()
    {
        base.Enter();

        KINECTManager.Instance.Close();

        CurrentTask.ChangeTask(new LoadingTask(this));

        DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath + "/GameData/Data");
        foreach (DirectoryInfo  directory in di.GetDirectories())
        {

            MainData.Instance.directoryPathDatas.Add(new DirectoryPathData(directory));
        }

        di = new DirectoryInfo(Application.streamingAssetsPath + "/GameData/广告");
        MainData.Instance.AP_filePathData = new FilePathData(di);


        di = new DirectoryInfo(Application.streamingAssetsPath + "/GameData/互动游戏");
        foreach (DirectoryInfo directory in di.GetDirectories())
        {
            MainData.Instance.Game_filePathData.Add(new GameFilePathData(directory));
        }


        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact,3, OnDelayed);
    }

    private void OnDelayed()
    {
        StateManager.ChangeState(new MultiInterState());
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