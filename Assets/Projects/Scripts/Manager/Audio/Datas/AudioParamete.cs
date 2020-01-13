using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MTFrame.MTAudio
{
    /// <summary>
    /// 音频参数
    /// </summary>
    public class AudioParamete
    {
        /// <summary>
        /// 音量
        /// </summary>
        public float volume = 1;
        /// <summary>
        /// 循环
        /// </summary>
        public bool loop = false;
        /// <summary>
        /// 3D数值
        /// </summary>
        public float audio3DValue = 1;
        /// <summary>
        /// 最小距离
        /// </summary>
        public float minDistance = 1;
        /// <summary>
        /// 最大距离
        /// </summary>
        public float maxDistance = 500;
    }
}