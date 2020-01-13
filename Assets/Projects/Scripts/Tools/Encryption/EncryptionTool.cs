using MT.Key;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MTFrame.Tool.ViKey;
using UnityEngine;
using UnityEngine.UI;
using MTFrame.MTFile;
using MTFrame.MTEvent;

/// <summary>
/// 加密工具
/// </summary>
public class EncryptionTool
{
    private static EncryptionTool instance;
    /// <summary>
    /// 单例
    /// </summary>
    public static EncryptionTool Instance
    {
        get
        {
            if (instance == null)
                instance = new EncryptionTool();
            return instance;
        }
    }

    /// <summary>
    ///加密数据
    /// </summary>
    public EncryptionData encryptionData = new EncryptionData();

    public System.Action<bool, long> OnTimeCall;
    public System.Action<bool, MTKeyStateType> OnMTKeyCall;
    public System.Action<bool> OnViKeyCall;


    //机密信息反馈的UI画布
    private Canvas EncryptionCanvas;
    //Logo水印图片
    private RawImage LogoWatermarkPhoto;
    //时间限制水印图片
    private RawImage TimeWatermarkPhoto;

    //网络加密水印图片
    private RawImage MTKeyWatermarkPhoto;
    //加密狗加密水印图片
    private RawImage ViKeyWatermarkPhoto;
    //提示图片
    private RawImage tipsBGPhoto;
    //提示文字
    private Text tipsText;
    //实时网络加密回调数据
    private MTKeyStateType mtKeyparameter = MTKeyStateType.网络加密成功;
    private bool isRend;

#if UNITY_STANDALONE_WIN
    //实时加密狗回调数据
    private bool isViKeyConnected = true;
#endif

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        string path = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        path = Application.streamingAssetsPath + "/ManagerData/Encryption/加密文件.txt";
        FileObject fileObject =FileManager.Read(path, FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);
        SetInfo(fileObject);
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/ManagerData/Encryption/加密文件.txt";
        if (!File.Exists(FileManager.two_EncryptPath(path, ".mt")))
        {
            path = Application.streamingAssetsPath + "/ManagerData/Encryption/加密文件.txt";
            FileManager.ReadWeb(path, (fileObject) =>
            {
                SetInfo(fileObject);
                Save();
            }, FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);
        }
        else
        {
        FileObject fileObject =FileManager.Read(path, FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);
        SetInfo(fileObject);
        }
#endif

    }
    private void SetInfo(FileObject fileObject)
    {
        if (!fileObject.isError)
        {
            Debug.Log("加密文件丢失程序出错关闭程序");
            Main.Instance.Quit();
            return;
        }
        string data = System.Text.Encoding.UTF8.GetString(fileObject.Buffet);
        if (!data.IsJson())
        {
            Debug.Log("加密文件内容丢失程序出错关闭程序");
            Main.Instance.Quit();
            return;
        }

        encryptionData = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptionData>(data);
        isRend = true;

        if (!encryptionData.mTKeyEncryptionData.isMTKey && !encryptionData.isLogo && !encryptionData.vikeyEncryptionData.isViKey)
            return;

        EncryptionCanvas = new GameObject("[EncryptionCanvas]").AddComponent<Canvas>();//创建UI画布
        (EncryptionCanvas.transform as RectTransform).sizeDelta = new Vector2(encryptionData.W, encryptionData.H);
        CanvasScaler canvasScaler = EncryptionCanvas.gameObject.AddComponent<CanvasScaler>();//创建UI画布必要组件
        EncryptionCanvas.gameObject.AddComponent<GraphicRaycaster>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(encryptionData.W, encryptionData.H);
        EncryptionCanvas.sortingOrder = 99;//该变画布层级

        GameObject.DontDestroyOnLoad(EncryptionCanvas.gameObject);//设置画布不被销毁

        if (encryptionData.isLogo)
        {
            LogoWatermarkPhoto = SetWatermarkPhoto(encryptionData.LogoWatermark, "LogoImage");//创建Logo水印
        }
        if (encryptionData.timeEncryptionData.isTime)
        {
            TimeWatermarkPhoto = SetWatermarkPhoto(encryptionData.TimeWatermark, "TimeImage");//创建时间限制水印
            TimeWatermarkPhoto.enabled = false;
        }
        if (encryptionData.mTKeyEncryptionData.isMTKey)
        {
            MTKeyWatermarkPhoto = SetWatermarkPhoto(encryptionData.MTKeyWatermark, "MTKeyImage");//创建网络加密水印
            MTKeyWatermarkPhoto.enabled = false;
        }
#if UNITY_STANDALONE_WIN
            if (encryptionData.vikeyEncryptionData.isViKey)
            {
                ViKeyWatermarkPhoto = SetWatermarkPhoto(encryptionData.ViKeyWatermark, "ViKeyImage");//创建加密狗水印
                ViKeyWatermarkPhoto.enabled = false;
            }
#endif
        EncryptionCanvas.renderMode = RenderMode.ScreenSpaceOverlay;//改变画布模式
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 3, OnUpdate, true);//更新加密水印的状态

    }
    

    /// <summary>
    /// 开启加密
    /// </summary>
    public void OpenEncryption()
    {
        if (encryptionData.mTKeyEncryptionData.isMTKey)
        {
            //监听网络加密反馈
            MTKeyManager.Instance?.OnAddLisenter(encryptionData.mTKeyEncryptionData.MTKeyServiceNumber, encryptionData.mTKeyEncryptionData.MTKeyProjectName, encryptionData.mTKeyEncryptionData.isMTKey, (parameter) =>
            {
                Debug.Log(parameter);
                mtKeyparameter = parameter;

            });
        }
#if UNITY_STANDALONE_WIN
        if (encryptionData.vikeyEncryptionData.isViKey)
        {
            //监听加密狗加密反馈
            ViKeyManager.Instance.OnAddLisenter(new VikeyEncryptionInfo {
                vikeyEncryptionType = encryptionData.vikeyEncryptionData.vikeyEncryptionType,
                Key = encryptionData.vikeyEncryptionData.ViKeyNumber,
                ProjectName = encryptionData.vikeyEncryptionData.ViKeyProjectName }, 
                (isConnected) =>
            {
                Debug.Log("加密狗加密=" + isConnected);
                isViKeyConnected = isConnected;
            });
        }
#endif
    }



    //更新
    private void OnUpdate()
    {
        if (encryptionData.timeEncryptionData.isTime)
        {
            if (encryptionData.timeEncryptionData.systemDateTime < DateTime.Now)
                encryptionData.timeEncryptionData.systemDateTime = DateTime.Now;
            else
                encryptionData.timeEncryptionData.systemDateTime = encryptionData.timeEncryptionData.systemDateTime.AddSeconds(3);  
            
            if (encryptionData.timeEncryptionData.LimitDateTime < DateTime.Now)
                SetTimeEncryption();

            else if (encryptionData.timeEncryptionData.LimitDateTime < encryptionData.timeEncryptionData.systemDateTime)
                SetTimeEncryption();
            else
                OnTimeCall?.Invoke(false, (encryptionData.timeEncryptionData.LimitDateTime - DateTime.Now).Ticks);

        }

        if (encryptionData.mTKeyEncryptionData.isMTKey)
        {
            switch (mtKeyparameter)
            {
                case MTKeyStateType.网络加密成功:
                    MTKeyWatermarkPhoto.enabled = false;
                    OnMTKeyCall?.Invoke(false, mtKeyparameter);
                    tipsBGPhoto?.gameObject.SetActive(false);
                    break;
                case MTKeyStateType.网络加密失败:
                    MTKeyWatermarkPhoto.enabled = true;
                    OnMTKeyCall?.Invoke(true, mtKeyparameter);
                    tipsBGPhoto?.gameObject.SetActive(false);
                    break;
                case MTKeyStateType.网络出错:
                    MTKeyWatermarkPhoto.enabled = true;
                    SetTips("网络出错！！！\n请检查网络连接是否正常");
                    OnMTKeyCall?.Invoke(true, mtKeyparameter);
                    break;
                case MTKeyStateType.网络断开:
                    MTKeyWatermarkPhoto.enabled = true;
                    SetTips("网络断开！！！\n请检查网络连接是否正常");
                    OnMTKeyCall?.Invoke(true, mtKeyparameter);
                    break;
                default:
                    break;
            }
        }
#if UNITY_STANDALONE_WIN
        if (encryptionData.vikeyEncryptionData.isViKey)
        {
            ViKeyWatermarkPhoto.enabled = !isViKeyConnected;
            OnViKeyCall?.Invoke(isViKeyConnected);
        }
        
#endif
    }

    private void SetTimeEncryption()
    {
        TimeWatermarkPhoto.enabled = true;
        Debug.Log("软件已过期");
        if (encryptionData.timeEncryptionData.CloseTime != 0)
        {
            Debug.Log(encryptionData.timeEncryptionData.CloseTime + "秒后退出软件");
            CloseEncryption();
            EventManager.AddUpdateListener(UpdateEventEnumType.Update, "LimitedTimeUpdate", (t) =>
            {
                OnTimeCall?.Invoke(true, (long)(encryptionData.timeEncryptionData.CloseTime - t));
                if (t >= encryptionData.timeEncryptionData.CloseTime)
                    Main.Instance.Quit();
            });
        }

        else
        {
            OnTimeCall?.Invoke(true, 0);
        }
    }

    /// <summary>
    /// 保存加密参数
    /// </summary>
    public void Save()
    {
        if (!isRend)
            return;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        string path = Application.streamingAssetsPath + "/ManagerData/Encryption/加密文件.txt";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath + "/ManagerData/Encryption/加密文件.txt";
#endif
        string data = Newtonsoft.Json.JsonConvert.SerializeObject(encryptionData);
        byte[] buffe = System.Text.Encoding.UTF8.GetBytes(data);
        FileManager.Write(path, buffe,FileFormatType.txt, EncryptModeType.two_LevelByteEncryption);
    }

    /// <summary>
    /// 设置水印图片
    /// </summary>
    /// <param name="watermarkData"></param>
    /// <returns></returns>
    private RawImage SetWatermarkPhoto(WatermarkData watermarkData, string watermarkName)
    {
        RawImage watermarkPhoto = new GameObject(watermarkName).AddComponent<RawImage>();
        watermarkPhoto.rectTransform.SetParent(EncryptionCanvas.transform);
        watermarkPhoto.rectTransform.localScale = Vector3.one;
        watermarkPhoto.rectTransform.anchorMin = watermarkData.anchorMin.Get();
        watermarkPhoto.rectTransform.anchorMax = watermarkData.anchorMax.Get();
        watermarkPhoto.rectTransform.localPosition = new Vector3(watermarkData._Rect.x, watermarkData._Rect.y, 0);
        watermarkPhoto.rectTransform.sizeDelta = new Vector2(watermarkData._Rect.z, watermarkData._Rect.w);
        watermarkPhoto.texture = watermarkData._TextureData.Get();
        watermarkPhoto.raycastTarget = false;

        return watermarkPhoto;
    }

    /// <summary>
    /// 设置提示
    /// </summary>
    /// <param name="watermarkData"></param>
    /// <returns></returns>
    private void SetTips(string parameter)
    {
        if (tipsBGPhoto == null)
        {
            tipsBGPhoto = new GameObject("Tips").AddComponent<RawImage>();
            tipsBGPhoto.rectTransform.SetParent(EncryptionCanvas.transform);
            tipsBGPhoto.rectTransform.localScale = Vector3.one;
            tipsBGPhoto.rectTransform.localPosition = Vector3.zero;
            tipsBGPhoto.rectTransform.sizeDelta = new Vector2(Screen.currentResolution.width, 200);
            tipsBGPhoto.color = new Color(1, 0, 0, 0.4f);
            tipsBGPhoto.raycastTarget = false;
        }
        if (tipsText == null)
        {
            tipsText = new GameObject("TipsText").AddComponent<Text>();
            tipsText.rectTransform.SetParent(tipsBGPhoto.transform);
            tipsText.rectTransform.localScale = Vector3.one;
            tipsText.rectTransform.localPosition = Vector3.zero;
            tipsText.rectTransform.sizeDelta = new Vector2(Screen.currentResolution.width, 200);
            tipsText.fontStyle = FontStyle.Bold;
            tipsText.fontSize = 60;
            tipsText.alignment = TextAnchor.MiddleCenter;
            tipsText.color = Color.black;
            tipsText.font = Resources.Load<Font>("Font/SIMHEI");
        }
        tipsBGPhoto?.gameObject.SetActive(true);
        tipsText.text = parameter;
    }

    /// <summary>
    /// 关闭加密
    /// </summary>
    public void CloseEncryption()
    {
        //Save();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, OnUpdate);
#if UNITY_STANDALONE_WIN
        if (encryptionData.vikeyEncryptionData.isViKey)
        {
            ViKeyManager.Instance.Close();
        }
#endif
    }

}
