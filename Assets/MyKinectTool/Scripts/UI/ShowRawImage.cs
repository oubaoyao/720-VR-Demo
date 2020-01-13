using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class ShowRawImage : MonoBehaviour
    {
        public KinectImageType imageType; 
        [Header("背景面板")]
        public RawImage rawImage;
        public void Update()
        {
            rawImage.texture= KINECTManager.Instance.GetOrbbecImage(imageType);
        }
    }
}
