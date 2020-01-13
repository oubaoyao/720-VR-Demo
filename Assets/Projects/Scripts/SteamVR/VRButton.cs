using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.Extras;

[RequireComponent(typeof(BoxCollider))]
public class VRButton : MonoBehaviour
{
    public SteamVR_LaserPointer LeftSteamVrLaserPointer, RightSteamVrLaserPointer;
    public UnityEvent mOnEnter = null;
    public UnityEvent mOnClick = null;
    public UnityEvent mOnUp = null;

    private void Awake()
    {
        LeftSteamVrLaserPointer.PointerClick += SteamVrLaserPointer_PointerClick;
        LeftSteamVrLaserPointer.PointerIn += SteamVrLaserPointer_PointerIn;
        LeftSteamVrLaserPointer.PointerOut += SteamVrLaserPointer_PointerOut;

        RightSteamVrLaserPointer.PointerClick += SteamVrLaserPointer_PointerClick;
        RightSteamVrLaserPointer.PointerIn += SteamVrLaserPointer_PointerIn;
        RightSteamVrLaserPointer.PointerOut += SteamVrLaserPointer_PointerOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<BoxCollider>().center = Vector3.zero;
        //如果该按钮使用Grid Lauout Group组件进行排列的话，则无法取得该按钮的rect.size
        transform.GetComponent<BoxCollider>().size = this.GetComponent<RectTransform>().rect.size;
    }

    void OnDestroy()
    {
        LeftSteamVrLaserPointer.PointerClick -= SteamVrLaserPointer_PointerClick;
        LeftSteamVrLaserPointer.PointerIn -= SteamVrLaserPointer_PointerIn;
        LeftSteamVrLaserPointer.PointerOut -= SteamVrLaserPointer_PointerOut;

        RightSteamVrLaserPointer.PointerClick -= SteamVrLaserPointer_PointerClick;
        RightSteamVrLaserPointer.PointerIn -= SteamVrLaserPointer_PointerIn;
        RightSteamVrLaserPointer.PointerOut -= SteamVrLaserPointer_PointerOut;
    }
    private void SteamVrLaserPointer_PointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject == this.gameObject)
        {
            if (mOnUp != null) mOnUp.Invoke();
        }
    }

    private void SteamVrLaserPointer_PointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject == this.gameObject)
        {
            if (mOnEnter != null) mOnEnter.Invoke();
        }
    }

    private void SteamVrLaserPointer_PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject == this.gameObject)
        {
            if (mOnClick != null) mOnClick.Invoke();
        }
    }
}
