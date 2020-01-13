namespace MTFrame
{
    /// <summary>
    ///面板过度动画接口
    /// </summary>
    public interface IPanelTween
    {
        //打开
        void Open(float tweenTime);
        //隐藏
        void Hide(float tweenTime);
    }
}