using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MTFrame.MTAudio
{
    /// <summary>
    /// 音频对象
    /// </summary>
    public class AudioObject : MonoBehaviour
    {
        /// <summary>
        /// 音频名字
        /// </summary>
        public string audioName { get { return audioSource.clip == null ? "" : audioSource.clip.name; } }
        /// <summary>
        /// 声音是否正在播放
        /// </summary>
        public bool isPlay { get { return audioSource.isPlaying; } }
        /// <summary>
        /// 声音是否暂停
        /// </summary>
        public bool isPause;
        /// <summary>
        /// 标准声音
        /// </summary>
        public float volume;
        /// <summary>
        /// 音效长度
        /// </summary>
        public float soundLength
        {
            get
            {
                if (clip == null) return 0;
                return clip.length;
            }
        }
        //音频剪辑
        [SerializeField]
        private AudioClip clip;

        public AudioSource audioSource { get; private set; }


        public void Awake()
        {
            audioSource = FindTool.GetComponent<AudioSource>(this.transform);
            audioSource.playOnAwake = false;
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="soundPath"></param>
        /// <param name="isLoop"></param>
        /// <param name="volume"></param>
        public void init(AudioClip audio, AudioParamete audioParamete)
        {
            this.clip = audio;

            audioSource.clip = clip;
            audioSource.loop = audioParamete.loop;
            audioSource.volume = audioParamete.volume;
            volume = audioParamete.volume;
            audioSource.spatialBlend = audioParamete.audio3DValue;
            audioSource.minDistance = audioParamete.minDistance;
            audioSource.maxDistance = audioParamete.maxDistance;
        }

        /// <summary>
        /// 设置音频数据
        /// </summary>
        public void SetAudioData(AudioParamete audioData)
        {
            audioSource.clip = clip;
            audioSource.loop = audioData.loop;
            audioSource.volume = audioData.volume;
            volume = audioData.volume;
            audioSource.spatialBlend = audioData.audio3DValue;
            audioSource.minDistance = audioData.minDistance;
            audioSource.maxDistance = audioData.maxDistance;
        }
        /// <summary>
        /// 播放声音
        /// </summary>
        public void Play()
        {
            audioSource.Play();
            isPause = false;
        }
        /// <summary>
        /// 暂停声音
        /// </summary>
        /// <param name="stopTime"></param>
        public void Pause()
        {
            audioSource.Pause();
            isPause = true;
        }
        /// <summary>
        /// 停止声音
        /// </summary>
        /// <param name="stopTime"></param>
        public void Stop()
        {
            audioSource.Stop();
            isPause = false;
        }

        /// <summary>
        /// 设置声音
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume)
        {
            audioSource.volume = this.volume * volume;
        }
    }
}