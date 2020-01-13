using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MTFrame.JsonTool;

namespace MTFrame.MTFile
{
    /// <summary>
    ///  Json文档读写
    /// </summary>
    public class JsonFileSave : ByteEncryptBase
    {

        #region 读取方式
        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] None_Read(string path)
        {
            byte[] buff = base.None_Read(path);
            if (buff == null)
                return buff;
            string str = Encoding.UTF8.GetString(buff);
            if (!JsonCheckTool.IsJson(str))
            {
                Debug.Log("读取失败400：数据不是json:" + str);
                return null;
            }

            return buff;
        }
        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] ByteEncryption_Read(string path)
        {
            byte[] buff = base.ByteEncryption_Read(path);
            if (buff == null)
                return buff;

            string str = Encoding.UTF8.GetString(buff);
            if (!JsonCheckTool.IsJson(str))
            {
                Debug.Log("读取失败400：数据不是json:" + str);
                return null;
            }

            return buff;
        }
        #endregion

        #region 写入方式
        /// <summary>
        /// 无加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected override void None_Write(string path, byte[] data)
        {
            string str = Encoding.UTF8.GetString(data);
            if (!JsonCheckTool.IsJson(str))
            {
                Debug.Log("写入失败400：数据不是json:" + str);
                return;
            }
            base.None_Write(path, data);
        }
        /// <summary>
        /// byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected override void ByteEncryption_Write(string path, byte[] data)
        {
            string str = Encoding.UTF8.GetString(data);
            if (!JsonCheckTool.IsJson(str))
            {
                Debug.Log("写入失败400：数据不是json:" + str);
                return;
            }
            base.ByteEncryption_Write(path, data);
        }
        #endregion
    }
}