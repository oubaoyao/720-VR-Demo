using MTFrame;
using MTFrame.MTEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataPanel : BasePanel
{
    public Text processTips;

    private float currentProcess;
    private float loadProcess;
    public override void InitFind()
    {
        base.InitFind();
        processTips = FindTool.FindChildComponent<Text>(transform,"processTips");
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, "LoadProcess", OnLoadProcess);
    }

    public override void Hide()
    {
        base.Hide();
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, "LoadProcess", OnLoadProcess);
    }

    private void OnUpdate(float timeProcess)
    {
        currentProcess = Mathf.Lerp(currentProcess, loadProcess, Time.deltaTime);
        if (currentProcess <= 1)
        {
            processTips.text = (currentProcess * 100).ToString("00") + "%";
        }
    }

    private void OnLoadProcess(EventParamete parameteData)
    {
        float f = parameteData.GetParameter<float>()[0];
        loadProcess = f;
    }
}