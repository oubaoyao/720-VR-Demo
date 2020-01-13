#region 文件信息
/*
 * 文件名:		_Vector3
 * 作者:			Mink
 * 创建时间:   2018年07月11日 15:46:19
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
    public class _Vector3  {

        public _Vector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public float x, y, z;

        public Vector3 Get()
        {
            return new Vector3(x, y, z);
        }

        public void Set(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }