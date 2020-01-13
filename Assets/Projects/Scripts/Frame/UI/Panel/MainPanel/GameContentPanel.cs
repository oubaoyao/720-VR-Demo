using MTFrame;
using MTFrame.MTKinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameContentPanel : BasePanel
{
    public Transform ContentGroup;
    public  GameContentButton contentButton ;

    public List<GameContentButton> gameContentButtons = new List<GameContentButton>();

    public override void InitFind()
    {
        base.InitFind();
        gameContentButtons.Clear();
         ContentGroup = FindTool.FindChildComponent<Transform>(transform, "BG_Group/BG/Mask/ContentGroup");

        foreach (var item in MainData.Instance.Game_filePathData)
        {
            GameContentButton gb = GameObject.Instantiate(contentButton, ContentGroup);
            gb.Init(item.directoryInfo.Name);
            FileInfo fileInfo = item.fileInfos.Find(p => p.Extension.ToUpper() == ".PNG");
            if (fileInfo != null)
            {
                FileManager.ReadWeb(fileInfo.FullName, (fileObject) => {
                    Texture2D texture2D = new Texture2D(0, 0);
                    texture2D.LoadImage(fileObject.Buffet);
                    texture2D.Apply();
                    gb.SetImage(texture2D);
                }, MTFrame.MTFile.FileFormatType.png);
            }
            gameContentButtons.Add(gb);
            gb.OnClick += OnClick;

        }
    }

    private void OnClick(BaseButton obj)
    {
        GameFilePathData gameFilePathData = MainData.Instance.Game_filePathData.Find(p => p.directoryInfo.Name == (obj as GameContentButton).GetName());
        FileInfo fileInfo = gameFilePathData.fileInfos.Find(p => p.Extension.ToUpper() == ".LNK");

        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(fileInfo.FullName);

        Application.OpenURL(shortcut.FullName);
        Debug.Log(shortcut.FullName);
        int lenth = 0;
        while(!shortcut.FullName.Substring(shortcut.FullName.Length-15-lenth,1).Contains(@"\"))
        {
            lenth++;
        }
        // 除去自己"\"
        lenth--;
        string ProcessName = shortcut.FullName.Substring(shortcut.FullName.Length - 15 - lenth, lenth);
        Debug.Log("ProcessName:" + ProcessName);
        MainData.Instance.isMainWindow=false ;
        KinectManager.Instance.ClearKinectUsers();
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 2.0f, () => {
            MainPanel.Instance.audioSource.Pause();
            SoftwareSettingsTool.Instance.productName = ProcessName;
        });

        lastUIPanel.Hide();
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }


    public override void Open()
    {
        base.Open();

        foreach (var item in gameContentButtons)
        {
            item?.Open();
        }
    }

    public override void Hide()
    {
        base.Hide();
        foreach (var item in gameContentButtons)
        {
            item?.Close();
        }
    }
}