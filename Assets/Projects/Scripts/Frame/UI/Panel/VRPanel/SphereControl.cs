using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class SphereControl : MonoBehaviour
{
    public static SphereControl Instance;

    public GameObject Player;

    //public MeshRenderer SphereRenderer;

    public SteamVR_LaserPointer LeftsteamVR_LaserPointer, RightsteamVR_LaserPointer;

    public Skybox skybox;

    public Sprite[] textures;

    public Texture[] SkyboxTexture;

    public Texture WaitBgSkyBox;

    private void Awake()
    {
        Instance = this;

        LeftsteamVR_LaserPointer = Player.transform.GetChild(0).GetComponent<SteamVR_LaserPointer>();
        RightsteamVR_LaserPointer = Player.transform.GetChild(1).GetComponent<SteamVR_LaserPointer>();

        skybox = Player.GetComponentInChildren<Skybox>();

        textures = Resources.LoadAll<Sprite>("莫扎特2");
        SkyboxTexture = Resources.LoadAll<Texture>("莫扎特");
        WaitBgSkyBox = Resources.Load<Texture>("待机页背景/待机页背景");
    }

    public void Laser_OpenAndClose()
    {
        if (LeftsteamVR_LaserPointer.enabled)
        {
            LeftsteamVR_LaserPointer.enabled = false;    //射线关闭
            LeftsteamVR_LaserPointer.pointer.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            LeftsteamVR_LaserPointer.enabled = true;    //射线开启
            LeftsteamVR_LaserPointer.pointer.transform.GetComponent<MeshRenderer>().enabled = true;
        }

        if (RightsteamVR_LaserPointer.enabled)
        {
            RightsteamVR_LaserPointer.enabled = false;    //射线关闭
            RightsteamVR_LaserPointer.pointer.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            RightsteamVR_LaserPointer.enabled = true;    //射线开启
            RightsteamVR_LaserPointer.pointer.transform.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void SetSkyBox(Texture texture)
    {
        skybox.material.SetTexture("_Tex", texture);
        Valve.VR.SteamVR_Fade.View(Color.black, 0);
        Valve.VR.SteamVR_Fade.View(Color.clear, 3);
    }

    public void SetLaserColor()
    {
        LeftsteamVR_LaserPointer.color = new Color(0, 255/255f, 0);
        RightsteamVR_LaserPointer.color = new Color(0, 255 / 255f, 0);
    }

    public void ResetLaserColor()
    {
        LeftsteamVR_LaserPointer.color = Color.black;
        RightsteamVR_LaserPointer.color = Color.black;
    }
}
