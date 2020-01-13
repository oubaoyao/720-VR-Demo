using MTFrame.MTEvent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 用于拓展方法功能
/// </summary>
public static class ExtendTool
{
    #region List拓展
    /// <summary>
    /// 去除集合中空的选项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void RemoveNull<T>(this List<T> list)
    {
        List<T> vs = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(null))
            {
                vs.Add(list[i]);
            }
        }
        foreach (var item in vs)
        {
            list.Remove(item);
        }
    }
    
    public static T GetLast<T>(this List<T> list)
    {
        T t = default(T);

        return t;
    }

    #endregion

    public static bool IsNumeric(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }
    public static bool IsInt(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }
    public static bool IsUnsign(this string value)
    {
        return Regex.IsMatch(value, @"^\d*[.]?\d*$");
    }
    public static bool isTel(this string strInput)
    {
        return Regex.IsMatch(strInput, @"\d{3}-\d{8}|\d{4}-\d{7}");
    }

    /// <summary>
    /// 压缩图片
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns></returns>
    public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        source = result;
        return source;
    }
}