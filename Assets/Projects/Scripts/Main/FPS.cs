using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 帧数
/// </summary>
public class FPS : MainBehavior,ISerializeButton
{
    public List<string> SerializeButtonName
    {
        get
        {
            return new List<string> {"Init" };
        }
    }

    public List<Action> SerializeButtonMethod
    {
        get
        {
            return new List<Action> { Init };
        }
    }

    private void Init()
    {
        name = "[FPS]";
    }

    public static FPS Instance;

    [Header("OnGUI for frame rate---")]
    //[Header("是否显示")]
    public bool isShow =true;
    [Header("位置大小")]
    public Rect rect = new Rect(300f, 200f, 500f, 300f);
    [Header("颜色")]
    public Color textColor = Color.white;
    [Header("字体模式")]
    public FontStyle fontStyle = FontStyle.Normal; 
    [Header("字体大小")]
    public int guiFontSize = 50;
    [Header("更新频率")]
    public float updateInterval = 0.5F;
    private GUIStyle style = new GUIStyle();

    private double lastInterval;
    private int frames = 0;
    private float fps;

    protected override void Awake()
    {
        Instance = this;
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }


    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="count"></param>
    public void SetFPS(int count)
    {
        Application.targetFrameRate = count;
    }

    //OnGUI显示帧率图像
    private void OnGUI()
    {
        if (!isShow) return;
        style.fontSize = guiFontSize;
        style.fontStyle = fontStyle;
        style.normal.textColor = textColor;
        GUI.Label(rect,"fps:"+ fps.ToString("00"), this.style);
    }
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
