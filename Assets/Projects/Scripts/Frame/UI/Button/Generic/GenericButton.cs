using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace MTFrame
{
    /// <summary>
    /// 通用按钮
    /// </summary>
    public class GenericButton : BaseButton, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,IPointerUpHandler,IPointerDownHandler
    {
        /// <summary>
        /// 点击触发
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            TriggerClick();
        }

        /// <summary>
        /// 按下触发
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            TriggerDown();
        }

        /// <summary>
        /// 进入触发
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            TriggerEnter();
        }
        /// <summary>
        /// 离开触发
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            TriggerExit();
        }
        /// <summary>
        /// 弹起触发
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            TriggerUp();
        }

        /// <summary>
        /// 触发点击
        /// </summary>
        public override void TriggerClick()
        {
            base.TriggerClick();
        }
        /// <summary>
        /// 触发进入
        /// </summary>
        public override void TriggerEnter()
        {
            base.TriggerEnter();
        }
        /// <summary>
        /// 触发离开
        /// </summary>
        public override void TriggerExit()
        {
            base.TriggerExit();
        }
        /// <summary>
        /// 触发弹起
        /// </summary>
        public override void TriggerUp()
        {
            base.TriggerUp();
        }
        /// <summary>
        /// 触发按下
        /// </summary>
        public override void TriggerDown()
        {
            base.TriggerDown();
        }
    }
}