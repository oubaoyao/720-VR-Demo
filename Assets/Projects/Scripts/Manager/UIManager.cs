using MTFrame;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器
/// </summary>
public class UIManager
{
    /// <summary>
    /// 打开的面板队列
    /// </summary>
    private static Dictionary<WindowTypeEnum, List<BasePanel>> PanelDic = new Dictionary<WindowTypeEnum, List<BasePanel>>();
    /// <summary>
    /// Windown列表
    /// </summary>
    private static Dictionary<WindowTypeEnum, BaseWindow> windowsDic = new Dictionary<WindowTypeEnum, BaseWindow>();


    #region 创建面板
    /// <summary>
    /// 创建面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowTypeEnum"></param>
    /// <returns></returns>
    public static T CreatePanel<T>(Transform parent) where T : BasePanel
    {
        T t = default(T);
        T[] ts = Resources.LoadAll<T>("UIPanel");
        if (ts.Length > 0)
        {
            t = SourcesManager.LoadSources<T>(ts[0], parent);
        }
        else
        {
            Debug.Log("资源中不存在" + t.GetType().Name);
        }
        return t;
    }

    /// <summary>
    /// 创建面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowTypeEnum"></param>
    /// <returns></returns>
    public static T CreatePanel<T>(WindowTypeEnum windowTypeEnum, UIPanelStateEnum panelStateEnum= UIPanelStateEnum.Open) where T : BasePanel
    {
        T t = default(T);
        t = GetPanel<T>(windowTypeEnum);
        if (t == null)
        {
            T[] ts = Resources.LoadAll<T>("UIPanel");
            if (ts.Length > 0)
            {
                t = SourcesManager.LoadSources<T>(ts[0], windowsDic[windowTypeEnum].transform);
                if (!PanelDic.ContainsKey(windowTypeEnum))
                    PanelDic.Add(windowTypeEnum, new List<BasePanel> { t });
                else
                {
                    PanelDic[windowTypeEnum].RemoveNull();
                    PanelDic[windowTypeEnum].Add(t);
                }
            }
            else
            {
                Debug.Log("资源中不存在" + t.GetType().Name);
            }
        }
        switch (panelStateEnum)
        {
            case UIPanelStateEnum.Open:
                t.Open();
                break;
            case UIPanelStateEnum.Hide:
                t.Hide();
                break;
            default:
                break;
        }
        return t;
    }
    #endregion

    #region 面板控制
    /// <summary>
    /// 更改面板状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowTypeEnum"></param>
    /// <param name="showModeEnum"></param>
    public static T ChangePanelState<T>(WindowTypeEnum windowTypeEnum, UIPanelStateEnum panelStateEnum) where T : BasePanel
    {
        T t = default(T);
        t = GetPanel<T>(windowTypeEnum);
        if (t)
        {
            switch (panelStateEnum)
            {
                case UIPanelStateEnum.Open:
                    t.Open();
                    break;
                case UIPanelStateEnum.Hide:
                    t.Hide();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("面板不存在" + windowTypeEnum + "窗口中");
        }
        return t;
    }

    /// <summary>
    /// 删除面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowTypeEnum"></param>
    public static void RemovePanel(BasePanel basePanel)
    {
        BaseWindow baseWindow = basePanel.GetComponentInParent<BaseWindow>();
        if (baseWindow != null)
        {
            if (PanelDic.ContainsKey(baseWindow.Type))
            {
                PanelDic[baseWindow.Type].Remove(basePanel);
            }
        }
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="windowTypeEnum"></param>
    /// <returns></returns>
    public static T GetPanel<T>(WindowTypeEnum windowTypeEnum) where T :BasePanel
    {
        T panel = default(T);
        if (PanelDic.ContainsKey(windowTypeEnum))
        {
            panel = PanelDic[windowTypeEnum].FindLast((p) => p.name==typeof(T).ToString())as T;
        }

        return panel;
    }
    /// <summary>
    /// 获取窗口下所有面板
    /// </summary>
    /// <param name="windowTypeEnum"></param>
    /// <returns></returns>
    public static List<T> GetPanelS<T>(WindowTypeEnum windowTypeEnum) where T : BasePanel
    {
        List<T> panel = new List<T>();
        if (PanelDic.ContainsKey(windowTypeEnum))
        {
            panel = PanelDic[windowTypeEnum]as List<T> ;
        }

        return panel;
    }



    #endregion

    #region Window操作
    /// <summary>
    /// 添加window
    /// </summary>
    /// <param name="window"></param>
    public static void AddWindow(BaseWindow window)
    {
        if (!windowsDic.ContainsKey(window.Type))
        {
            windowsDic.Add(window.Type, window);
        }
    }
    /// <summary>
    /// 移除Window
    /// </summary>
    /// <param name="window"></param>
    public static void RemoveWindow(BaseWindow window)
    {
        if (windowsDic.ContainsKey(window.Type))
        {
            windowsDic.Remove(window.Type);
        }
    }
    #endregion

}