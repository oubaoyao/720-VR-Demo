
namespace MTFrame.MTEvent
{
    /// <summary>
    /// 常用事件接口
    /// </summary>
    public interface IGenericEvent
    {
        //初始化
        void Init();
        //添加监听
        GenericEvent_CallBack AddListener(string eventName, GenericEvent_CallBack listener);
        //去除监听
        void RemoveListener(string eventName, GenericEvent_CallBack listener);
        //去除指定所有监听
        void RemoveAllListener(string eventName);
        //清除整个事件
        void Clear();
        //触发事件
        void TriggerEvent(string eventName, EventParamete parameteData);
    }
}