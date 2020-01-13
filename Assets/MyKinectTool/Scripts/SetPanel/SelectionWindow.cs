
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTKinect
{
    public class SelectionWindow : MainBehavior, ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>()
                {
                    "FindObj",
                };
            }
        }

        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>()
                {
                    FindObj,
                };
            }
        }


        public Gesture gesture;
        public ButtonBase GestureInput, EventInput, Remove;
        private SetGesturePanel setGesturePanel;
       protected override void Start()
        {
            Init();
            FindObj();
            InitEvent();
        }

        private void Init()
        {
        }

        private void FindObj()
        {
            setGesturePanel = GetComponentInParent<SetGesturePanel>();
            GestureInput =  FindTool.FindChildComponent<ButtonBase>(transform, "GestureInput");
            EventInput =  FindTool.FindChildComponent<ButtonBase>(transform, "EventInput");
            Remove =  FindTool.FindChildComponent<ButtonBase>(transform, "Remove");
        }

        private void InitEvent()
        {
            GestureInput.OnClick.AddListener(() =>
            {
                Recovery();
                setGesturePanel.gestureWindows.SetPanel(gesture, setGesturePanel);
            });
            EventInput.OnClick.AddListener(() =>
            {
                Recovery();
                setGesturePanel.eventWindows.SetPanel(gesture);
            });
            Remove.OnClick.AddListener(() =>
            {
                Recovery();
                setGesturePanel.RemoveGesture(gesture);
            });
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recovery()
        {
            transform.localScale = Vector3.zero;
        }
        /// <summary>
        /// 设置面板
        /// </summary>
        /// <param name="_gesture"></param>
        public void SetPanel(Gesture _gesture)
        {
            transform.localScale = Vector3.one;
            Vector2 pos = transform.position;
            pos.y = _gesture.transform.position.y;
            transform.position = pos;
            RectTransform rect_1 = (transform as RectTransform);
            RectTransform rect_2 = (setGesturePanel.transform as RectTransform);
             //Debug.Log(rect_1.localPosition.y + "=======" + rect_2.sizeDelta.y);
            if (rect_1.localPosition.y < -((rect_2.sizeDelta.y / 2) - (rect_1.sizeDelta.y)))
            {
                transform.localPosition += new Vector3(0, rect_1.sizeDelta.y-20, 0);
            }

            gesture = _gesture;
        }

    }
}
