using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MTFrame.MTFile
{
    /// <summary>
    ///  EXR文档读写
    /// </summary>
    public class EXRFileSave : ByteEncryptBase
    {
        #region 读取方式
        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] None_Read(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Extension.ToLower() != ".exr" && fileInfo.Extension.ToLower() != ".mt")
            {
                Debug.Log("读取失败400：文件格式错误:" + fileInfo.Extension);
                return null;
            }
            return base.None_Read(path);
        }
        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] ByteEncryption_Read(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Extension.ToLower() != ".exr" && fileInfo.Extension.ToLower() != ".mt")
            {
                Debug.Log("读取失败400：文件格式错误:" + fileInfo.Extension);
                return null;
            }
            return base.ByteEncryption_Read(path);
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
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Extension.ToLower() != ".exr" && fileInfo.Extension.ToLower() != ".mt")
            {
                Debug.Log("写入失败400：文件格式错误:" + fileInfo.Extension);
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
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Extension.ToLower() != ".exr" && fileInfo.Extension.ToLower() != ".mt")
            {
                Debug.Log("写入失败400：文件格式错误:" + fileInfo.Extension);
            }
            base.ByteEncryption_Write(path, data);
        }
        #endregion
    }
}