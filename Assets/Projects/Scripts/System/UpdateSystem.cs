using MTFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 更新系统
/// </summary>
public class UpdateSystem : MainBehavior
{
    private static  UpdateSystem updateSystem;
    private static bool isupdateSystem=false ;
    public static UpdateSystem Instance
    {
        get
        {
            if (isupdateSystem)
                return null;
            updateSystem = FindObjectOfType<UpdateSystem>();
            if (updateSystem == null)
                updateSystem = new GameObject("[UpdateSystem]").AddComponent<UpdateSystem>();
            return updateSystem; }
    }

    private List<object> updates = new List<object>();

    private List<object> mainUpdates = new List<object>();
    private List<object> fixedUpdates = new List<object>();
    private List<object> lateUpdates = new List<object>();


    protected override void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        base.Awake();
    }

    /// <summary>
    /// 添加更新对象
    /// </summary>
    /// <typeparam name="T">更新对象类型</typeparam>
    /// <param name="update">对象</param>
    public void Add<T>(T update) where T : IUpdate
    {
        updates.Add(update);

        mainUpdates = GetAllType<IMainUpdate>();
        fixedUpdates = GetAllType<IFixedUpdate>();
        lateUpdates = GetAllType<ILateUpdate>();
        updates.RemoveNull();
    }


    /// <summary>
    /// 移除更新对象
    /// </summary>
    /// <typeparam name="T">更新对象类型</typeparam>
    /// <param name="update">对象</param>
    public void Remove<T>(T update) where T : IUpdate
    {
        updates.Remove(update);

        mainUpdates = GetAllType<IMainUpdate>();
        fixedUpdates = GetAllType<IFixedUpdate>();
        lateUpdates = GetAllType<ILateUpdate>();
        updates.RemoveNull();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        updates?.Clear();
        mainUpdates?.Clear();
        fixedUpdates?.Clear();
        lateUpdates?.Clear();
    }
   /// <summary>
   /// 转化类型
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <returns></returns>
    private List<object> GetAllType<T>() where T : IUpdate
    {
        var type = typeof(T);

        List<object> lists = new List<object>();
        foreach (IUpdate iu in updates)
        {
            if (type.IsAssignableFrom(iu.GetType()))
            {
                lists.Add(iu);
            }
        }

        return lists;
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < fixedUpdates.Count; i++)
        {
            (fixedUpdates[i] as IFixedUpdate).OnFixedUpdate();
        }
    }
    private void Update()
    {
        for (int i = 0; i < mainUpdates.Count; i++)
        {
            (mainUpdates[i] as IMainUpdate).OnUpdate();
        }
    }
    private void LateUpdate()
    {
        for (int i = 0; i < lateUpdates.Count; i++)
        {
            (lateUpdates[i] as ILateUpdate).OnLateUpdate();
        }
    }

    protected override void OnDestroy()
    {
        isupdateSystem = true;
        base.OnDestroy();
        Close();
    }
}
