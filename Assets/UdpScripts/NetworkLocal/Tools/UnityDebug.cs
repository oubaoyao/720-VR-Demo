using System;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommonTools;

public class UnityDebug : DebugControl
{
    private DebugBehaviour DebugObj;

    //如果
    public override string Debug_OutputPath => "Debug/";

    public UnityDebug()
    {
        DebugObj = new GameObject("[DebugGameObject]").AddComponent<DebugBehaviour>();
    }

    public override void LogNormal(string str)
    {
        DebugObj.AddDebug(str);
        Debug.Log(str);
    }

    public override void LogError(string str)
    {
        DebugObj.AddDebug(str);
        Debug.LogError(str);
    }

    public override void LogWarnning(string str)
    {
        DebugObj.AddDebug(str);
        Debug.LogWarning(str);
    }
}

public class DebugBehaviour : MonoBehaviour
{
    private List<string> debugList = new List<string>();

    public void AddDebug(string msg)
    {
        if (debugList.Count >= 100) debugList.Clear();
        debugList.Add(msg);
    }

    #region OnGUI绘制显示面板
    private Vector2 scrollPosition;
    private int visibleIndex = 0;
    private float visibleTimer = 0;
    private bool IsVisible;

    private void Update()
    {
        if (!IsVisible)
        {
            ////依照按键显示UI面板
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (visibleTimer == 0 || Time.time - visibleTimer < 1f)
            //    {
            //        visibleIndex++;
            //        visibleTimer = Time.time;

            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            IsVisible = true;

            //            visibleIndex = 0;
            //            visibleTimer = 0;
            //        }
            //    }
            //}

            //if (visibleIndex > 0 && (Time.time - visibleTimer >= 1f))
            //{
            //    visibleIndex = 0;
            //    visibleTimer = 0;
            //}
        }
    }

    void OnGUI()
    {
        if (!IsVisible)
        {
            return;
        }
        //绘制背景板
        Rect windowRect = new Rect(20, 20, Screen.width - (20 * 2), Screen.height * 0.5f);
        windowRect = GUILayout.Window(123456, windowRect, DrawConsoleWindow, "Console");
    }

    /// <summary>  
    /// Displays a window that lists the recorded logs.  
    /// </summary>  
    /// <param name="windowID">Window ID.</param>  
    void DrawConsoleWindow(int windowID)
    {
        //绘制信息列表
        DrawLogsList();
        //绘制工具按钮
        DrawToolbar();

        // Allow the window to be dragged by its title bar.  
        //Rect titleBarRect = new Rect(0, 0, 10000, 20);
        //GUI.DragWindow(titleBarRect);
    }

    /// <summary>  
    /// Displays a scrollable list of logs.  
    /// </summary>  
    void DrawLogsList()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Iterate through the recorded logs.  
        for (int i = 0; i < debugList.Count; i++)
        {
            GUI.contentColor = Color.white;
            GUILayout.Label(debugList[i]);
        }

        GUILayout.EndScrollView();

        // Ensure GUI colour is reset before drawing other components.  
        GUI.contentColor = Color.white;
    }

    /// <summary>  
    /// Displays options for filtering and changing the logs list.  
    /// </summary>  
    void DrawToolbar()
    {
        GUILayout.BeginHorizontal();

        GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        if (GUILayout.Button(clearLabel))
        {
            debugList.Clear();
        }

        GUIContent collapseLabel = new GUIContent("Close", "Hide repeated messages.");
        if (GUILayout.Button(collapseLabel))
        {
            IsVisible = false;
        }

        //GUILayout.Label(MTEngine.NetworkManager.Instance.PeerID.ToString());

        GUILayout.EndHorizontal();
    }
    #endregion
}