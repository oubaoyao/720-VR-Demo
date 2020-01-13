using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame.MTKinect;

public class NobleMetalControl : MonoBehaviour
{
    public static NobleMetalControl Instance;

    private Vector3 lastPos;
    private Vector3 nextPos;
    private Vector3 Center_Vector;

    private float TouchRotationSpeed = 5.0f;
    private float Max_LocalScale = 2.0f;
    private float Min_LocalScale = 1.0f;
    public float spinSpeed = 10;

    //存储初始信息
    List<Vector3> Local_Position = new List<Vector3>();
    List<Quaternion> Local_Rotasion = new List<Quaternion>();
    List<Vector3> Local_Scale = new List<Vector3>();

    public List<Transform> NobleMetal = new List<Transform>();
    public Transform Cur_NobleMetal;

    private int NobleMetal_Array_Number = 0;

    private long UserID;
    //是否第一次握右拳
    private bool IsFirstGrip;
    //第一次握拳的时候右手的位置
    private Vector3 FirstGripPos;
    private float CountDown;
    private float CountDown_Max = 2.0f;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            NobleMetal.Add(child);
            Local_Position.Add(child.localPosition);
            Local_Rotasion.Add(child.localRotation);
            Local_Scale.Add(child.localScale);
        }
        lastPos = nextPos = Vector3.zero;
        //Center_Vector = new Vector3(0, 0, 3);
        IsFirstGrip = true;
        CountDown = CountDown_Max;
    }

    // Update is called once per frame
    void Update()
    {
        if(Cur_NobleMetal != null)
        {
            UserID = KinectManager.Instance.GetPrimaryUserID();
            //获取左右手位置
            Vector3 left_Pos = KinectManager.Instance.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.HandLeft, Camera.main, new Rect(0, 0, 640, 480));
            Vector3 right_Pos = KinectManager.Instance.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.HandRight, Camera.main, new Rect(0, 0, 640, 480));
            //获取左右手肘位置
            Vector3 Right_Elbow = KinectManager.Instance.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.ElbowRight, Camera.main, new Rect(0, 0, 640, 480));
            Vector3 Left_Elbow = KinectManager.Instance.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.ElbowLeft, Camera.main, new Rect(0, 0, 640, 480));
            //缩放
            if (KINECTManager.Instance.GetHandState(HandType.Right) == InteractionManager.HandEventType.Grip && KINECTManager.Instance.GetHandState(HandType.Left) == InteractionManager.HandEventType.Grip 
                && IsFirstGrip != false && Mathf.Abs(left_Pos.y - right_Pos.y) < 0.5f && right_Pos.y > Right_Elbow.y && left_Pos.y > Left_Elbow.y)
            {
                //Debug.Log("right_Pos:" + right_Pos);
                float factor = Vector3.Distance(right_Pos, left_Pos) * 8;
                //Debug.Log("缩放因子：" + factor);
                if (factor > Max_LocalScale)
                {
                    factor = Max_LocalScale;
                }
                else if (factor < Min_LocalScale)
                {
                    factor = Min_LocalScale;
                }
                Vector3 newLocalScale = new Vector3(factor, factor, factor);
                Cur_NobleMetal.transform.localScale = Vector3.Lerp(Cur_NobleMetal.transform.localScale, newLocalScale, spinSpeed * Time.deltaTime);
            }

            //旋转
            if (KINECTManager.Instance.GetHandState(HandType.Right) == InteractionManager.HandEventType.Grip && left_Pos.y < Left_Elbow.y)
            {
                if (IsFirstGrip == true)
                {
                    //记录每次第一次握拳右手的位置,以此为基准
                    FirstGripPos = right_Pos;
                    IsFirstGrip = false;
                }
                Vector3 right_Pos2 = right_Pos;
                //Debug.Log("手坐标：" + right_Pos);
                //Debug.Log("111手坐标:" + FirstGripPos);
                //水平方向差值
                float Pos_x = right_Pos2.x - FirstGripPos.x;
                //竖直方向差值
                float Pos_y = right_Pos2.y - FirstGripPos.y;
                //当差值大于0.05f的时候才会认为是在切换方向
                if(Mathf.Abs(Pos_x) > 0.05f || Mathf.Abs(Pos_y) > 0.05f)
                {
                    //如果水平方向的差值大于竖直方向的差值，则优先水平方向的操作，反之则优先竖直方向的操作
                    if (Mathf.Abs(Pos_x) > Mathf.Abs(Pos_y))
                    {
                        //大于零则向右旋转，反之则向左旋转
                        if (Pos_x > 0)
                        {
                            Debug.Log("向右旋转");
                            Cur_NobleMetal.transform.Rotate(Vector3.down * TouchRotationSpeed, Space.World);
                        }
                        else
                        {
                            Debug.Log("向左旋转");
                            Cur_NobleMetal.transform.Rotate(Vector3.up * TouchRotationSpeed, Space.World);
                        }
                    }
                    else
                    {
                        if (Pos_y > 0)
                        {
                            Debug.Log("向前旋转");
                            Cur_NobleMetal.transform.Rotate(Vector3.left * TouchRotationSpeed, Space.World);
                        }
                        else
                        {
                            Debug.Log("向后旋转");
                            Cur_NobleMetal.transform.Rotate(Vector3.right * TouchRotationSpeed, Space.World);
                        }
                    }
                }
                else
                {
                    //不动则还原初始角度
                    CountDown -= Time.deltaTime;
                    if(CountDown <= 0 )
                    {
                        NobleMetal[NobleMetal_Array_Number].localRotation = Local_Rotasion[NobleMetal_Array_Number];
                        CountDown = CountDown_Max;
                    }
                }
                //NobleMetal_Rotate();
            }
            else
            {
                IsFirstGrip = true;
                CountDown = CountDown_Max;
               //lastPos = Vector3.zero;
            }
        }

    }

    //public void NobleMetal_Rotate()
    //{
    //    nextPos = InteractionManager.Instance.cursorScreenPos;
    //    nextPos.z = 1;
    //    if (lastPos != Vector3.zero && nextPos != lastPos)
    //    {
    //        Vector3 center = Center_Vector;
    //        Vector3 lastDir = lastPos - center;
    //        Vector3 nextDir = nextPos - center;

    //        Vector3 cross = Vector3.Cross(lastDir.normalized, nextDir.normalized);
    //        Cur_NobleMetal.transform.Rotate(cross * TouchRotationSpeed, Space.World);

    //        lastPos = nextPos;

    //    }
    //    else if(lastPos == Vector3.zero)
    //    {
    //        lastPos = nextPos;
    //    }
    //}

    private void OnPlayerGesture(long userid, string gesture)
    {
        if (userid == PlayerManager.Instance.GetPrimaryPlay().UserID && IsFirstGrip)
        {
            switch (gesture)
            {
                case "右挥手":
                    Debug.Log("下一个");
                    Next();
                    break;
                case "左挥手":
                    Last();
                    break;
            }
        }
    }

    private void Next()
    {
        Reset_Transform_Tool(NobleMetal_Array_Number);
        NobleMetal[NobleMetal_Array_Number].gameObject.SetActive(false);
        NobleMetal_Array_Number++;
        if (NobleMetal_Array_Number > 2)
        {
            NobleMetal_Array_Number = 0;
        }
        Cur_NobleMetal = NobleMetal[NobleMetal_Array_Number];
        Cur_NobleMetal.gameObject.SetActive(true);
        ContentPanel.Instance.SwitchIntroduceImageAndTiltle(NobleMetal_Array_Number);
    }

    private void Last()
    {
        Reset_Transform_Tool(NobleMetal_Array_Number);
        NobleMetal[NobleMetal_Array_Number].gameObject.SetActive(false);
        NobleMetal_Array_Number--;
        if (NobleMetal_Array_Number < 0)
        {
            NobleMetal_Array_Number = 2;
        }
        Cur_NobleMetal = NobleMetal[NobleMetal_Array_Number];
        Cur_NobleMetal.gameObject.SetActive(true);
        ContentPanel.Instance.SwitchIntroduceImageAndTiltle(NobleMetal_Array_Number);
    }

    public void Hide()
    {
        for (int i = 0; i < NobleMetal.Count; i++)
        {
            Reset_Transform_Tool(i);
            NobleMetal[i].gameObject.SetActive(false);
        }
        Cur_NobleMetal = null;
        ContentPanel.Instance.NobelMetalCanvas.DOFillAlpha(0, 0.1f);
        ContentPanel.Instance.JiXiangWuAnimator.SetBool("IsStart", false);
        MainPanel.Instance.audioSource.Play();
        PlayerManager.Instance.onPlayerGestureEvent -= OnPlayerGesture;
        IsFirstGrip = true;
    }

    public void Open()
    {
        if(Cur_NobleMetal!=null)
        {
            Reset_Transform_Tool(NobleMetal_Array_Number);
            Cur_NobleMetal.gameObject.SetActive(false);
        }
        PlayerManager.Instance.onPlayerGestureEvent -= OnPlayerGesture;
        PlayerManager.Instance.onPlayerGestureEvent += OnPlayerGesture;
        NobleMetal_Array_Number = 0;
        Cur_NobleMetal = NobleMetal[NobleMetal_Array_Number];
        ContentPanel.Instance.SwitchIntroduceImageAndTiltle(NobleMetal_Array_Number);
        MainPanel.Instance.audioSource.Pause();
        ContentPanel.Instance.JiXiangWuAnimator.SetBool("IsStart",true);
        ContentPanel.Instance.NobelMetalCanvas.DOFillAlpha(1, 0.5f).OnComplete(()=> { Cur_NobleMetal.gameObject.SetActive(true); });

    }

    private void Reset_Transform_Tool(int i)
    {
        NobleMetal[i].localPosition = Local_Position[i];
        NobleMetal[i].localRotation = Local_Rotasion[i];
        NobleMetal[i].localScale = Local_Scale[i];
    }
}
