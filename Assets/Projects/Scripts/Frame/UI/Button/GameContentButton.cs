using MTFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameContentButton : MenuButton
{
    public RawImage GameImage;
    protected override void Start()
    {
        base.Start();
        GameImage = FindTool.FindChildComponent<RawImage>(transform, "Mask/GameImage");
    }

    public void SetImage(Texture2D texture )
    {
        GameImage.texture = texture;
    }

    public override void TriggerEnter()
    {
        base.TriggerEnter();
    }

    public override void TriggerExit()
    {
        base.TriggerExit();
    }

}