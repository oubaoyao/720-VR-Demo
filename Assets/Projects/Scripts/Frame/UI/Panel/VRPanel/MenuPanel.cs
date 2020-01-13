using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using Valve.VR;
using System;
using Valve.VR.InteractionSystem;

public class MenuPanel : BasePanel
{
    public VRButton[] MenuButton;

    public DetailsPanel detailsPanel;

    public SteamVR_Action_Boolean Menu = SteamVR_Input.GetBooleanAction("Menu");
    public SteamVR_Action_Boolean TurnLeft = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean TurnRight = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    public Sprite[] Sprite_weidianji,Sprite_dianji;

    public override void InitFind()
    {
        base.InitFind();
        MenuButton = FindTool.FindChildNode(transform, "decoration/MenuButtonGroup").GetComponentsInChildren<VRButton>();

        detailsPanel = FindTool.FindChildComponent<DetailsPanel>(transform, "DetailsPanel");

        Sprite_weidianji = Resources.LoadAll<Sprite>("Button/weidianji");
        Sprite_dianji = Resources.LoadAll<Sprite>("Button/dianji");
    }

    public override void InitEvent()
    {
        base.InitEvent();

        for (int i = 0; i < MenuButton.Length; i++)
        {
            InitVRButton(MenuButton[i], i);
        }

        MenuButton[0].mOnClick.AddListener(() => {
            detailsPanel.SwitchJianjiePanel();

        });

        MenuButton[1].mOnClick.AddListener(() => {
            detailsPanel.SwitchPanel(detailsPanel.OI_Sprite, "操作说明");
        });

        MenuButton[2].mOnClick.AddListener(() => {
            detailsPanel.SwitchPanel(detailsPanel.HuXing_Sprite, "户型选择");
        });
    }

    public override void Open()
    {
        base.Open();
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "MenuUpdate", MenuUpdate);
        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "MenuUpdate", MenuUpdate);
    }

    private void MenuUpdate(float timeProcess)
    {
        if (Menu != null && Menu.GetStateUp(SteamVR_Input_Sources.Any))
        {
            if (IsOpen)
            {
                Hide();
            }
            else
            {
                Open();
            }
            SphereControl.Instance.Laser_OpenAndClose();
        }

        if(IsOpen && detailsPanel.DetailsCanvasGroups[1].alpha > 0)
        {
            if(TurnLeft!=null && TurnLeft.GetStateUp(SteamVR_Input_Sources.Any))
            {
                detailsPanel.Previous_Page();
            }

            if (TurnRight != null && TurnRight.GetStateUp(SteamVR_Input_Sources.Any))
            {
                detailsPanel.Next_Page();
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "MenuUpdate", MenuUpdate);
    }
    //public override void Hide()
    //{
    //    base.Hide();
    //    EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "MenuUpdate", MenuUpdate);
    //}

    private void Menu_Button_OnEnter(Transform transform,int number)
    {
        transform.GetComponent<Image>().sprite = Sprite_dianji[number];
        transform.GetChild(1).GetComponent<Image>().enabled = true;
    }

    private void Menu_Button_OnUp(Transform transform,int number)
    {
        transform.GetComponent<Image>().sprite = Sprite_weidianji[number];
        transform.GetChild(1).GetComponent<Image>().enabled = false;
    }

    public void SwitchSkyBox(string SkyBoxName)
    {
        if (detailsPanel.Current_SkyBox_Name != SkyBoxName && detailsPanel.Is_Open_Huxing)
        {
            foreach (Texture item in SphereControl.Instance.SkyboxTexture)
            {
                if (item.name == SkyBoxName)
                {
                    SphereControl.Instance.SetSkyBox(item);
                    detailsPanel.Current_SkyBox_Name = item.name;
                    break;
                }
            }
        }
    }

    private void InitVRButton(VRButton vRButton,int number)
    {
        vRButton.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        vRButton.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        vRButton.mOnEnter.AddListener(() => {
            Menu_Button_OnEnter(vRButton.transform,number);
        });

        vRButton.mOnUp.AddListener(() => {
            Menu_Button_OnUp(vRButton.transform,number);
        });
    }


    //public VRButton[] VRButtonGroup;
    //public VRButton DisplayVRButton;



    //public RawImage DisplayImage;

    //public SteamVR_Action_Boolean Menu = SteamVR_Input.GetBooleanAction("Menu");

    //public string Current_RoomName = null;

    //public override void InitFind()
    //{
    //    base.InitFind();
    //    VRButtonGroup = FindTool.FindChildNode(transform, "ButtonGroup").GetComponentsInChildren<VRButton>();
    //    DisplayVRButton = FindTool.FindChildComponent<VRButton>(transform, "RawImage");

    //    DisplayImage = FindTool.FindChildComponent<RawImage>(transform, "RawImage");

    //}

    //public override void InitEvent()
    //{
    //    base.InitEvent();
    //    for (int i = 0; i < VRButtonGroup.Length; i++)
    //    {
    //        VRButtonGroup[i].GetComponentInChildren<Text>().text = SphereControl.Instance.textures[i].name;
    //        VRButtonGroup[i].LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
    //        VRButtonGroup[i].RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;
    //        InitVRButton(VRButtonGroup[i], i);
    //    }

    //    DisplayVRButton.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
    //    DisplayVRButton.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;
    //    DisplayVRButton.mOnClick.AddListener(() =>
    //    {
    //        if (Current_RoomName != DisplayImage.texture.name)
    //        {
    //            foreach (Texture item in SphereControl.Instance.SkyboxTexture)
    //            {
    //                if (item.name.Contains(DisplayImage.texture.name))
    //                {
    //                    SphereControl.Instance.SetSkyBox(item);
    //                    Current_RoomName = item.name;
    //                    break;
    //                }
    //            }
    //        }
    //    });

    //    DisplayVRButton.mOnEnter.AddListener(() =>
    //    {
    //        DisplayVRButton.transform.GetComponent<RawImage>().color = Color.red;
    //    });

    //    DisplayVRButton.mOnUp.AddListener(() =>
    //    {
    //        DisplayVRButton.transform.GetComponent<RawImage>().color = Color.white;
    //    });

    //    DisplayImage.texture = SphereControl.Instance.textures[0];

    //    //Teleport.roomNameDelegate += ChangeSkyBox;
    //}

    //public override void Open()
    //{
    //    base.Open();
    //    SphereControl.Instance.ResetLaserColor();
    //    SphereControl.Instance.SetSkyBox( SphereControl.Instance.SkyboxTexture[0]);
    //    Current_RoomName = SphereControl.Instance.SkyboxTexture[0].name;
    //    EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
    //    SphereControl.Instance.Player.transform.localPosition = Vector3.zero;
    //    SphereControl.Instance.Player.transform.localEulerAngles = Vector3.zero;
    //}

    //public override void Hide()
    //{
    //    base.Hide();
    //    EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
    //}

    //private void OnUpdate(float timeProcess)
    //{
    //    if(Menu!=null&& Menu.GetStateDown(SteamVR_Input_Sources.Any))
    //    {
    //        if(IsOpen)
    //        {
    //            Hide();
    //        }
    //        else
    //        {
    //            Open();
    //        }
    //        SphereControl.Instance.Laser_OpenAndClose();
    //    }
    //}

    //protected override void OnDestroy()
    //{
    //    SphereControl.Instance.skybox.material.SetTexture("_Tex", SphereControl.Instance.SkyboxTexture[0]);
    //    base.OnDestroy(); 
    //    EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "OnUpdate", OnUpdate);
    //}

    //private void InitVRButton(VRButton button, int i)
    //{
    //    button.mOnClick.AddListener(() =>
    //    {
    //        DisplayImage.texture = SphereControl.Instance.textures[i];
    //    });

    //    button.mOnEnter.AddListener(() =>
    //    {
    //        button.transform.GetComponent<Image>().color = Color.red;
    //    });

    //    button.mOnUp.AddListener(() =>
    //    {
    //        button.transform.GetComponent<Image>().color = Color.white;
    //    });
    //}
}
