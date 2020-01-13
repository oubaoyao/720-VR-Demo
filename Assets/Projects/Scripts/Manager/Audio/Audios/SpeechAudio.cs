using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MTFrame.MTAudio
{

    /// <summary>
    /// 对话音效
    /// </summary>
    public class SpeechAudio : BaseAudio
    {
        protected override string defaultPath
        {
            get
            {
                return AudioPath.speechAudioPath;
            }
            set { }
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="audioParamete"></param>
        public override AudioObject Play(string path, Transform parent, AudioParamete audioParamete)
        {
            StopAllAudio();
            return base.Play(path, parent, audioParamete);
        }
        /// <summary>
        /// 添加音效物体
        /// </summary>
        /// <param name="path"></param>
        /// <param name="trans"></param>
        /// <param name="audioParamete"></param>
        /// <returns></returns>
        protected override AudioObject AddAudioObject(string path, Transform trans, AudioParamete audioParamete)
        {
            AudioObject audioObject = base.AddAudioObject(path, trans, audioParamete);
            if (trans == null)
            {
                if (defaultParent)
                    audioObject.transform.parent = defaultParent;
                else
                {
                    defaultParent = new GameObject("SpeechAudioManage").transform;
                    GameObject.DontDestroyOnLoad(defaultParent);
                    audioObject.transform.parent = defaultParent;
                }
            }
            return audioObject;
        }
    }
}