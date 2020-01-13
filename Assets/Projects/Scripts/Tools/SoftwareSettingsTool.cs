using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 软件前置工具
/// </summary>
public class SoftwareSettingsTool : MainBehavior, ISerializeButton
{
    public static SoftwareSettingsTool Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public List<string> SerializeButtonName
    {
        get
        {
            return new List<string>()
            {
                "设置","初始化"
            };
        }
    }

    public List<Action> SerializeButtonMethod
    {
        get
        {
            return new List<Action>()
            {
                SetPlayerSettings,InitPlayerSettings
            };
        }
    }

    [Header("公司名称")]
    /// <summary>
    /// 公司名称
    /// </summary>
    public string companyName = "MTKJ";

    [Header("产品名称")]
    /// <summary>
    /// 产品名称
    /// </summary>
    public string productName;

#if UNITY_STANDALONE_WIN

    [Header("是否开启软件前置")]
    /// <summary>
    /// 是否开启软件前置
    /// </summary>
    public bool isOpenPrepose;
    [Header("是否后台运行")]
    /// <summary>
    /// 是否后台运行
    /// </summary>
    public bool runInBackground = false;

#if UNITY_EDITOR
    [Header("显示分辨率对话框")]
    /// <summary>
    /// 显示分辨率对话框
    /// </summary>
    public ResolutionDialogSetting dialogSetting = ResolutionDialogSetting.Disabled;

#endif

#endif



    public void SetPlayerSettings()
    {
#if UNITY_EDITOR

        Debug.Log("设置");
        if (productName != null && productName != "" && productName != string.Empty)
            PlayerSettings.productName = productName;
        PlayerSettings.companyName = companyName;

#if UNITY_STANDALONE_WIN

        PlayerSettings.runInBackground = runInBackground;
        PlayerSettings.displayResolutionDialog = dialogSetting;
#endif
#endif
    }
    public void InitPlayerSettings()
    {
#if UNITY_EDITOR

        productName = PlayerSettings.productName;
        companyName = PlayerSettings.companyName;

#if UNITY_STANDALONE_WIN
        runInBackground = PlayerSettings.runInBackground;
        dialogSetting = PlayerSettings.displayResolutionDialog;
#endif
#endif
    }


#if UNITY_STANDALONE_WIN

    [DllImport("User32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("User32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

    void FixedUpdate()
    {


        if (!isOpenPrepose) return;

        // apptitle自己到查看进程得到，一般就是程序名不带.exe
        // 或者用spy++查看
        IntPtr hwnd = FindWindow(null, productName);

        // 如果没有找到，则不做任何操作（找不到一般就是apptitle错了）
        if (hwnd == IntPtr.Zero)
        {
            productName = "NongHangKinect";
            MainData.Instance.isMainWindow = true;
            MainPanel.Instance?.audioSource.Play();
            return;
        }

        IntPtr activeWndHwnd = GetForegroundWindow();

        // 当前程序不是活动窗口，则设置为活动窗口
        if (hwnd != activeWndHwnd)
        {
            ShowWindowAsync(hwnd,3);
            SetForegroundWindow(hwnd);
        }
    }

#endif
}