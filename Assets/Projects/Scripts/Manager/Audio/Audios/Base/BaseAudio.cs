using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MTFrame. MTAudio
{

    /// <summary>
    /// 音频基类
    /// </summary>
    public abstract class BaseAudio
    {
        /// <summary>
        /// 存放所有音效的列表
        /// </summary>
        protected List<AudioObject> audioList;

        /// <summary>
        ///默认父级物体 
        /// </summary>
        protected Transform defaultParent;

        /// <summary>
        /// 默认路径
        /// </summary>
        protected abstract string defaultPath { get;set; }


        public void Init()
        {
            audioList = new List<AudioObject>();
        }

        /// <summary>
        /// 播放
        /// </summary>
        public virtual AudioObject Play(string path, Transform parent, AudioParamete audioParamete)
        {
            if (parent == null)
                parent = defaultParent;//更换成默认目标父体
            AudioObject audio = default(AudioObject);//定义默认空的音频
            try
            {
                audio = audioList.Find(a => !a.isPlay && a.transform.parent == parent && a.audioName == path);//根据条件返回音频物体

                if (audio == null)
                {
                    audio = AddAudioObject(path, parent, audioParamete);//没有符合的音频，新添加一个音频
                }
                else
                {
                    audio.SetAudioData(audioParamete);//找到符合要求的音频更换音频数据
                }
                audio.Play();//播放音频
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Data);
                audioList.RemoveNull();//清除空的音频物体
            }
            return audio;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause(string path, Transform parent)
        {
            if (parent == null)
                parent = defaultParent;//更换成默认目标父体
            AudioObject audio = default(AudioObject);//定义默认空的音频
            try
            {
                audio = audioList.Find(a => a.isPlay && a.transform.parent == parent && a.audioName == path);//根据条件返回音频物体

                if (audio)
                    audio.Pause();//找到符合要求的音频，暂停音频

            }
            catch (Exception ex)
            {
                Debug.Log("音效错误" + ex.Data);
                audioList.RemoveNull();//清除空的音频物体
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="path"></param>
        public virtual void Stop(string path, Transform parent)
        {
            if (parent == null)
                parent = defaultParent;//更换成默认目标父体
            AudioObject audio = default(AudioObject);//定义默认空的音频
            try
            {
                for (int i = 0; i < audioList.Count; i++)
                {
                    audio = audioList[i];
                    if (audioList[i] != null && audioList[i].transform.parent == parent && audioList[i].audioName == path && audioList[i].isPlay)
                    {
                        audioList[i].Stop();//停止音频
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("音效错误" + ex.Data);
                audioList.RemoveNull();//清除空的音频物体
            }
        }

        /// <summary>
        /// 暂停所有音频
        /// </summary>
        public void PauseAllAudio()
        {
            try
            {
                for (int i = 0; i < audioList.Count; i++)
                {
                    if (audioList[i] != null && audioList[i].isPlay)
                    {
                        audioList[i].Pause();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("音效错误" + ex.Data);
                audioList.RemoveNull();
            }
        }
        /// <summary>
        /// 播放所有暂停音效
        /// </summary>
        /// <param name="audioType"></param>
        public void PlayAllPauseAudio()
        {
            try
            {
                for (int i = 0; i < audioList.Count; i++)
                {
                    if (audioList[i] != null && audioList[i].isPause)
                    {
                        audioList[i].Play();
                    }
                }
            }
            catch
            {
                audioList.RemoveNull();
            }
        }
        /// <summary>
        /// 停止所有音频
        /// </summary>
        public void StopAllAudio()
        {
            try
            {
                for (int i = 0; i < audioList.Count; i++)
                {
                    if (audioList[i] != null && audioList[i].isPlay)
                    {
                        audioList[i].Stop();
                    }
                }
            }
            catch
            {
                audioList.RemoveNull();
            }
        }

        /// <summary>
        /// 生成音频对象
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="parent"></param>
        /// <param name="audioParamete"></param>
        /// <returns></returns>
        protected virtual AudioObject AddAudioObject(string path, Transform parent, AudioParamete audioParamete)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(defaultPath + path);
            GameObject g = new GameObject(path);
            AudioObject audioObject = g.AddComponent<AudioObject>();
            audioObject.init(audioClip, audioParamete);
            if (parent)
            {
                audioObject.transform.parent = parent;
                audioObject.transform.localEulerAngles = Vector3.zero;
                audioObject.transform.localPosition = Vector3.zero;
            }
            audioList.Add(audioObject);
            return audioObject;
        }


        /// <summary>
        /// 设置音频音量
        /// </summary>
        public void SetVolume(float volume)
        {
            try
            {
                for (int i = 0; i < audioList.Count; i++)
                {
                    audioList[i].SetVolume(volume);
                }
            }
            catch
            {
                audioList.RemoveNull();
            }
        }
    }
}