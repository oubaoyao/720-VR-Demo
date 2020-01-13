using MTFrame;
using MTFrame.MTKinect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonGroupPanel : BasePanel
{
    public long userID;
    public KinectInterop.JointType jointType;
    public Rect rect = new Rect(0, 0, 640, 480);

    public Vector2 offectPos = new Vector2(0, 50);
    public float offectValue = 2;


    public ContentPanel contentPanel;
    public GameContentPanel gameContentPanel;

    public Transform MenuButtonStartPos;
    public CanvasGroup Group_Button, Tips;
    public MenuButton[] baseButtons;

    private Vector3 pos;



    public override void Init(BasePanel basePanel = null)
    {
        base.Init(basePanel);

    }
    public override void InitFind()
    {
        base.InitFind();

        MenuButtonStartPos = FindTool.FindChildComponent<Transform>(transform.parent, "MenuButtonStartPos");
        Group_Button = FindTool.FindChildComponent<CanvasGroup>(transform, "Group_Button");
        baseButtons = Group_Button.GetComponentsInChildren<MenuButton>();
        Tips = FindTool.FindChildComponent<CanvasGroup>(transform.parent, "Tips");

        contentPanel = FindTool.FindChildComponent<ContentPanel>(transform.parent, "ContentPanel");
        gameContentPanel = FindTool.FindChildComponent<GameContentPanel>(transform.parent, "GameContentPanel");

        contentPanel.Init(this);
        gameContentPanel.Init(this);

        Tips.DOFillAlpha(0, 0.5f);

        for (int i = 0; i < MainData.Instance.directoryPathDatas.Count; i++)
        {
            string str = MainData.Instance.directoryPathDatas[i].directoryInfo.Name;
            //如果提示这里索引超出范围，就将预设里六个按钮，被关掉的两个按钮的勾打上
            baseButtons[i].Init(str);
        }

    }

    public override void InitEvent()
    {
        base.InitEvent();
        int index = 0;
        for (int i = 0; i < MainData.Instance.directoryPathDatas.Count; i++)
        {
            baseButtons[i].OnClick += OnClick;
            index++;
        }
        for (int i = index; i < baseButtons.Length - 1; i++)
        {
            baseButtons[i].gameObject.SetActive(false);
        }
        //Debug.Log("baseButtons:" + baseButtons.Length);
        baseButtons[5].OnClick += OnGameButtonClick;
    }

    private void OnGameButtonClick(BaseButton obj)
    {
        Tips.DOFillAlpha(1, 0.5f);
        Hide();
        gameContentPanel.Open();
    }

    private void OnClick(BaseButton button)
    {
        Tips.DOFillAlpha(1, 0.5f);
        Hide();
        contentPanel.SetPanel((button as MenuButton).GetName());
    }

    public override void Open()
    {
        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);

        foreach (var item in tweenObjects)
        {
            item.Stop();
        }

        contentPanel.Hide();
        gameContentPanel.Hide();
        Tips.DOFillAlpha(0, 0.5f);
        Group_Button.DOFillAlpha(1, 0.5f);
        Group_Button.transform.DOSize(Vector3.one, 0.5f).OnComplete(() =>
        {
            foreach (BaseButton button in baseButtons)
            {
                (button as MenuButton).Open();
            }
        });
    }

    List<TweenObject> tweenObjects = new List<TweenObject>();
    public override void Hide()
    {
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);

        tweenObjects.Clear();

        contentPanel.Hide();
        gameContentPanel.Hide();
        tweenObjects.Add(Group_Button.DOFillAlpha(0, 0.5f));
        tweenObjects.Add(Group_Button.transform.DOSize(Vector3.zero, 0.5f).OnComplete(() =>
        {
            tweenObjects.Add(transform.DOSize(Vector3.one * 0.5f, 0.5f).OnComplete(() =>
            {
                tweenObjects.Add(transform.DOMove(MenuButtonStartPos.transform.position, 1).Ease(TweenType.EaselnOutBack));
            }));
        }));
        foreach (BaseButton button in baseButtons)
        {
            (button as MenuButton).Close();
        }
    }

    private void OnUpdate(float timeProcess)
    {
        Vector3 _pos = KINECTManager.Instance.GetUserIDJointPos2D(userID, jointType, Camera.main, rect) + offectPos;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _pos, Time.deltaTime * 5);
        pos = KINECTManager.Instance.GetUserIDJointPos(userID, jointType, Camera.main, rect);
        float f = offectValue - pos.z * 0.5f;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * f, Time.deltaTime * 3);
    }
}