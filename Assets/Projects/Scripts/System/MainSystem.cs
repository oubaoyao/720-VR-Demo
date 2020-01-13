using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 更新系统
/// </summary>
public class MainSystem : MainBehavior
{
    private static MainSystem updateSystem;
    public static MainSystem Instance
    {
        get { if (updateSystem == null) updateSystem = new GameObject("[MainSystem]").AddComponent<MainSystem>(); return updateSystem; }
    }
    protected override void Awake()
    {
        base.Awake();
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// 开启携程
    /// </summary>
    /// <param name="enumerator"></param>
    public void OnStartCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }
    /// <summary>
    /// 暂停携程
    /// </summary>
    public void OnStopCoroutine(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
    }


    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="OnCall"></param>
    public void ReadWebData(string path, Action<UnityWebRequest> OnCall)
    {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        StartCoroutine(readWebData("file://" + path, OnCall));
#elif UNITY_ANDROID
        StartCoroutine(readWebData(path, OnCall));
#endif
    }

    private IEnumerator readWebData(string path, Action<UnityWebRequest> OnCall)
    {
        UnityWebRequest myRed = UnityWebRequest.Get(path);
        yield return myRed.SendWebRequest();
        if (myRed.error != null)
        {
            Debug.Log("错误==" + path);
        }
        else
        {
            OnCall?.Invoke(myRed);
        }
    }



}
