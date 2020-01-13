using UnityEngine;

/// <summary>
/// 主要的行为
/// </summary>
public class MainBehavior : MonoBehaviour, ISources
{
    /// <summary>
    /// 自身目标
    /// </summary>
    public GameObject Target
    {
        get
        {
            return gameObject;
        }
    }
    /// <summary>
    /// 实例化时最先调用 只调用一次
    /// </summary>
    protected virtual void Awake()
    {
    }
    /// <summary>
    /// 实例化开始时调用 只调用一次
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// 每次启用是调用
    /// </summary>
    protected virtual void OnEnable()
    {

    }
    /// <summary>
    /// 销毁时调用
    /// </summary>
    protected virtual void OnDestroy()
    {

    }
    /// <summary>
    /// 每次禁用时调用
    /// </summary>
    protected virtual void OnDisable()
    {

    }
}