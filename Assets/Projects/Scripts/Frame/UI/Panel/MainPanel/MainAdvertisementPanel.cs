using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainAdvertisementPanel : BasePanel
{
    public Transform ContentGroup;

    public ContentControl[] contentControls;

    private int index = 0;
    private List<FileInfo> fileInfos = new List<FileInfo>();
    private List<Texture2D> texture2Ds = new List<Texture2D>();
    public List<VideoPlayer> videoPlayers = new List<VideoPlayer>();
    public List<RawImage> ContentRawImage = new List<RawImage>();

    public override void InitFind()
    {
        base.InitFind();

        contentControls = GetComponentsInChildren<ContentControl>();

        ContentGroup = FindTool.FindChildComponent<Transform>(transform, "BG/ContentGroup");

        fileInfos = MainData.Instance.AP_filePathData.fileInfos;

        for (int i = 0; i < contentControls.Length; i++)
        {
            videoPlayers.Add(contentControls[i].gameObject.GetComponentInChildren<VideoPlayer>());
        }

        for (int i = 0; i < contentControls.Length; i++)
        {
            RawImage rawImage = contentControls[i].gameObject.transform.GetChild(1).GetComponent<RawImage>();
            ContentRawImage.Add(rawImage);
        }

    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        MainPanel.Instance.audioSource.Pause();
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].enabled = true;
        }
        for (int i = 0; i < ContentRawImage.Count; i++)
        {
            ContentRawImage[i].enabled = true;
        }
        SetContent(0);
        Debug.Log("开始播放广告");

        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact,MainData.Instance.aP_IntervalTime, OnContent, true);
    }

    public override void Hide()
    {
        base.Hide();
        index = 0;
        Debug.Log("停止播放广告");
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].Stop();
            videoPlayers[i].enabled = false;
        }
        MainPanel.Instance.audioSource.Play();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact,OnContent);
    }

    private void OnContent()
    {
        ContentGroup.DOLocalMoveX(-1920, 1).OnComplete(() =>
        {
            for (int i = 0; i < ContentRawImage.Count; i++)
            {
                ContentRawImage[i].enabled = false;
            }
            ContentGroup.localPosition = Vector3.zero;
            SetContent(1);
        });
    }


    private void SetContent(int v)
    {
        index += v;
        if (index < 0)
            index = fileInfos.Count - 1;
        else if (index >= fileInfos.Count)
            index = 0;

        texture2Ds.Clear();
        foreach (var item in contentControls)
        {
            texture2Ds.Add(item.texture2D);
        }

        if (fileInfos.Count == 1)
        {
            for (int i = 0; i < contentControls.Length; i++)
            {
                contentControls[i].SetContent(fileInfos[index]);
                Debug.Log(fileInfos[index]);
            }
            return;
        }

        for (int i = 0; i < contentControls.Length; i++)
        {
            int _index = index + i;
            if (_index >= fileInfos.Count)
                _index = 0 + (_index - fileInfos.Count);
            if (i == 1)
            {
                contentControls[i].SetContent(fileInfos[_index], texture2Ds[i + v]);
            }
            else
            {
                contentControls[i].SetContent(fileInfos[_index]);
            }
            Debug.Log(fileInfos[_index]);
        }
    }

}