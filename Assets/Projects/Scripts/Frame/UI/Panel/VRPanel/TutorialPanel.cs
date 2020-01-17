using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialPanel : BasePanel
{
    public CanvasGroup BGCanvasGroup, TextGroup,Teach1;
    public Text ContentText;
    public Image baize;
    public Animation jiantou;
    public Animator Frame;

    public bool IsTeaching;
    public bool IsTeach1;
    public bool IsTeach2;

    private float[] Time_daziji = {
        4.0f,
    };

    private string[] Tips = {
        "亲爱的用户，您好，我是白泽，请根据指示完成VR中的“点击”过程。请将手柄中发射的绿色射线对准“户型选择”按钮，然后扣下扳机键。",

    };

    public override void InitFind()
    {
        base.InitFind();
        BGCanvasGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "bg");
        TextGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "TextGroup");
        Teach1 = FindTool.FindChildComponent<CanvasGroup>(transform, "1");

        ContentText = FindTool.FindChildComponent<Text>(transform, "TextGroup/Text");

        baize = FindTool.FindChildComponent<Image>(transform, "baize");

        jiantou = FindTool.FindChildComponent<Animation>(transform, "1/jiantou");

        Frame = FindTool.FindChildComponent<Animator>(transform, "1/frame");
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        Tutorial_1();
        IsTeach1 = false;
        IsTeach2 = false;
    }

    public override void Hide()
    {
        base.Hide();
    }

    /// <summary>
    /// 教学 第一步
    /// </summary>
    private void Tutorial_1()
    {
        IsTeaching = false;
        baize.DOFillAmount(1, 1.0f).OnComplete(() => {
            TextGroup.DOFillAlpha(1,0.5f).OnComplete(()=> {
                ContentText.DOText(Tips[0], Time_daziji[0]).OnComplete(() => {
                    TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 1.0f, () => {
                        TextGroup.DOFillAlpha(0, 0.5f).OnComplete(() => {
                            BGCanvasGroup.alpha = 0;
                        });
                        Teach1_Anima_Open();
                        IsTeaching = true;
                    });
                });
            });
        });
    }

    public void Tutorial_2()
    {
        IsTeaching = false;
        BGCanvasGroup.DOFillAlpha(1, 0.5f).OnComplete(() => {

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
}
