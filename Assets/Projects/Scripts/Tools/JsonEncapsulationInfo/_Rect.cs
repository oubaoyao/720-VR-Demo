#region 文件信息
/*
 * 文件名:		_Rect
 * 作者:			Mink
 * 创建时间:   2018年07月11日 15:46:42
 * 公司:			广州市梦途信息科技有限责任公司
 * Unity版本:   2017.2.0f3
 * 项目名称:    StarsPlan_TotalControlCabin
 * 描述信息:
 * 
*/
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class _Rect
    {
        public _Rect(Rect  rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width ;
            height = rect.height;
        }

        public float x, y, width, height;

        public Rect Get()
        {
            return new Rect(x, y, width, height);
        }

        public void Set(Rect rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width;
            height = rect.height;
        }
    }