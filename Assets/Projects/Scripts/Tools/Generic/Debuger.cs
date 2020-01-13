using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
/// <summary>
/// 日志管理
/// </summary>
public class Debuger : MonoBehaviour
{
    #region 日志数据类
    /// <summary>
    /// 日志数据
    /// </summary>
    private class LogData
    {
        public string Content;
        public string StackTrace;
        public LogType LogType;
        public DateTime LogTime;

        public override string ToString()
        {
            if (StackTrace != "")
            {
                string txt = string.Format("{0} :  信息类型 = {1}\r\n输出信息 = {2}\r\n输出位置 = {3}\r\n-----------------------------------------------------------\r\n",
                    LogTime.ToString(), LogType.ToString(), Content, StackTrace);
                return txt;
            }
            else
            {
                string txt = string.Format("{0} :  信息类型 = {1}\r\n输出信息 = {2}\r\n-----------------------------------------------------------\r\n",
                  LogTime.ToString(), LogType.ToString(), Content);
                return txt;
            }
        }
    }
    #endregion

    private static Debuger instance;
    public static Debuger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("[Debuger]").AddComponent<Debuger>();
                GameObject.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    /// <summary>
    /// 是否输出日志信息
    /// </summary>
    public static bool IsDebug = true;
    /// <summary>
    /// 是否打印日志文件
    /// </summary>
    public static bool IsWirteLog = true;
    /// <summary>
    /// 是否已创建输出文件
    /// </summary>
    private static bool isCreatFile = false;
    /// <summary>
    /// 输出的文件序号
    /// </summary>
    private static int FileIndex = 0;

#if  UNITY_EDITOR
    public static Action<object> Log = Debug.Log;
    public static Action<object> LogWarnning = Debug.LogWarning;
    public static Action<object> LogError = Debug.LogError;
#else
    public static void Log(object message) { if(IsDebug) Debug.Log(message); }
    public static void LogWarnning(object message) { if(IsDebug) Debug.LogWarning(message); }
    public static void LogError(object message) { if(IsDebug) Debug.LogError(message); }
#endif

    /// <summary>
    /// 输出的日志数据队列
    /// </summary>
    private Queue<LogData> OutputLogQueue=new Queue<LogData>();
    /// <summary>
    /// 接收的日志数据队列
    /// </summary>
    private Queue<LogData> ReceiveLogQueue=new Queue<LogData>();
    /// <summary>
    /// 当前时间
    /// </summary>
    private DateTime dateTime;

    /// <summary>
    /// 最大日志队列数
    /// </summary>
    private int MaxLogQueueCount { get { return 100; } }
    /// <summary>
    /// 输出的文件夹位置
    /// </summary>
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private string DirectoryPath { get{ return  Application.dataPath + "/Output/" + dateTime.Year + dateTime.Month + dateTime.Day; } }
#elif UNITY_ANDROID
    private string DirectoryPath { get{ return  Application.persistentDataPath + "/Output/" + dateTime.Year + dateTime.Month + dateTime.Day; } }
#endif
    /// <summary>
    /// 输出的文件名称
    /// </summary>
    private string FileName { get { return "Log_" + dateTime.Year + dateTime.Month + dateTime.Day + "_" + FileIndex + ".ini"; } }
    /// <summary>
    /// 显示面板的按键
    /// </summary>
    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.A,
        KeyCode.B,
    };

    private Debuger()
    {

    }

    public void Init()
    {
        dateTime = DateTime.Now;

#if UNITY_EDITOR
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
        Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
        Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
#else
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
#endif
        //Application.logMessageReceived += LogCallback;
        Application.logMessageReceivedThreaded += LogCallback;
    }

    private void Update()
    {
        UpdateLog();
        ShowLogPanel();
    }

    /// <summary>
    /// 在销毁时保存日志数据并清除
    /// </summary>
    public void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= LogCallback;

#if !UNITY_EDITOR
        if(IsWirteLog) WriteLog();
#endif

        OutputLogQueue.Clear();
        OutputLogQueue = null;
        instance = null;
    }
    
    /// <summary>
    /// 打印日志
    /// </summary>
    private void WriteLog()
    {
        if (!isCreatFile)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (!Directory.Exists( Application.dataPath + "/Output/")) Directory.CreateDirectory( Application.dataPath + "/Output/");
#elif UNITY_ANDROID
            if (!Directory.Exists(Application.persistentDataPath + "/Output/")) Directory.CreateDirectory(Application.persistentDataPath + "/Output/");
#endif
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
            while (File.Exists(DirectoryPath + "/" + FileName)) FileIndex++;

            isCreatFile = true;
            File.Create(DirectoryPath + "/" + FileName).Close();
        }

        FileStream fileStream = File.Open(DirectoryPath + "/" + FileName, FileMode.Open);
        LogData data = null;
        string message = "";
        while (OutputLogQueue.Count > 0)
        {
            data = OutputLogQueue.Dequeue();
            message += data.ToString();
            //buf = System.Text.ASCIIEncoding.UTF8.GetBytes(data.ToString());
            //fileStream.Write(buf, 0, buf.Length);
        }

        byte[] buf = System.Text.ASCIIEncoding.UTF8.GetBytes(message);
        fileStream.Write(buf, 0, buf.Length);
        fileStream.Close();
        fileStream = null;
    }

    /// <summary>
    /// 处理接收到的输出信息
    /// </summary>
    private void UpdateLog()
    {
        while (ReceiveLogQueue.Count > 0)
        {
            //处理输出的文字格式
            LogData log = ReceiveLogQueue.Dequeue();
            StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(log.LogType);

#region 处理Log型输出的格式
            if (stackTraceLogType == StackTraceLogType.Full && log.LogType == LogType.Log)
            {
                int skipIndex = 0; 
#if UNITY_EDITOR
                string[] str = log.StackTrace.Split('\n');
                log.StackTrace = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i].Contains("(Mono JIT Code)"))
                    {
                        String[] strArr = str[i].Split(new string[] { "(Mono JIT Code)" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArr.Length >= 2 && strArr[1].Contains(".cs:"))
                        {
                            //跳过前三行
                            if (skipIndex >= 3) log.StackTrace += "\r\n" + strArr[1];
                            else skipIndex++;
                        }
                    }
                }
#else
                string[] str =  log.StackTrace.Split(new string[] { "(Mono JIT Code)" }, StringSplitOptions.RemoveEmptyEntries);            
                log.StackTrace = "";
                for (int i = 1; i < str.Length - 1; i++)
                {
                    //跳过前三行
                    if (skipIndex >= 3) log.StackTrace += "\r\n" + str[i];
                    else skipIndex++;
                }
#endif
            }
#endregion

            if (OutputLogQueue.Count >= MaxLogQueueCount)
            {
#if !UNITY_EDITOR
                if (IsWirteLog) WriteLog();
#endif
                OutputLogQueue.Clear();
            }
            OutputLogQueue.Enqueue(log);
        }
    }

    /// <summary>
    /// 获取系统日志数据
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        LogData log = new LogData();
        log.Content = condition;
        log.StackTrace = stackTrace;
        log.LogType = type;
        log.LogTime = DateTime.Now;
        ReceiveLogQueue.Enqueue(log);
    }

#region OnGUI绘制显示面板

    private Vector2 scrollPosition;
    private int visibleIndex = 0;
    private float visibleTimer = 0;
    /// <summary>
    /// 是否正在显示UI面板
    /// </summary>
    public bool IsVisible = false;

    /// <summary>
    /// 显示输出面板
    /// </summary>
    private void ShowLogPanel()
    {
        if (!IsVisible)
        {
            //依照按键显示UI面板
            if (Input.GetKeyDown(keyCodes[visibleIndex]))
            {
                if (visibleTimer == 0 || Time.time - visibleTimer < 1f)
                {
                    visibleIndex++;
                    visibleTimer = Time.time;
                }

                if (visibleIndex >= keyCodes.Length)
                {
                    IsVisible = true;
                    visibleIndex = 0;
                    visibleTimer = 0;
                }
            }

            if (visibleIndex > 0 && (Time.time - visibleTimer >= 1f))
            {
                visibleIndex = 0;
                visibleTimer = 0;
            }
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
        foreach (LogData log in OutputLogQueue)
        {
            GUI.contentColor = Color.green;
            switch (log.LogType)
            {
                case LogType.Error:
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    GUI.contentColor = Color.yellow;
                    break;
                case LogType.Log:
                    GUI.contentColor = Color.white;
                    break;
                case LogType.Exception:
                    GUI.contentColor = Color.red;
                    break;
                default:
                    break;
            }
            GUILayout.Label(log.ToString());
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
            OutputLogQueue.Clear();
        }

        GUIContent collapseLabel = new GUIContent("Close", "Hide repeated messages.");
        if (GUILayout.Button(collapseLabel))
        {
            IsVisible = false;
        }

        GUILayout.EndHorizontal();
    }
#endregion
}
