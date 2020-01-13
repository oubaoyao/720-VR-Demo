using MTFrame.MTKinect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tesk : MonoBehaviour {

    public RawImage rawImage;
    public Toggle toggle;
    //public GameObject obj;

    public Vector2 vector;
	// Use this for initialization
	void Start ()
    {
        PlayerManager.Instance.AddPlayerGestureEvent(0, "举起右手", "测试", "注释：测试专用", () => { Debug.Log("测试"); });
        PlayerManager.Instance.AddPlayerGestureEvent(0, "右挥手", "测试1", "注释：测试专用", () => { Debug.Log("测试1"); });
        PlayerManager.Instance.AddPlayerGestureEvent(0, "举起左手", "测试2", "注释：测试专用", () => { Debug.Log("测试2"); });
        toggle.onValueChanged.AddListener((isOn) => { Main.Instance.IsDebug = isOn; });
    }
	
	// Update is called once per frame
	void Update ()
    {
        rawImage.texture = KINECTManager.Instance.GetOrbbecImage(KinectImageType.colour);
        //PlayerManager.Instance.AddTrackEvent(JointTrackType._3D, 0, JointType.Head, obj.transform); 
    }
}
