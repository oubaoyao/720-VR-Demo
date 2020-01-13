
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MTFrame.MTFile
{
    /// <summary>
    /// byte 加密基类
    /// </summary>
    public class ByteEncryptBase
    {
        /// <summary>
        /// 加密文件后缀
        /// </summary>
        protected string exten = ".mt";

        protected bool Error = false;
        

        /// <summary>
        /// 读取文档信息
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="encryptModeType">加密类型</param>
        /// <returns></returns>
        public FileObject ReadData(string path, EncryptModeType encryptModeType)
        {
            byte[] data = new byte[] { };
            switch (encryptModeType)
            {
                case EncryptModeType.None:
                    data = None_Read(path);
                    break;
                case EncryptModeType.one_LevelByteEncryption:
                    one_EncryptPath(ref path);
                    data = ByteEncryption_Read(path);
                    break;
                case EncryptModeType.two_LevelByteEncryption:
                    two_EncryptPath(ref path);
                    data = ByteEncryption_Read(path);
                    break;
                case EncryptModeType.three_LevelByteEncryption:
                    three_EncryptPath(ref path);
                    data = ByteEncryption_Read(path);
                    break;
                default:
                    break;
            }
            return new FileObject { Buffet = data, isError = Error };
        }


        /// <summary>
        /// 读取文档信息
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="encryptModeType">加密类型</param>
        /// <returns></returns>
        public void ReadAsyncData(string path, EncryptModeType encryptModeType, System.Action<FileObject> OnCall)
        {
            switch (encryptModeType)
            {
                case EncryptModeType.None:
                    None_AsyncRead(path, OnCall);
                    break;
                case EncryptModeType.one_LevelByteEncryption:
                    one_EncryptPath(ref path);
                    ByteEncryption_AsyncRead(path, OnCall);
                    break;
                case EncryptModeType.two_LevelByteEncryption:
                    two_EncryptPath(ref path);
                    ByteEncryption_AsyncRead(path, OnCall);
                    break;
                case EncryptModeType.three_LevelByteEncryption:
                    three_EncryptPath(ref path);
                    ByteEncryption_AsyncRead(path, OnCall);
                    break;
                default:
                    break;
            }

        }


        /// <summary>
        /// 读取文档信息
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="encryptModeType">加密类型</param>
        /// <returns></returns>
        public void ReadWebData(string path, EncryptModeType encryptModeType, System.Action<FileObject> OnCall)
        {
            switch (encryptModeType)
            {
                case EncryptModeType.None:
                    None_WebRead(path, OnCall);
                    break;
                case EncryptModeType.one_LevelByteEncryption:
                    one_EncryptPath(ref path);
                    ByteEncryption_WebRead(path, OnCall);
                    break;
                case EncryptModeType.two_LevelByteEncryption:
                    two_EncryptPath(ref path);
                    ByteEncryption_WebRead(path, OnCall);
                    break;
                case EncryptModeType.three_LevelByteEncryption:
                    three_EncryptPath(ref path);
                    ByteEncryption_WebRead(path, OnCall);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="data">数据</param>
        /// <param name="encryptModeType">加密类型</param>
        public void WriteData(string path, byte[] data, EncryptModeType encryptModeType)
        {
            switch (encryptModeType)
            {
                case EncryptModeType.None:
                    None_Write(path, data);
                    break;
                case EncryptModeType.one_LevelByteEncryption:
                    one_EncryptPath(ref path);
                    ByteEncryption_Write(path, data);
                    break;
                case EncryptModeType.two_LevelByteEncryption:
                    two_EncryptPath(ref path);
                    ByteEncryption_Write(path, data);
                    break;
                case EncryptModeType.three_LevelByteEncryption:
                    three_EncryptPath(ref path);
                    ByteEncryption_Write(path, data);
                    break;
                default:
                    break;
            }
        }


        #region 读取方式

        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual byte[] None_Read(string path)
        {
            byte[] data = new byte[] { };
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                data = File.ReadAllBytes(path);
                Error = true;
            }
            else
            {
                Error = false;
                Debug.Log("文件不存在=path:" + path);
                return null;
            }
            return data;
        }

        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual async void None_AsyncRead(string path, System.Action<FileObject> OnCall)
        {
            string _path = path;
            System.Action<FileObject> _OnCall = OnCall;
            await Task.Run(() => 
            {
               byte[] data= None_Read(path);
                OnCall?.Invoke(new FileObject { Buffet = data, isError = Error });

            }); 
        }

        /// <summary>
        /// 无加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual void None_WebRead(string path, System.Action<FileObject> OnCall)
        {
            MainSystem.Instance.ReadWebData(path, (w) =>
            {
                if (w.error == null)
                {
                    OnCall?.Invoke(new FileObject { Buffet = w.downloadHandler.data, isError = true });
                }
                else
                {
                    Error = false;
                    OnCall?.Invoke(new FileObject { Buffet = null, isError = false });
                    Debug.Log("文件不存在=path:" + path);
                }
            });
        }

        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual byte[] ByteEncryption_Read(string path)
        {

            FileInfo fileInfo = default(FileInfo);

            byte[] data = new byte[] { };

            fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                string str = File.ReadAllText(path);
                string[] strs = str.Split('-');
                data = new byte[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    data[i] = Convert.ToByte(strs[i], 16);
                }
                Error = true;
            }
            else
            {
                Error = false;
                Debug.Log("文件不存在=path:" + path);
                return null;
            }
            return data;
        }

        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual async void ByteEncryption_AsyncRead(string path, System.Action<FileObject> OnCall)
        {
            string _path = path;
            System.Action<FileObject> _OnCall = OnCall;
            await Task.Run(() =>
            {
                byte[] data = ByteEncryption_Read(path);

                OnCall?.Invoke(new FileObject { Buffet = data, isError = Error });
            });
        }

        /// <summary>
        /// byte加密读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual void ByteEncryption_WebRead(string path, System.Action<FileObject> OnCall)
        {
            byte[] data = new byte[1024];
            MainSystem.Instance.ReadWebData(path, (w) =>
            {
                if (w.error == null)
                {
                    string[] strs = w.downloadHandler.text.Split('-');
                    data = new byte[strs.Length];
                    for (int i = 0; i < strs.Length; i++)
                    {
                        data[i] = Convert.ToByte(strs[i], 16);
                    }
                    Error = true;

                    OnCall?.Invoke(new FileObject { Buffet = data, isError = true });
                }
                else
                {
                    Error = false;
                    OnCall?.Invoke(new FileObject { Buffet = null, isError = false });
                    Debug.Log("文件不存在=path:" + path);
                }
            });

        }
        #endregion

        #region 写入方式
        /// <summary>
        /// 无加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected virtual void None_Write(string path, byte[] data)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
            }
            if (data != null)
            {
                File.WriteAllBytes(path, data);
            }
            else
            {
                Debug.Log("数据为空=data:" + data);
            }
        }

        /// <summary>
        /// byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected virtual void ByteEncryption_Write(string path, byte[] data)
        {
            FileInfo fileInfo = default(FileInfo);

            fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
            }
            if (data != null)
            {
                string a = BitConverter.ToString(data);
                File.WriteAllText(path, a);
            }
            else
            {
                Debug.Log("数据为空=data:" + data);
            }
        }

        #endregion


        /// <summary>
        /// 一级byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        private string one_EncryptPath(ref string path)
        {
            byte[] buffer_0 = default(byte[]);
            string name_0 = "";

            string[] vs = path.Split('/');

            buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

            name_0 = BitConverter.ToString(buffer_0, 0);

            path = path.Substring(0, path.LastIndexOf('/'));

            path += "/" + name_0 + exten;
            return path;
        }
        /// <summary>
        /// 二级byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        private string two_EncryptPath(ref string path)
        {
            byte[] buffer_0 = default(byte[]);
            byte[] buffer_1 = default(byte[]);
            string name_0 = "";
            string name_1 = "";

            string[] vs = path.Split('/');

            buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 2]);
            buffer_1 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

            name_0 = BitConverter.ToString(buffer_0, 0);
            name_1 = BitConverter.ToString(buffer_1, 0);

            path = path.Substring(0, path.LastIndexOf('/'));
            path = path.Substring(0, path.LastIndexOf('/'));

            path += "/" + name_0;
            path += "/" + name_1 + exten;
            return path;
        }
        /// <summary>
        /// 三级byte加密写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        private string three_EncryptPath(ref string path)
        {
            byte[] buffer_0 = default(byte[]);
            byte[] buffer_1 = default(byte[]);
            byte[] buffer_2 = default(byte[]);
            string name_0 = "";
            string name_1 = "";
            string name_2 = "";

            string[] vs = path.Split('/');

            buffer_0 = Encoding.UTF8.GetBytes(vs[vs.Length - 3]);
            buffer_1 = Encoding.UTF8.GetBytes(vs[vs.Length - 2]);
            buffer_2 = Encoding.UTF8.GetBytes(vs[vs.Length - 1]);

            name_0 = BitConverter.ToString(buffer_0, 0);
            name_1 = BitConverter.ToString(buffer_1, 0);
            name_2 = BitConverter.ToString(buffer_2, 0);

            path = path.Substring(0, path.LastIndexOf('/'));
            path = path.Substring(0, path.LastIndexOf('/'));
            path = path.Substring(0, path.LastIndexOf('/'));

            path += "/" + name_0;
            path += "/" + name_1;
            path += "/" + name_2 + exten;
            return path;
        }
    }


}