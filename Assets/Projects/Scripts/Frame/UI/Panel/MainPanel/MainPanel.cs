using MTFrame;
using MTFrame.MTKinect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{
    public static MainPanel Instance;

    public MenuButtonGroupPanel menuButtonGroupPanel;

    public CanvasGroup LoseTips;

    public Hand[] hands;

    private Player PrimaryPlay;

    public AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void InitFind()
    {
        base.InitFind();

        LoseTips = FindTool.FindChildComponent<CanvasGroup>(transform, "BG/Mask/LoseTips");

        menuButtonGroupPanel = GetComponentInChildren<MenuButtonGroupPanel>();

        menuButtonGroupPanel.Init(this);

        hands = GetComponentsInChildren<Hand>();

        audioSource = FindTool.FindChildComponent<AudioSource>(transform, "BGM");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        PlayerManager.Instance.onFindPrimarilyPlayerEvent += FindPrimarilyPlayer;
        PlayerManager.Instance.onLosePrimarilyPlayerEvent += LosePrimarilyPlayer;
    }

    public override void Open()
    {
        base.Open();
        menuButtonGroupPanel.Hide();
        PlayerManager.Instance.onPlayerGestureEvent += OnPlayerGestureEvent;

        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, MainData.Instance.aP_BideTime, OnDelayedCall);
    }

    public override void Hide()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].UserID = 0;
        }
        base.Hide();
        PlayerManager.Instance.onPlayerGestureEvent -= OnPlayerGestureEvent;
    }


    private void LosePrimarilyPlayer(Player obj)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].UserID =0;
        }
        LoseTips.DOFillAlpha(1, 0.5f);
        menuButtonGroupPanel.Hide();
        PrimaryPlay = null;
        menuButtonGroupPanel.userID = 0;

        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, MainData.Instance.aP_BideTime, OnDelayedCall);
    }
    private void FindPrimarilyPlayer(Player obj)
    {
        if (!MainData.Instance.isMainWindow)
            return;

        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].UserID = obj.UserID;
        }
        LoseTips.DOFillAlpha(0, 0.5f);
        menuButtonGroupPanel.Open();
        PrimaryPlay = obj;
        menuButtonGroupPanel.userID = PrimaryPlay.UserID;

        UIManager.ChangePanelState<MainAdvertisementPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact,OnDelayedCall);
    }

    private void OnDelayedCall()
    {
        UIManager.ChangePanelState<MainAdvertisementPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Open);
    }

    private void OnPlayerGestureEvent(long userid, string gesture)
    {
        if (PrimaryPlay != null)
            if (userid == PrimaryPlay.UserID)
            {
                if (gesture == "举起左手")
                {
                    menuButtonGroupPanel.Open();
                    menuButtonGroupPanel.userID = userid;
                }
            }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerManager.Instance.onFindPrimarilyPlayerEvent -= FindPrimarilyPlayer;
        PlayerManager.Instance.onLosePrimarilyPlayerEvent -= LosePrimarilyPlayer;
    }
}