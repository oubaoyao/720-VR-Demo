using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame
{
    /// <summary>
    /// 渐变渐变动画
    /// </summary>
    public class GradualTween : UIBasePanelTween
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public GradualTween(BasePanel panel) : base(panel)
        {
        }


        /// <summary>
        ///  把Panel重置到该动画的初始状态
        /// </summary>
        public override void Init()
        {
        }
        /// <summary>
        /// 把该动画修改过的Panel的状态重置回来
        /// </summary>
        public override void Exit()
        {
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="tweenTime"></param>
        public override void Open(float tweenTime)
        {
            base.Open(tweenTime);
            panel.CanvasGroup.DOFillAlpha(1, tweenTime, TweenMode.NoUnityTimeLineImpact);
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="tweenTime"></param>
        public override void Hide(float tweenTime)
        {
            base.Hide(tweenTime);
            panel.CanvasGroup.DOFillAlpha(0, tweenTime, TweenMode.NoUnityTimeLineImpact);
        }
    }
}