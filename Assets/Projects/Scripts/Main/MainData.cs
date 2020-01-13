using MTFrame.MTFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 主要数据
/// </summary>
public class MainData
{

    private static MainData instance;
    public static MainData Instance { get { if (instance == null) instance = new MainData(); return instance; } }


    /// <summary>
    /// BGM音量
    /// </summary>
    public static float BGMAudioVolume;
    /// <summary>
    /// 对话音量
    /// </summary>
    public static float SpeechAudioVolume;
    /// <summary>
    /// 效果音量
    /// </summary>
    public static float EffsetAudioVolume;

    /// <summary>
    /// 外置文件路径管理
    /// </summary>
    public List<DirectoryPathData> directoryPathDatas=new List<DirectoryPathData>();

    /// <summary>
    /// 外置游戏文件路径管理
    /// </summary>
    public List<GameFilePathData> Game_filePathData = new List<GameFilePathData>();

    /// <summary>
    /// 外置广告文件路径管理
    /// </summary>
    public FilePathData AP_filePathData;

    /// <summary>
    ///待机广告时间 
    /// </summary>
    public float aP_BideTime = 10;
    /// <summary>
    ///广告间隔时间 
    /// </summary>
    public float aP_IntervalTime = 5;

    /// <summary>
    /// 是否主要窗口
    /// </summary>
    public  bool isMainWindow=true;
}

public class DirectoryPathData
{
    public DirectoryPathData(DirectoryInfo directoryInfos)
    {
        this.directoryInfo = directoryInfos;
        foreach (DirectoryInfo directory in directoryInfos.GetDirectories())
        {
            filePathData.Add(new FilePathData(directory));
        }
    }
    public DirectoryInfo directoryInfo;
    public List<FilePathData> filePathData = new List<FilePathData>();

}

public class FilePathData
{
    public FilePathData(DirectoryInfo directoryInfos)
    {
        this.directoryInfo = directoryInfos;
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
            if (fileInfo.Extension.ToUpper() == ".PNG" || fileInfo.Extension.ToUpper() == ".JPG" || fileInfo.Extension.ToUpper() == ".MP4" || fileInfo.Extension.ToUpper() == ".MOV" || fileInfo.Extension.ToUpper() == ".AVI")
            {
                fileInfos.Add(fileInfo);
            }
        }
    }
    public DirectoryInfo directoryInfo;
    public List<FileInfo> fileInfos=new List<FileInfo>();

}
public class GameFilePathData
{
    public GameFilePathData(DirectoryInfo directoryInfos)
    {
        this.directoryInfo = directoryInfos;
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
            if (fileInfo.Extension.ToUpper() == ".PNG" || fileInfo.Extension.ToUpper() == ".JPG" || fileInfo.Extension.ToUpper() == ".LNK")
            {
                fileInfos.Add(fileInfo);
            }
        }

    }
    public DirectoryInfo directoryInfo;
    public List<FileInfo> fileInfos = new List<FileInfo>();

}