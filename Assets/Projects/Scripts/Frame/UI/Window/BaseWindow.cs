using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为场景物体的存储窗口
/// </summary>
[ExecuteInEditMode]
public abstract class BaseWindow : MainBehavior
{
    /// <summary>
    /// 窗口类型
    /// </summary>
    public abstract WindowTypeEnum Type { get; }

    protected override void Awake()
    {
        name = GetType().Name;
        base.Awake();
        UIManager.AddWindow(this);//添加窗口
        Canvas canvas= GetComponent<Canvas>();
        if (canvas)
            canvas.worldCamera = Camera.main;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIManager.RemoveWindow(this);//去除窗口
    }
}