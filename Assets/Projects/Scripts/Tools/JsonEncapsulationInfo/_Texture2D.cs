#region 文件信息
/*
 * 文件名:		_Texture
 * 作者:			Mink
 * 创建时间:   2018年07月11日 15:46:56
 * 公司:			广州市梦途信息科技有限责任公司
 * Unity版本:   2017.2.0f3
 * 项目名称:    StarsPlan_TotalControlCabin
 * 描述信息:
 * 
*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class _Texture2D
{
    public _Texture2D(Texture2D texture2D, TextureType textureType = TextureType.PNG)
    {
        Set(texture2D, textureType);
    }

    [HideInInspector]
    public string textureBase64;

    public Texture2D Get()
    {
        Texture2D texture2D = new Texture2D(0, 0);
        if (textureBase64 != null)
        {
            byte[] buffe = Convert.FromBase64String(textureBase64);
            if (buffe != null)
            {
                texture2D.LoadImage(buffe);
                texture2D.Apply();
            }
        }
        return texture2D;
    }
    public void Set(Texture2D texture2D, TextureType textureType = TextureType.PNG)
    {
        if (texture2D != null)
            switch (textureType)
            {
                case TextureType.PNG:
                    textureBase64 = Convert.ToBase64String(texture2D.EncodeToPNG());
                    break;
                case TextureType.JPG:
                    textureBase64 = Convert.ToBase64String(texture2D.EncodeToJPG());
                    break;
                case TextureType.EXR:
                    textureBase64 = Convert.ToBase64String(texture2D.EncodeToEXR());
                    break;
                default:
                    break;
            }
    }
}
public enum TextureType
{
    PNG,
    JPG,
    EXR
}