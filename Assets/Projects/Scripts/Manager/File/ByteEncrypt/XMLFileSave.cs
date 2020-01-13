using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MTFrame.MTFile
{
    /// <summary>
    ///  XML文档读写
    /// </summary>
    public class XMLFileSave : ByteEncryptBase
    {
        #region 读取方式
        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] None_Read(string path)
        {
            return base.None_Read(path);
        }
        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override byte[] ByteEncryption_Read(string path)
        {
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
            base.None_Write(path, data);
        }
        /// <summary>
        /// byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected override void ByteEncryption_Write(string path, byte[] data)
        {
            base.ByteEncryption_Write(path, data);
        }
        #endregion
    }
}