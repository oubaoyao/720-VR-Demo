#region 文件信息
/*
 * 文件名:		_Vector4
 * 作者:			Mink
 * 创建时间:   2018年07月11日 15:46:29
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
    public class _Vector4
    {
        public _Vector4(Vector4 vector4)
        {
            x = vector4.x;
            y = vector4.y;
            z = vector4.z;
            w = vector4.w;
        }
        public float x, y, z, w;

        public Vector4 Get()
        {
            return new Vector4(x,y,z,w);
        }

        public void Set(Vector4 vector )
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
            w = vector.w;
        }
    }