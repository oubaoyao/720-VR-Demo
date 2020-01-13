using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class WaitPanel : BasePanel
{
    public VRButton button;

    private bool IsRotate;

    public Transform WaiQuan, NeiQuan, Center;

    private float RotaSpeed = 2.0f;

    public Image image;

    public override void InitFind()
    {
        base.InitFind();
        button = FindTool.FindChildComponent<VRButton>(transform, "buttonGroup");
        WaiQuan = FindTool.FindChildNode(transform, "buttonGroup/nishizhen");
        NeiQuan = FindTool.FindChildNode(transform, "buttonGroup/shunshizhen");
        Center = FindTool.FindChildNode(transform, "buttonGroup/nishizhen1");
        image = FindTool.FindChildComponent<Image>(transform, "buttonGroup/yuan");
    }

    public override void InitEvent()
    {
        base.InitEvent();

        button.LeftSteamVrLaserPointer = SphereControl.Instance.LeftsteamVR_LaserPointer;
        button.RightSteamVrLaserPointer = SphereControl.Instance.RightsteamVR_LaserPointer;

        button.mOnEnter.AddListener(() => {
            IsRotate = true;
        });

        button.mOnUp.AddListener(() => {
            IsRotate = false;
            image.fillAmount = 0;
            RotaSpeed = 5.0f;
        });
    }

    public override void Open()
    {
        base.Open();
        image.fillAmount = 0;
        SphereControl.Instance.SetSkyBox( SphereControl.Instance.WaitBgSkyBox);
        SphereControl.Instance.SetLaserColor();

    }

    private void Update()
    {
        if(IsOpen && IsRotate)
        {
            RotaSpeed += 0.05f;
            WaiQuan.Rotate(Vector3.forward * RotaSpeed);
            Center.Rotate(Vector3.forward * RotaSpeed);
            NeiQuan.Rotate(Vector3.back * RotaSpeed);
            image.fillAmount += 0.003f;
            if(image.fillAmount >= 1)
            {
                VRState.PanelSwitch(PanelName.MenuPanel);
                SphereControl.Instance.SetSkyBox(SphereControl.Instance.SkyboxTexture[0]);
            }
        }
    }
}
