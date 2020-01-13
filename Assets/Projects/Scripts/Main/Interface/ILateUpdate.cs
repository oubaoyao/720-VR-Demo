
/// <summary>
/// 迟到的update更新接口
/// </summary>
public interface ILateUpdate : IUpdate
{
    /// <summary>
    /// 迟到的更新
    /// </summary>
    void OnLateUpdate();
}