
namespace MTFrame. MTEvent
{
    /// <summary>
    /// 信息监听接口
    /// </summary>
    public interface IListenerMessage
    {
        /// <summary>
        /// 监听信息ID
        /// </summary>
        string[] ListenerMessageID { get; set; }
        
        /// <summary>
        /// 回调信息
        /// </summary>
        /// <param name="parameteData"></param>
        void OnListenerMessage(EventParamete parameteData);
    }
}