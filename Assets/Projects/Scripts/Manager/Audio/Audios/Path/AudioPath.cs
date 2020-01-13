using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MTFrame.MTAudio
{
    /// <summary>
    /// 音频路径
    /// </summary>
    public struct AudioPath
    {
        /// <summary>
        /// 效果音频路径
        /// </summary>
        public const string effectAudioPath = "Audio/EffsetAudio/";
        /// <summary>
        /// 背景音频路径
        /// </summary>
        public const string bgmAudioPath = "Audio/BGMAudio/";
        /// <summary>
        /// 对话音频路径
        /// </summary>
        public const string speechAudioPath = "Audio/SpeechAudio/";

        /// <summary>
        /// 效果音频路径
        /// </summary>
        public struct EffsetAudioPath
        {
            public const string effset = "Effset";
        }
        /// <summary>
        /// 背景音频路径
        /// </summary>
        public struct BGMAudioPath
        {
            public const string bgm = "BGM";
        }
        /// <summary>
        /// 对话音频路径
        /// </summary>
        public struct SpeechAudioPath
        {
            public const string speech = "Speech";
        }
    }
}