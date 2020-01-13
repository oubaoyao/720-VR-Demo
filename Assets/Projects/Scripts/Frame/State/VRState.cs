using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;

public enum SwitchPanel
{
    PanelToState,
}
public enum PanelName
{
    WaitPanel,
    MenuPanel,
}


public class VRState : BaseState
{
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[] {
                SwitchPanel.PanelToState.ToString(),
            };
        }
        set { }
    }
    public override void OnListenerMessage(EventParamete parameteData)
    {
        if (parameteData.EvendName == SwitchPanel.PanelToState.ToString())
        {
            PanelName panelName = parameteData.GetParameter<PanelName>()[0];
            switch (panelName)
            {
                case PanelName.WaitPanel:
                    CurrentTask.ChangeTask(new WaitTask(this));
                    break;
                case PanelName.MenuPanel:
                    CurrentTask.ChangeTask(new MenuTask(this));
                    break;
                default:
                    break;
            }
        }
    }
    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new WaitTask(this));
    }
    public static void PanelSwitch(PanelName panelName)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter<PanelName>(panelName);
        EventManager.TriggerEvent(GenericEventEnumType.Message, SwitchPanel.PanelToState.ToString(), eventParamete);
    }
}
