using MTFrame.MTKinect;
using UnityEngine;

public class TrackingHead : MonoBehaviour
{
    public int index;
    public KinectInterop. JointType jointType;
    public Rect rect = new Rect(0, 0, 640, 480);

    public Vector2 offectPos;
    private void Update()
    {
        transform.localPosition=KINECTManager.Instance.GetIndexJointPos2D(index, jointType,Camera.main, rect)+ offectPos;
    }
}