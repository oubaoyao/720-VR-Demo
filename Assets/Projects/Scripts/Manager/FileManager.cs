using MTFrame.MTFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 存储管理
/// </summary>
public class FileManager
{

    private static TXTFileSave txtFileSave=new TXTFileSave();
    private static JsonFileSave jsonFileSave = new JsonFileSave();
    private static XMLFileSave xmlFileSave = new XMLFileSave();
    private static PNGFileSave pngFileSave = new PNGFileSave();
    private static JPGFileSave jpgFileSave = new JPGFileSave();
    private static EXRFileSave exrFileSave = new EXRFileSave();

    /// <summary>
    /// 读取
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="path">地址</param>
    /// <param name="fileFormatType">文件类型</param>
    /// <param name="encryptModeType">加密类型</param>
    /// <returns></returns>
    public static FileObject Read(string path, FileFormatType fileFormatType= FileFormatType.txt, EncryptModeType encryptModeType= EncryptModeType.None )
    {
        FileObject fileObject = default(FileObject);
        switch (fileFormatType)
        {
            case FileFormatType.txt:
                fileObject = txtFileSave.ReadData(path, encryptModeType);
                break;
            case FileFormatType.json:
                fileObject = jsonFileSave.ReadData(path, encryptModeType);
                break;
            case FileFormatType.xml:
                fileObject = xmlFileSave.ReadData(path, encryptModeType);
                break;

            case FileFormatType.png:
                fileObject = pngFileSave.ReadData(path, encryptModeType);
                break;
            case FileFormatType.jpg:
                fileObject = jpgFileSave.ReadData(path, encryptModeType);
                break;
            case FileFormatType.exr:
                fileObject = exrFileSave.ReadData(path, encryptModeType);
                break;
            default:
                break;
        }
        return fileObject;
    }


    /// <summary>
    /// 异步读取
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="path">地址</param>
    /// <param name="fileFormatType">文件类型</param>
    /// <param name="encryptModeType">加密类型</param>
    /// <returns></returns>
    public static void ReadAsync(string path, System.Action<FileObject> OnCall, FileFormatType fileFormatType = FileFormatType.txt, EncryptModeType encryptModeType = EncryptModeType.None)
    {
        switch (fileFormatType)
        {
            case FileFormatType.txt:
                txtFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.json:
                jsonFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.xml:
                xmlFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;

            case FileFormatType.png:
                pngFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.jpg:
                jpgFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.exr:
                exrFileSave.ReadAsyncData(path, encryptModeType, OnCall);
                break;
            default:
                break;
        }
    }




    /// <summary>
    /// Web读取
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="path">地址</param>
    /// <param name="fileFormatType">文件类型</param>
    /// <param name="encryptModeType">加密类型</param>
    /// <returns></returns>
    public static void ReadWeb(string path, System.Action<FileObject> OnCall, FileFormatType fileFormatType = FileFormatType.txt, EncryptModeType encryptModeType = EncryptModeType.None)
    {
        switch (fileFormatType)
        {
            case FileFormatType.txt:
                txtFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.json:
                jsonFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.xml:
                xmlFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;

            case FileFormatType.png:
                pngFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.jpg:
                jpgFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;
            case FileFormatType.exr:
                exrFileSave.ReadWebData(path, encryptModeType, OnCall);
                break;
            default:
                break;
        }
    }



    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="path">地址</param>
    /// <param name="data">数据</param>
    /// <param name="fileFormatType">文件类型</param>
    /// <param name="encryptModeType">加密类型</param>
    public static void Write(string path,byte[] data , FileFormatType fileFormatType = FileFormatType.txt, EncryptModeType encryptModeType = EncryptModeType.None)
    {
        switch (fileFormatType)
        {
            case FileFormatType.txt:
                txtFileSave.WriteData(path, data, encryptModeType);
                break;
            case FileFormatType.json:
                jsonFileSave.WriteData(path, data, encryptModeType);
                break;
            case FileFormatType.xml:
                xmlFileSave.WriteData(path, data, encryptModeType);
                break;

            case FileFormatType.png:
                pngFileSave.WriteData(path, data, encryptModeType);
                break;
            case FileFormatType.jpg:
                jpgFileSave.WriteData(path, data, encryptModeType);
                break;
            case FileFormatType.exr:
                exrFileSave.WriteData(path, data, encryptModeType);
                break;
            default:
                break;
        }
    }





    /// <summary>
    /// 一级byte加密写入
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static string one_EncryptPath(string path, string exten)
    {
        byte[] buffer_0 = default(byte[]);
        string name_0 = "";

        string[] vs = path.Split('/');

        buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

        name_0 = BitConverter.ToString(buffer_0, 0);

        path = path.Substring(0, path.LastIndexOf('/'));

        path += "/" + name_0 + exten;
        return path;
    }
    /// <summary>
    /// 二级byte加密写入
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static string two_EncryptPath(string path, string exten)
    {
        byte[] buffer_0 = default(byte[]);
        byte[] buffer_1 = default(byte[]);
        string name_0 = "";
        string name_1 = "";

        string[] vs = path.Split('/');

        buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 2]);
        buffer_1 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

        name_0 = BitConverter.ToString(buffer_0, 0);
        name_1 = BitConverter.ToString(buffer_1, 0);

        path = path.Substring(0, path.LastIndexOf('/'));
        path = path.Substring(0, path.LastIndexOf('/'));

        path += "/" + name_0;
        path += "/" + name_1 + exten;
        return path;
    }
    /// <summary>
    /// 三级byte加密写入
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static string three_EncryptPath(string path, string exten)
    {
        byte[] buffer_0 = default(byte[]);
        byte[] buffer_1 = default(byte[]);
        byte[] buffer_2 = default(byte[]);
        string name_0 = "";
        string name_1 = "";
        string name_2 = "";

        string[] vs = path.Split('/');

        buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 3]);
        buffer_1 = Encoding.UTF8.GetBytes(vs[vs.Length - 2]);
        buffer_2 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

        name_0 = BitConverter.ToString(buffer_0, 0);
        name_1 = BitConverter.ToString(buffer_1, 0);
        name_2 = BitConverter.ToString(buffer_2, 0);

        path = path.Substring(0, path.LastIndexOf('/'));
        path = path.Substring(0, path.LastIndexOf('/'));
        path = path.Substring(0, path.LastIndexOf('/'));

        path += "/" + name_0;
        path += "/" + name_1;
        path += "/" + name_2 + exten;
        return path;
    }
}
