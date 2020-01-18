using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialPanel : BasePanel
{
    public CanvasGroup BGCanvasGroup, TextGroup,Teach1,Teach2,Teach3;
    public Text ContentText;
    public Image baize;
    public Animation jiantou,shoubing_left,shoubing_right;
    public Animator Frame;

    public bool IsTeaching;
    public bool IsTeach1;
    public bool IsTeach2;
    public bool IsTeach3;

    public bool IsLeft;
    public bool IsRight;

    public VRButton CloseButton;

    private float DelayTime = 2.0f;

    private float[] Time_daziji = {
        8.0f,
        7.0f,
        6.0f,
    };

    private string[] Tips = {
        "亲爱的用户，您好，我是白泽，请根据指示完成VR中的“点击”过程。请将手柄中发射的绿色射线对准“户型选择”按钮，然后扣下扳机键。",
        "恭喜您，您已经学会了VR中的点击方式，在本软件中所有的按钮都可以通过此方式进行点击。接下来请按一下圆盘的左右键。",
        "恭喜您，您已经完成了全部得教学，最后告诉您，本菜单页可以通过菜单键打开和关闭哦，如果不知道哪个是菜单键，可以进入“操作说明”页面进行查看。"
    };

    public override void InitFind()
    {
        base.InitFind();
        BGCanvasGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "bg");
        TextGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "TextGroup");
        Teach1 = FindTool.FindChildComponent<CanvasGroup>(transform, "1");
        Teach2 = FindTool.FindChildComponent<CanvasGroup>(transform, "2");
        Teach3 = FindTool.FindChildComponent<CanvasGroup>(transform, "3");

        ContentText = FindTool.FindChildComponent<Text>(transform, "TextGroup/Text");

        baize = FindTool.FindChildComponent<Image>(transform, "baize");

        jiantou = FindTool.FindChildComponent<Animation>(transform, "1/jiantou");
        shoubing_left = FindTool.FindChildComponent<Animation>(transform, "2/shoubingGroup/left");
        shoubing_right = FindTool.FindChildComponent<Animation>(transform, "2/shoubingGroup/right");

        Frame = FindTool.FindChildComponent<Animator>(transform, "1/frame");

        CloseButton = FindTool.FindChildComponent<VRButton>(transform, "3/Text");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        CloseButton.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        CloseButton.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        CloseButton.mOnEnter.AddListener(() => {
            CloseButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        });

        CloseButton.mOnClick.AddListener(() => {
            Hide();
            IsTeach3 = true;
        });

        CloseButton.mOnUp.AddListener(() => {
            CloseButton.transform.localScale = new Vector3(1, 1, 1);
        });
    }

    public override void Open()
    {
        base.Open();
        Tutorial_1();
        IsTeach1 = false;
        IsTeach2 = false;
        IsTeach3 = false;
        IsLeft = false;
        IsRight = false;
    }

    public override void Hide()
    {
        base.Hide();
        Reset();
    }

    /// <summary>
    /// 教学 第一步
    /// </summary>
    private void Tutorial_1()
    {
        IsTeaching = false;
        TextGroup.transform.GetChild(1).GetComponent<Text>().text = "";
        baize.DOFillAmount(1, 1.0f).OnComplete(() => {
            TextGroup.DOFillAlpha(1,0.5f).OnComplete(()=> {
                ContentText.DOText(Tips[0], Time_daziji[0]).OnComplete(() => {
                    TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, DelayTime, () => {
                        BG_Close();
                        Teach1_Anima_Open();
                        IsTeaching = true;
                    });
                });
            });
        });
    }

    /// <summary>
    /// 教学 第二步
    /// </summary>
    public void Tutorial_2()
    {
        Teach1_Anima_Hide();
        IsTeaching = false;
        BGCanvasGroup.blocksRaycasts = true;
        TextGroup.transform.GetChild(1).GetComponent<Text>().text = "";
        BGCanvasGroup.DOFillAlpha(1, 0.5f).OnComplete(() => {
            TextGroup.DOFillAlpha(1, 0.5f).OnComplete(() => {
                ContentText.DOText(Tips[1], Time_daziji[1]).OnComplete(() => {
                    TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, DelayTime, () => {
                        baize.enabled = false;
                        BG_Close();
                        Teach2.DOFillAlpha(1, 0.5f).OnComplete(() => {
                            IsTeaching = true;
                        });
                    });
                });
            });

        });
    }

    /// <summary>
    /// 教学 第三步
    /// </summary>
    public void Tutorial_3()
    {
        IsTeaching = false;
        Teach2.alpha = 0;
        BGCanvasGroup.blocksRaycasts = true;
        TextGroup.transform.GetChild(1).GetComponent<Text>().text = "";
        BGCanvasGroup.DOFillAlpha(1, 0.5f).OnComplete(() => {
            baize.enabled = true;
            TextGroup.DOFillAlpha(1, 0.5f).OnComplete(() =>
            {
                ContentText.DOText(Tips[2], Time_daziji[2]).OnComplete(() =>
                {
                    TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, DelayTime, () => {
                        Teach3.DOFillAlpha(1, 0.5f);
                        Teach3.blocksRaycasts = true;
                        IsTeaching = true;
                    });
                });
            });
         });
    }

    private void Teach1_Anima_Open()
    {
        Teach1.alpha = 1;
        jiantou.Play();
        Frame.SetBool("Open", true);
    }

    private void Teach1_Anima_Hide()
    {
        Teach1.alpha = 0;
        jiantou.Stop();
        Frame.SetBool("Close", true);
    }

    private void BG_Close()
    {
        TextGroup.DOFillAlpha(0, 0.5f);
        BGCanvasGroup.DOFillAlpha(0, 0.5f);
        BGCanvasGroup.blocksRaycasts = false;
    }

    private void Reset()
    {
        BGCanvasGroup.alpha = 1;
        BGCanvasGroup.blocksRaycasts = true;

        baize.fillAmount = 0;

        TextGroup.alpha = 0;

        Teach1.alpha = 0;
        Teach2.alpha = 0;
        Teach3.alpha = 0;
        Teach3.blocksRaycasts = false;

        shoubing_right.transform.GetComponent<CanvasGroup>().alpha = 1;
        shoubing_left.transform.GetComponent<CanvasGroup>().alpha = 1;
    }
}
