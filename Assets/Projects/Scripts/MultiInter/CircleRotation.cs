using UnityEngine;
using System.Collections;
/// <summary>
/// 旋转
/// </summary>
public class CircleRotation : MonoBehaviour {

    [SerializeField]
    private float m_RotationSpeed = 10;
    [SerializeField]
    private float m_Diretion = 1;

    private Transform rect;
    	
    void Awake()
    {
        rect = this.transform;
    }

	// Update is called once per frame
	void Update () {
        rect.Rotate((Vector3.forward * m_Diretion).normalized * m_RotationSpeed * Time.deltaTime);
	}
}
