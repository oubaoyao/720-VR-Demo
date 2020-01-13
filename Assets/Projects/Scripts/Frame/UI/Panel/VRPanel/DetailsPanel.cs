using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

/// <summary>
/// 详情页模板类
/// </summary>
[System.Serializable]
public class DetailsItem
{
    public VRButton Left_Button, Right_Button,Display_Button;

    public Image DisplayImage;

    public Text PageText, TiltleText;

    public Sprite[] sprite;

    public int Current_Page;
}


public class DetailsPanel : BasePanel
{

    public Text tiltleText;

    public CanvasGroup[] DetailsCanvasGroups;

    private float ScaleRatio = 1.5f;

    public DetailsItem detailsItem;

    public Sprite[] OI_Sprite, HuXing_Sprite;

    public MenuPanel menuPanel;

    public string Current_SkyBox_Name;

    public bool Is_Open_Huxing;

    protected override void Start()
    {
        base.Start();
        Reset();
        HuXing_Sprite = SphereControl.Instance.textures;
        Current_SkyBox_Name = HuXing_Sprite[0].name;
    }

    public override void InitFind()
    {
        base.InitFind();

        detailsItem = new DetailsItem();

        tiltleText = FindTool.FindChildComponent<Text>(transform, "tiltleText");

        DetailsCanvasGroups = FindTool.FindChildNode(transform, "DetailsGroup").GetComponentsInChildren<CanvasGroup>();

        detailsItem.Left_Button = FindTool.FindChildComponent<VRButton>(transform, "DetailsGroup/操作说明and户型选择/TurnPageButtonGroup/leftButton");
        detailsItem.Right_Button = FindTool.FindChildComponent<VRButton>(transform, "DetailsGroup/操作说明and户型选择/TurnPageButtonGroup/RightButton");
        detailsItem.Display_Button = FindTool.FindChildComponent<VRButton>(transform, "DetailsGroup/操作说明and户型选择/DisplayImage");

        detailsItem.DisplayImage = FindTool.FindChildComponent<Image>(transform, "DetailsGroup/操作说明and户型选择/DisplayImage");

        detailsItem.PageText = FindTool.FindChildComponent<Text>(transform, "DetailsGroup/操作说明and户型选择/PageNumberText");
        detailsItem.TiltleText = FindTool.FindChildComponent<Text>(transform, "DetailsGroup/操作说明and户型选择/tiltleText");

        menuPanel = FindTool.FindParentComponent<MenuPanel>(transform, "MenuPanel");

        OI_Sprite = Resources.LoadAll<Sprite>("OISprite");

    }

    public override void InitEvent()
    {
        base.InitEvent();

        detailsItem.Left_Button.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        detailsItem.Left_Button.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        detailsItem.Right_Button.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        detailsItem.Right_Button.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        detailsItem.Display_Button.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        detailsItem.Display_Button.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        detailsItem.Left_Button.mOnEnter.AddListener(() => {
            ButtonAnimation(detailsItem.Left_Button.transform);
        });

        detailsItem.Left_Button.mOnClick.AddListener(() => {
            Previous_Page();
        });

        detailsItem.Left_Button.mOnUp.AddListener(() => {
            Reset_Button_Scale(detailsItem.Left_Button.transform);
        });

        detailsItem.Right_Button.mOnEnter.AddListener(() => {
            ButtonAnimation(detailsItem.Right_Button.transform);
        });

        detailsItem.Right_Button.mOnClick.AddListener(() => {
            Next_Page();
        });

        detailsItem.Right_Button.mOnUp.AddListener(() => {
            Reset_Button_Scale(detailsItem.Right_Button.transform);
        });

        detailsItem.Display_Button.mOnEnter.AddListener(() => {
            if(Is_Open_Huxing)
            {
                detailsItem.Display_Button.transform.GetComponent<Image>().color = Color.red;
            }
        });

        detailsItem.Display_Button.mOnClick.AddListener(() => {
            if (Is_Open_Huxing)
            {
                menuPanel.SwitchSkyBox(detailsItem.DisplayImage.sprite.name);
            }
        });

        detailsItem.Display_Button.mOnUp.AddListener(() => {
            if (Is_Open_Huxing)
            {
                detailsItem.Display_Button.transform.GetComponent<Image>().color = Color.white;
            }
        });
    }

    private void ButtonAnimation(Transform transform)
    {
        transform.localScale = new Vector3(ScaleRatio, ScaleRatio, ScaleRatio);
    }

    private void Reset_Button_Scale(Transform transform)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Reset()
    {
        SwitchPanel(OI_Sprite,"操作说明");
    }

    private void Set_Page_Text(Text text,int Current_Page,int All_Page)
    {
        text.text = "页码 " + Current_Page + " / " + All_Page;
    }

    /// <summary>
    /// 切换操作说明和户型选择页面
    /// </summary>
    /// <param name="sprites"></param>
    public void SwitchPanel(Sprite[] sprites,string name)
    {
        if(name == "户型选择")
        {
            Is_Open_Huxing = true;
        }
        else
        {
            Is_Open_Huxing = false;
        }

        detailsItem.sprite = null;
        detailsItem.sprite = sprites;

        detailsItem.Current_Page = 0;
        Set_Page_Text(detailsItem.PageText,1, detailsItem.sprite.Length);

        detailsItem.DisplayImage.sprite = detailsItem.sprite[0];

        tiltleText.text = name;

        detailsItem.TiltleText.text = detailsItem.sprite[0].name;

        DetailsCanvasGroups[0].alpha = 0;
        DetailsCanvasGroups[1].alpha = 0;
        DetailsCanvasGroups[1].DOFillAlpha(1, 0.5f);
        DetailsCanvasGroups[1].blocksRaycasts = true;

    }

    /// <summary>
    /// 切换简介页面
    /// </summary>
    public void SwitchJianjiePanel()
    {
        DetailsCanvasGroups[0].DOFillAlpha(1, 0.5f);

        DetailsCanvasGroups[1].alpha = 0;
        DetailsCanvasGroups[1].blocksRaycasts = false;

        tiltleText.text = "简介";
    }

    public void Next_Page()
    {
        detailsItem.Current_Page++;
        if(detailsItem.Current_Page >= detailsItem.sprite.Length-1)
        {
            detailsItem.Current_Page = detailsItem.sprite.Length - 1;
        }

        detailsItem.DisplayImage.sprite = detailsItem.sprite[detailsItem.Current_Page];

        detailsItem.TiltleText.text = detailsItem.sprite[detailsItem.Current_Page].name;

        Set_Page_Text(detailsItem.PageText, detailsItem.Current_Page+1, detailsItem.sprite.Length);
    }

    public void Previous_Page()
    {
        detailsItem.Current_Page--;
        if (detailsItem.Current_Page <= 0)
        {
            detailsItem.Current_Page = 0;
        }

        detailsItem.DisplayImage.sprite = detailsItem.sprite[detailsItem.Current_Page];

        detailsItem.TiltleText.text = detailsItem.sprite[detailsItem.Current_Page].name;

        Set_Page_Text(detailsItem.PageText, detailsItem.Current_Page + 1, detailsItem.sprite.Length);
    }
}
