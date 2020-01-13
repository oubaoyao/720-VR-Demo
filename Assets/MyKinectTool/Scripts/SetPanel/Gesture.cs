using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class Gesture : MainBehavior
    {
        public Text GestureName;
        public ButtonBase gestureButton;
        public PlayerGestureData playerGestureData;
        private SetGesturePanel setGesturePanel;
        
        /// <summary>
        /// 初始化按钮
        /// </summary>
        /// <param name="_playerGestureData"></param>
        /// <param name="_setGesturePanel"></param>
        public void Init(PlayerGestureData _playerGestureData, SetGesturePanel _setGesturePanel)
        {
            playerGestureData = _playerGestureData;
            GestureName.text = _playerGestureData.playerGestureInfo.GestureName;
            if(_setGesturePanel)
               setGesturePanel = _setGesturePanel;
            gestureButton.OnClick.AddListener(() => 
            {
                setGesturePanel.ReturnGestureButton(this);
            });
            if (_playerGestureData.playerGestureInfo.isFindGesture)
              PlayerGestureManager.Instance.ReturnFindGesture(_playerGestureData);
        }

        /// <summary>
        /// 恢复按钮数据
        /// </summary>
        public void Recovery()
        {
            gameObject.SetActive(false);
            gestureButton.InitEvent();
        }
    }
}