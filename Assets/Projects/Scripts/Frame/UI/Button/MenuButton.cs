using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTFrame
{
    public class MenuButton : BaseButton, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isOpen;

        public float speedTime = 3;

        private Image cursor;
        private Vector3 cursorStartPos;
        private Text text;
        private float processValue;
        private Collider _collider;


        protected override void Start()
        {
            base.Start();

            cursor = FindTool.FindChildComponent<Image>(transform, "Mask/Value");
            cursorStartPos = cursor.transform.localPosition;
            text = FindTool.FindChildComponent<Text>(transform, "Text");
            _collider = GetComponent<Collider>();
        }

        public void Init(string _name)
        {
            text = FindTool.FindChildComponent<Text>(transform, "Text");
            text.text = _name;
        }

        public void Open()
        {
            isOpen = true;
            if (_collider)
                _collider.enabled = true;
        }

        public void Close()
        {
            isOpen = false;
            if (_collider)
                _collider.enabled = false;
        }


        /// <summary>
        /// 触发进入
        /// </summary>
        public override void TriggerEnter()
        {
            if (!isOpen)
                return;
            base.TriggerEnter();
        }
        /// <summary>
        /// 触发离开
        /// </summary>
        public override void TriggerExit()
        {
            if (!isOpen)
                return;
            base.TriggerExit();

            cursor.transform.localPosition = cursorStartPos;
        }

        public string GetName()
        {
            return text.text;
        }


        public override void TriggerClick()
        {
            if (!isOpen)
                return;
            base.TriggerClick();
            cursor.transform.localPosition = cursorStartPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TriggerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TriggerExit();
        }

        public void OnUpdate()
        {
            cursor.transform.localPosition += Vector3.right * Time.deltaTime * speedTime;
            if (cursor.transform.localPosition.x >= rectTransform.sizeDelta.x / 2)
                TriggerClick();
        }
    }
}