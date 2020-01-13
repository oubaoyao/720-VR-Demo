using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 查找场景对象工具
/// </summary>
public class FindTool
{
    /// <summary>
    /// 查找指定子物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindChildNode(Transform parent, string childName)
    {
        if (parent == null) return null;

        Transform result = parent.Find(childName);
        if (result == null && parent.childCount > 0)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                result = FindChildNode(parent.GetChild(i), childName);
                if (result != null)
                {
                    break;
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 查找指定父物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindParentNode(Transform child, string parentName)
    {
        if (child.parent == null) return null;

        Transform result = child.parent.name == parentName ? child.parent : null;
        if (result == null)
        {
            result = FindParentNode(child.parent, parentName);
        }
        return result;
    }
    /// <summary>
    /// 查找子物体中的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static T FindChildComponent<T>(Transform parent) where T : UnityEngine.Component
    {
        return parent.GetComponentInChildren<T>();
    }
    /// <summary>
    /// 查找指定子物体中的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <param name="isAdd"></param>
    /// <returns></returns>
    public static T FindChildComponent<T>(Transform parent, string childName, bool isAdd = false) where T : UnityEngine.Component
    {
        Transform child = FindChildNode(parent, childName);
        if (child == null) return null;

        T component = child.GetComponent<T>();
        if (isAdd && component == null) component = child.gameObject.AddComponent<T>();

        return component;
    }
    /// <summary>
    /// 查找子物体中的所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="list"></param>
    /// <param name="isAdd"></param>
    public static void FindChildComponents<T>(Transform parent, ref List<T> list, bool isAdd = false) where T : UnityEngine.Component
    {
        if (parent == null) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            T component = parent.GetChild(i).GetComponent<T>();
            if (component != null)
            {
                list.Add(component);
            }
            FindChildComponents<T>(parent.GetChild(i), ref list);
        }
        return;
    }
    /// <summary>
    /// 查找指定父物体中的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <param name="isAdd"></param>
    /// <returns></returns>
    public static T FindParentComponent<T>(Transform child, string parentName, bool isAdd = false) where T : UnityEngine.Component
    {
        Transform parent = FindParentNode(child, parentName);
        if (parent == null) return null;

        T component = parent.GetComponent<T>();
        if (isAdd && component == null) component = parent.gameObject.AddComponent<T>();

        return component;
    }
    /// <summary>
    /// 查找父物体中的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <param name="isAdd"></param>
    /// <returns></returns>
    public static T FindParentComponent<T>(Transform child) where T : UnityEngine.Component
    {
        if (child == null) return null;

        T result = child.GetComponent<T>();

        if (result == null)
        {
            result = FindParentComponent<T>(child.parent);
        }
        return result;
    }
    /// <summary>
    /// 查找物体上的组件，没有则添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T GetComponent<T>(Transform target) where T : UnityEngine.Component
    {
        if (target == null) return null;

        T result = target.GetComponent<T>();
        if (result == null) result = target.gameObject.AddComponent<T>();
        return result;
    }

    /// <summary>
    /// 获取指定名字的物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentName"></param>
    /// <returns></returns>
    public static T FindComponent<T>(string parentName) where T : Object
    {
        GameObject o = GameObject.Find(parentName);
        T result = o.GetComponent<T>();

        return result;
    }

    /// <summary>
    /// 获取指定名字的物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentName"></param>
    /// <returns></returns>
    public static T[] FindsSceneComponent<T>()where  T :Object
    {
        T[] result = GameObject.FindObjectsOfType<T>();

        return result;
    }

    /// <summary>
    /// 获取指定名字的物体所有组件包括子级
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentName"></param>
    /// <returns></returns>
    public static T[] FindComponentsInChildren<T>(string parentName) where T : Object
    {
        GameObject o = GameObject.Find(parentName);
        T[] result = o.GetComponentsInChildren<T>();

        return result;
    }
    /// <summary>
    /// 获取指定名字的物体所有组件包括父级
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentName"></param>
    /// <returns></returns>
    public static T[] FindComponentsInParent<T>(string parentName) where T : Object
    {
        GameObject o = GameObject.Find(parentName);
        T[] result = o.GetComponentsInParent<T>();

        return result;
    }
}