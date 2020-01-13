using MTFrame;
using MTFrame.MTKinect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public long UserID;

    public KinectInterop.JointType jointType;
    public Rect rect = new Rect(0, 0, 640, 480);

    public float offectValue = 1;
    public Vector3 pos;
    private void Update()
    {
        if (UserID == 0)
        {
            transform.localPosition = Vector3.one * 10000;
            return;
        }
        transform.localPosition = KINECTManager.Instance.GetUserIDJointPos2D(UserID, jointType, Camera.main, rect);

        pos = KINECTManager.Instance.GetUserIDJointPos(UserID, jointType, Camera.main, rect);
        float f = offectValue - pos.z * 0.5f;
        transform.localScale = Vector3.one * f;
    }

    public Collider theCollider;
    private void OnTriggerStay(Collider other)
    {
        BaseButton baseButton = default(BaseButton);
        if (theCollider == null)
        {
            theCollider = other;
        }
        else if(theCollider!= other)
        {
            baseButton = theCollider.GetComponent<BaseButton>();
            if(baseButton)
            baseButton.TriggerExit();

            theCollider = other;
        }
        baseButton =  theCollider.GetComponent<BaseButton>();
        if(baseButton)
           (baseButton as MenuButton).OnUpdate();        
    }

    private void OnTriggerExit(Collider other)
    {
        BaseButton baseButton = other.GetComponent<BaseButton>();
        if (baseButton)
            baseButton.TriggerExit();

    }

}