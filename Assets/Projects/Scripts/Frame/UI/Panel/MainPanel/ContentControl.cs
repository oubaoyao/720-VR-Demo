using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ContentControl : MonoBehaviour
{

    public RawImage ImageContent;
    public VideoPlayer VidoeContent;

    public bool isOpen;

    public Texture2D texture2D;

    // Use this for initialization
    void Start()
    {
        ImageContent = FindTool.FindChildComponent<RawImage>(transform, "ImageContent");
        VidoeContent = FindTool.FindChildComponent<VideoPlayer>(transform, "VidoeContent");

    }

    public void SetContent(FileInfo fileInfo, Texture2D texture = null)
    {
        switch (fileInfo.Extension.ToUpper())
        {
            case ".PNG":
                ImageContent.gameObject.SetActive(true);
                VidoeContent.gameObject.SetActive(false);
                if (texture != null)
                {
                    ImageContent.texture = texture;
                }
                else
                {
                    FileManager.ReadWeb(fileInfo.FullName, (fileObject) =>
                    {
                        texture2D = new Texture2D(0, 0);
                        texture2D.LoadImage(fileObject.Buffet);
                        texture2D.Apply();
                        ImageContent.texture = texture2D;

                    }, MTFrame.MTFile.FileFormatType.png, MTFrame.MTFile.EncryptModeType.None);
                }
                break;
            case ".JPG":
                ImageContent.gameObject.SetActive(true);
                VidoeContent.gameObject.SetActive(false); if (texture != null)
                {
                    ImageContent.texture = texture;
                }
                else
                {
                    FileManager.ReadWeb(fileInfo.FullName, (fileObject) =>
                    {
                        texture2D = new Texture2D(0, 0);
                        texture2D.LoadImage(fileObject.Buffet);
                        texture2D.Apply();
                        ImageContent.texture = texture2D;

                    }, MTFrame.MTFile.FileFormatType.jpg, MTFrame.MTFile.EncryptModeType.None);
                }
                break;
            case ".MOV":
            case ".MP4":
            case ".AVI":
                ImageContent.gameObject.SetActive(false);
                VidoeContent.gameObject.SetActive(true);
                VidoeContent.url = fileInfo.FullName;
                VidoeContent.Prepare();
                TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 0.4f, () => { gameObject.transform.GetChild(1).GetComponent<RawImage>().enabled = true; });
                if (isOpen)
                    VidoeContent.Play();
                else
                    VidoeContent.Stop();
                break;
        }

        GC.Collect();
    }


    public void Close()
    {
        if(VidoeContent)
           VidoeContent?.Stop();
    }

}
