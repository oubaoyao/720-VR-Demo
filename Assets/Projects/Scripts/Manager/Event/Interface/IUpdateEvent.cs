
namespace MTFrame. MTEvent
{
    /// <summary>
    /// 更新事件接口
    /// </summary>
    public interface IUpdateEvent
    {

        UpdateEvent_CallBack AddListener(string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum);
        //去除监听
        void RemoveListener(string eventName, UpdateEvent_CallBack listener, OperationModeEnumType operationModeEnum);
        //去除对应所有监听
        void RemoveAllListener(string eventName, OperationModeEnumType operationModeEnum);
        //清空所有事件
        void Clear(OperationModeEnumType operationModeEnum);

    }
}