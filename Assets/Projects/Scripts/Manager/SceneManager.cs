using MTFrame. MTScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
///加载场景管理
/// </summary>
public class SceneManager
{
    public static UnityLocal_SceneLoading unityLocal_SceneLoadingl=new UnityLocal_SceneLoading();


    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="levelName">场景名字</param>
    /// <param name="loadSceneMode">加载类型</param>
    public static void LoadScene(string levelName,LoadingModeType loadingModeType,LoadingType loadingType= LoadingType.Single)
    {
        switch (loadingModeType)
        {
            case LoadingModeType.UnityLocal:
                unityLocal_SceneLoadingl.LoadScene(levelName, loadingType);
                break;
            case LoadingModeType.Octree:
                UnityEngine.Debug.Log("我还没写哦~~~");
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="levelName">场景名字</param>
    /// <param name="startLoading">开始回调</param>
    /// <param name="stayLoading">进程回调</param>
    /// <param name="endLoading">结束回调</param>
    /// <param name="loadSceneMode">加载类型</param>
    public static void LoadSceneAsync(string levelName, LoadingModeType loadingModeType, Action _startLoading = null, Action<float> _updateLoading = null, Action _endLoading = null,
     LoadingType loadingType= LoadingType.Single)
    {
        switch (loadingModeType)
        {
            case LoadingModeType.UnityLocal:
                MainSystem.Instance.OnStartCoroutine( unityLocal_SceneLoadingl.LoadSceneAsync(levelName, _startLoading, _updateLoading, _endLoading, loadingType));
                break;
            case LoadingModeType.Octree:
                UnityEngine.Debug.LogError("我还没写哦~~~");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 卸载场景
    /// </summary>
    /// <param name="levelName">要卸载场景的名字</param>
    /// <param name="endUnload">结束卸载</param>
    /// <param name="startUnload">开始卸载</param>
    public static void UnloadScene(string levelName, LoadingModeType loadingModeType, Action _startUnload = null, Action<float> _updateUnload = null, Action _endUnload = null)
    {
        switch (loadingModeType)
        {
            case LoadingModeType.UnityLocal:
                MainSystem.Instance.OnStartCoroutine(unityLocal_SceneLoadingl.UnloadScene(levelName, _startUnload, _updateUnload,_endUnload));
                break;
            case LoadingModeType.Octree:
                UnityEngine.Debug.LogError("我还没写哦~~~");
                break;
            default:
                break;
        }
    }

}