using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <remarks>资源管理</remarks>
public class SourcesManager
{
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="pos"></param>
    /// <param name="angle"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static T LoadSources<T>(string path,Transform parent=null,Vector3? pos=null,Vector3? angle=null,Vector3? size=null) where T : ISources
    {
        T t = default(T);
        GameObject o = Resources.Load(path)as GameObject;
        if (o)
        {
            o = GameObject.Instantiate(o, parent);
            if (pos != null)
                o.transform.position = pos.Value;
            if (angle != null)
                o.transform.eulerAngles = angle.Value;
            if (size != null)
                o.transform.localScale = size.Value;
            t = o.GetComponent<T>();
        }
        else
        {
            Debug.Log(path + "路径资源不存在");
        }
        return t;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T LoadSources<T>(string path, GameObject target) where T : ISources
    {
        T t = default(T);
        GameObject o = Resources.Load(path) as GameObject;
        if (o)
        {
            if (target)
            {
                o = GameObject.Instantiate(o, target.transform);
                o.transform.position = target.transform.position;
                o.transform.eulerAngles = target.transform.eulerAngles;
                o.transform.localScale = target.transform.localScale;
            }
            else
            {
                o = GameObject.Instantiate(o);
            }
            t = o.GetComponent<T>();
        }
        else
        {
            Debug.Log(path + "路径资源不存在");
        }

        return t;
    }


    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="pos"></param>
    /// <param name="angle"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static T LoadSources<T>(T target, Transform parent = null, Vector3? pos = null, Vector3? angle = null, Vector3? size = null) where T : ISources
    {
        T t = default(T);
        GameObject o = target.Target;
        if (o)
        {
            o = GameObject.Instantiate(o, parent);
            if (pos != null)
                o.transform.position = pos.Value;
            if (angle != null)
                o.transform.eulerAngles = angle.Value;
            if (size != null)
                o.transform.localScale = size.Value;
            t = o.GetComponent<T>();
        }
        else
        {
            Debug.Log("资源不存在");
        }
        return t;
    }

    /// <summary>
    /// 销毁资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    public static void DestroySources<T>(T target) where T : ISources
    {
        GameObject.Destroy(target.Target);
    }

    /// <summary>
    /// 卸载资源清理缓存
    /// </summary>
    public static void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
}