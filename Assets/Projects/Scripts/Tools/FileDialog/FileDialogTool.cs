using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FileDialogTool
{
    private static FileDialogTool instance;
    public static FileDialogTool Instance
    {
        get
        {
            if (instance == null)
                instance = new FileDialogTool();
            return instance;
        }
    }

    System.Action<string> OpenQueueCall;

    /// <summary>
    /// 打开项目
    /// </summary>
    public void OpenProject(FileType fileType ,System.Action<string> action)
    {
        OpenQueueCall=action;
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        switch (fileType)
        {
            case FileType.All:
                pth.filter = "All files (*.*)\0*.*";
                break;
            case FileType.png:
                pth.filter = "All files (*.png)\0*.png";
                break;
            case FileType.jpg:
                pth.filter = "All files (*.jpg)\0*.jpg";
                break;
            case FileType.txt:
                pth.filter = "All files (*.txt)\0*.txt";
                break;
            case FileType.json:
                pth.filter = "All files (*.json)\0*.json";
                break;
            default:
                break;
        }
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
        pth.title = "打开项目";
        pth.defExt = "dat";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径;  
            Debug.Log(filepath);
            OpenQueueCall?.Invoke(filepath);
            OpenQueueCall=null;
        }
    }

    /// <summary>
    /// 保存文件项目
    /// </summary>
    public void SaveProject(FileType fileType)
    {
        SaveFileDlg pth = new SaveFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        switch (fileType)
        {
            case FileType.All:
                pth.filter = "All files (*.*)\0*.*";
                break;
            case FileType.png:
                pth.filter = "All files (*.png)\0*.png";
                break;
            case FileType.jpg:
                pth.filter = "All files (*.jpg)\0*.jpg";
                break;
            case FileType.txt:
                pth.filter = "All files (*.txt)\0*.txt";
                break;
            case FileType.json:
                pth.filter = "All files (*.json)\0*.json";
                break;
            default:
                break;
        }
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath; //默认路径
        pth.title = "保存项目";
        pth.defExt = "dat";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径;  
            Debug.Log(filepath);
        }
    }
}

public enum FileType
{
    All,
    png,
    jpg,
    txt,
    json
}

