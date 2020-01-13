using MTFrame.MTKinect;
using UnityEngine;
/// <summary>
/// 入口类
/// </summary>
public class Main : MonoBehaviour
{
    public static Main Instance;
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    [Header("屏幕分辨率")]
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    /// <summary>
    /// 是否全屏
    /// </summary>
    [Header("是否全屏")]
    public bool fullScreen = true;

    [Header("是否显示鼠标图标")]
    [SerializeField]
    
    private bool isCursor=true;
    public bool IsCursor
    {
        get { return isCursor; }
        set
        {
            isCursor = value;
            Cursor.visible = isCursor;
        }
    }

#endif
    [Header("是否显示Debug面板")]
    [SerializeField]
    private bool isDebug;
    public bool IsDebug
    {
        get { return isDebug; }
        set
        {
            isDebug = value;
            Debuger.Instance.IsVisible = isDebug;
        }
    }


    void Awake()
    {
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

#if UNITY_STANDALONE_WIN
        Screen.SetResolution(resolution.x, resolution.y, fullScreen);
#endif

        //Debuger.Instance.Init();//DeBug初始化 
        /*EncryptionTool.Instance.Init();*///初始化加密工具
        
        PoolManager.Init();//对象池初始化
        AudioManager.Init();//音效初始化
        DOTween.Instance.Init();
        //开启体感
        //KINECTManager.Instance.Init();
    }
    private void Start()
    {
        //EncryptionTool.Instance.OpenEncryption();
        Init();
    }
    /// <summary>
    ///初始化
    /// </summary>
    public void Init()
    {
        IsDebug = isDebug;

#if UNITY_STANDALONE_WIN
        IsCursor = isCursor;
#endif
        //在这里更改场景入口
        //StateManager.ChangeState(new LoadingState());
        StateManager.ChangeState(new VRState());

    }

    private void Update()
    {

    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        //EncryptionTool.Instance.CloseEncryption();
        TimeTool.Instance.Dispose();
    }

    /// <summary>
    /// 退出
    /// </summary>
    public void Quit()
    {
        Close();
        Debug.Log("退出");
        Application.Quit();
    }

    private void OnDestroy()
    {
        Close();
    }
    void OnApplicationQuit()
    {
        Close();
    }
}
