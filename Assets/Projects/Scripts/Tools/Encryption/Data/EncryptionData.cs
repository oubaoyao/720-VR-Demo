using System;
using System.Collections;
using System.Collections.Generic;
using MTFrame. Tool.ViKey;
using UnityEngine;

/// <summary>
/// 加密数据
/// </summary>
[System.Serializable]
public class EncryptionData
{
    #region 屏幕分辨率
    public int W=1920;
    public int H=1080;
    public float Size=5;
    #endregion


    #region 网络加密
    /// <summary>
    /// 网络加密
    /// </summary>
    public MTKeyEncryptionData mTKeyEncryptionData = new MTKeyEncryptionData();
    /// <summary>
    /// 网络加密水印
    /// </summary>
    public WatermarkData MTKeyWatermark = new WatermarkData();
    #endregion


    #region 加密狗
    /// <summary>
    /// 加密狗
    /// </summary>
    public VikeyEncryptionData vikeyEncryptionData = new VikeyEncryptionData();
    /// <summary>
    /// 加密狗水印
    /// </summary>
    public WatermarkData ViKeyWatermark = new WatermarkData();
    #endregion


    #region 时间限制
    /// <summary>
    /// 时间限制
    /// </summary>
    public TimeEncryptionData timeEncryptionData = new TimeEncryptionData();
    /// <summary>
    /// 时间限制水印
    /// </summary>
    public WatermarkData TimeWatermark = new WatermarkData();
    #endregion


    #region Logo图片
    public bool isLogo=true;
    /// <summary>
    /// Logo水印
    /// </summary>
    public WatermarkData LogoWatermark = new WatermarkData();
    #endregion
}

/// <summary>
/// 水印数据
/// </summary>
[System.Serializable]
public class WatermarkData
{
    public _Texture2D _TextureData=new _Texture2D(null);
    public _Vector4 _Rect=new _Vector4(new Vector4(0,0,400,200));

    public _Vector2 anchorMin=new _Vector2(Vector2.zero);
    public _Vector2 anchorMax = new _Vector2(Vector2.zero);
}

public class VikeyEncryptionData
{
    public bool isViKey = true;
    public VikeyEncryptionType vikeyEncryptionType = VikeyEncryptionType.Null;
    public string ViKeyNumber = "mt1111111111";
    public string ViKeyProjectName = "mtkj";
}
public class MTKeyEncryptionData
{
    public bool isMTKey = true;
    public string MTKeyServiceNumber = "mt1111111111";
    public string MTKeyProjectName = "mtkj";
}
public class TimeEncryptionData
{
    public bool isTime = true;
    public DateTime LimitDateTime = DateTime.Now;
    public float CloseTime = 10;
    
    public DateTime systemDateTime = DateTime.Now;
}


