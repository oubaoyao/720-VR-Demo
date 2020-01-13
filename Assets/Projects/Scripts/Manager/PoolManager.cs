
using MTFrame.MTPool;
/// <remarks>对象池管理类</remarks>
public class PoolManager
{
    //通用对象池
    private static GenericPropPool genericPropPool;
    //敌人对象池
    private static EnemyPool enemyPool;
    //玩家对象池
    private static PlayerPool playerPool;

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        playerPool = new PlayerPool();
        playerPool.Init();
        enemyPool = new EnemyPool();
        enemyPool.Init();
        genericPropPool = new GenericPropPool();
        genericPropPool.Init();
    }
    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="poolType"></param>
    /// <param name="poolObjectState"></param>
    /// <param name="Name"></param>
    /// <param name="sources"></param>
    public static void AddPool(PoolType poolType, PoolObjectStateType poolObjectState, string Name,ISources sources)
    {
        switch (poolType)
        {
            case PoolType.GenericProp:
                genericPropPool.AddPool(poolObjectState, Name, sources);
                break;
            case PoolType.Enemy:
                enemyPool.AddPool(poolObjectState, Name, sources);
                break;
            case PoolType.Player:
                playerPool.AddPool(poolObjectState, Name, sources);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="poolType"></param>
    /// <param name="poolObjectState"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    public static T GetPool<T>(PoolType poolType, PoolObjectStateType poolObjectState, string Name)
    {
        T t = default(T);
        switch (poolType)
        {
            case PoolType.GenericProp:
                t= genericPropPool.GetPool<T>(poolObjectState,Name);
                break;
            case PoolType.Enemy:
                t = enemyPool.GetPool<T>(poolObjectState, Name);
                break;
            case PoolType.Player:
                t = playerPool.GetPool<T>(poolObjectState, Name);
                break;
            default:
                break;
        }
        return t;
    }
    /// <summary>
    /// 清空
    /// </summary>
    /// <param name="poolType"></param>
    public static void Clear(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GenericProp:
               genericPropPool.Clear();
                break;
            case PoolType.Enemy:
                enemyPool.Clear();
                break;
            case PoolType.Player:
                playerPool.Clear();
                break;
            default:
                break;
        }
    }

}