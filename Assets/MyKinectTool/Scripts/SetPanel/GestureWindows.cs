using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class GestureWindows : MainBehavior, ISerializeButton
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
        public InputField gestureName, gestureOffset,  gestureTime;
        public ButtonBase intoEntry, leaveEntry, remove, Ok;
        public ButtonBase leftFoot, rightFoot, leftHand, rightHand;
        public Toggle leftFootToggle, rightFootToggle, leftHandToggle, rightHandToggle;
        public RawImage intoPhoto, leavePhoto;
        public Toggle toggle;
        public Dropdown dropdown;

        private Texture2D texture;
        private SetGesturePanel setGesturePanel;
        // Use this for initialization
       protected override void Start()
        {
            Init();
            FindObj();
            InitEvent();
        }

        
        private void Init()
        {
            texture = new Texture2D(100, 100);
            Color[] colors = new Color[100 * 100];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.black;
            }
            texture.SetPixels(colors);
            texture.Apply();
        }
        private void FindObj()
        {
            gestureName = FindTool.FindChildComponent<InputField>(transform, "Name/InputField");
            gestureOffset = FindTool.FindChildComponent<InputField>(transform, "Offset/InputField");
            intoEntry = FindTool.FindChildComponent<ButtonBase>(transform, "intoEntry");
            leaveEntry =  FindTool.FindChildComponent<ButtonBase>(transform, "leaveEntry");
            remove =  FindTool.FindChildComponent<ButtonBase>(transform, "Remove");
            Ok =  FindTool.FindChildComponent<ButtonBase>(transform, "Ok");

            leftFoot =  FindTool.FindChildComponent<ButtonBase>(transform, "Body/LeftFoot");
            rightFoot =  FindTool.FindChildComponent<ButtonBase>(transform, "Body/RightFoot");
            leftHand =  FindTool.FindChildComponent<ButtonBase>(transform, "Body/LeftHand");
            rightHand =  FindTool.FindChildComponent<ButtonBase>(transform, "Body/RightHand");

            leftFootToggle = FindTool.FindChildComponent<Toggle>(transform, "Body/LeftFoot/Toggle");
            rightFootToggle = FindTool.FindChildComponent<Toggle>(transform, "Body/RightFoot/Toggle");
            leftHandToggle = FindTool.FindChildComponent<Toggle>(transform, "Body/LeftHand/Toggle");
            rightHandToggle = FindTool.FindChildComponent<Toggle>(transform, "Body/RightHand/Toggle");

            intoPhoto =  FindTool.FindChildComponent<RawImage>(transform, "intoBack/intoPhoto");
            leavePhoto =  FindTool.FindChildComponent<RawImage>(transform, "leaveBack/leavePhoto");
            toggle =  FindTool.FindChildComponent<Toggle>(transform, "Toggle");
            dropdown =  FindTool.FindChildComponent<Dropdown>(transform, "Dropdown");
        }
        private void InitEvent()
        {
            //录入信息按钮事件添加
            intoEntry.OnClick.AddListener(() =>
            {
                PlayerGestureManager.Instance.SetGestureJointData( PlayerGestureInfo.GestureType.End, gesture.playerGestureData);
                
                if (gesture.playerGestureData.playerGestureInfo.intoPhotoBase64  != null)
                    intoPhoto.texture = gesture.playerGestureData.playerGestureInfo.intoPhotoBase64.Get();
                else
                    intoPhoto.texture = texture;

            });
            //录入信息按钮事件添加
            leaveEntry.OnClick.AddListener(() =>
            {
                PlayerGestureManager.Instance.SetGestureJointData(PlayerGestureInfo.GestureType.Start, gesture.playerGestureData);
                
                if (gesture.playerGestureData.playerGestureInfo.leavePhotoBase64 != null)
                    leavePhoto.texture = gesture.playerGestureData.playerGestureInfo.leavePhotoBase64.Get();
                else
                    leavePhoto.texture = texture;

            });
            //删除信息按钮事件添加
            remove.OnClick.AddListener(() =>
            {
                transform.localScale = Vector3.zero;
                setGesturePanel. RemoveGesture(gesture);
            });
            //确定信息按钮事件添加
            Ok.OnClick.AddListener(() =>
            {
                transform.localScale = Vector3.zero;
                gesture.playerGestureData.playerGestureInfo.isOnLeftFoot = leftFoot.isChoice;
                gesture.playerGestureData.playerGestureInfo.isOnRightFoot = rightFoot.isChoice;
                gesture.playerGestureData.playerGestureInfo.isOnLeftHand = leftHand.isChoice;
                gesture.playerGestureData.playerGestureInfo.isOnRightHand = rightHand.isChoice;

                gesture.playerGestureData.playerGestureInfo.isLeftFootMain = leftFootToggle.isOn;
                gesture.playerGestureData.playerGestureInfo.isRightFootMain = rightFootToggle.isOn;
                gesture.playerGestureData.playerGestureInfo.isLeftHandMain = leftHandToggle.isOn;
                gesture.playerGestureData.playerGestureInfo.isRightHandMain = rightHandToggle.isOn;

                gesture.playerGestureData.playerGestureInfo.GestureName = gestureName.text;
                gesture.playerGestureData.playerGestureInfo.isFindGesture = toggle.isOn;
                gesture.playerGestureData.playerGestureInfo.gestureActionType= (GestureActionType)(Enum.GetValues(typeof(GestureActionType))).GetValue(dropdown.value) ;
                if (gesture.playerGestureData.playerGestureInfo.isFindGesture)
                   PlayerGestureManager.Instance.ReturnFindGesture(gesture.playerGestureData);
                try
                {
                    gesture.playerGestureData.playerGestureInfo.timeGesture = float.Parse(gestureTime.text);
                    gesture.playerGestureData.playerGestureInfo.offset = int.Parse(gestureOffset.text);
                }
                catch (System.Exception e)
                {
                    print(e.Data);
                }
                gesture.gestureButton.Init();
                gesture.Init(gesture.playerGestureData, setGesturePanel);
            });
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        public void Recovery()
        {
            transform.localScale = Vector3.zero;
        }
        /// <summary>
        /// 设置面板信息
        /// </summary>
        /// <param name="_gesture"></param>
        /// <param name="_setGesturePanel"></param>
        public void SetPanel(Gesture _gesture, SetGesturePanel _setGesturePanel)
        {
            transform.localScale = Vector3.one;
            setGesturePanel = _setGesturePanel;
            gesture = _gesture;

            gestureName.text = gesture. playerGestureData.playerGestureInfo.GestureName;
            gestureOffset.text = gesture.playerGestureData.playerGestureInfo.offset.ToString();
            gestureTime.text = gesture.playerGestureData.playerGestureInfo.timeGesture.ToString();
            toggle.isOn = gesture.playerGestureData.playerGestureInfo.isFindGesture;
            dropdown.options.Clear();
            foreach (GestureActionType gat in Enum.GetValues(typeof( GestureActionType)))
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                optionData.text = gat.ToString();
                dropdown.options.Add(optionData);
            }
            dropdown.captionText.text = gesture.playerGestureData.playerGestureInfo.gestureActionType.ToString();
            dropdown.value = (int)gesture.playerGestureData.playerGestureInfo.gestureActionType;

            
            if (gesture.playerGestureData.playerGestureInfo.intoPhotoBase64!=null)
                intoPhoto.texture = gesture.playerGestureData.playerGestureInfo.intoPhotoBase64.Get();
            else
                intoPhoto.texture = texture;

            if (gesture.playerGestureData.playerGestureInfo.leavePhotoBase64 != null)
                leavePhoto.texture = gesture.playerGestureData.playerGestureInfo.leavePhotoBase64.Get();
            else
                leavePhoto.texture = texture;

            if (gesture.playerGestureData.playerGestureInfo.isOnLeftFoot)
                leftFoot.Init(true);
            else
                leftFoot.Init(false);

            if (gesture.playerGestureData.playerGestureInfo.isOnRightFoot)
                rightFoot.Init(true);
            else
                rightFoot.Init(false);

            if (gesture.playerGestureData.playerGestureInfo.isOnLeftHand)
                leftHand.Init(true);
            else
                leftHand.Init(false);

            if (gesture.playerGestureData.playerGestureInfo.isOnRightHand)
                rightHand.Init(true);
            else
                rightHand.Init(false);




            if (gesture.playerGestureData.playerGestureInfo.isLeftFootMain)
                leftFootToggle.isOn=true ;
            else
                leftFootToggle.isOn = false ;

            if (gesture.playerGestureData.playerGestureInfo.isRightFootMain)
                rightFootToggle.isOn = true;
            else
                rightFootToggle.isOn=false;

            if (gesture.playerGestureData.playerGestureInfo.isLeftHandMain)
                leftHandToggle.isOn = true;
            else
                leftHandToggle.isOn = false;

            if (gesture.playerGestureData.playerGestureInfo.isRightHandMain)
                rightHandToggle.isOn = true;
            else
                rightHandToggle.isOn = false;


            gesture.playerGestureData.playerGestureInfo.isLeftFootMain = leftFootToggle.isOn;
            gesture.playerGestureData.playerGestureInfo.isRightFootMain = rightFootToggle.isOn;
            gesture.playerGestureData.playerGestureInfo.isLeftHandMain = leftHandToggle.isOn;
            gesture.playerGestureData.playerGestureInfo.isRightHandMain = rightHandToggle.isOn;
        }
    }
}
