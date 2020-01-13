#region 文件信息
/*
 * 文件名:		_Vector2
 * 作者:			Mink
 * 创建时间:   2018年07月11日 15:46:08
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
    public class _Vector2
    {
        public _Vector2(Vector2 vector2)
        {
            x = vector2.x;
            y = vector2.y;
        }
        public float x, y;
        public Vector2 Get()
        {
            return new Vector2(x, y);
        }

        public void Set(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }  
    }
