using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehavior : MainBehavior
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();return _rectTransform; } }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}