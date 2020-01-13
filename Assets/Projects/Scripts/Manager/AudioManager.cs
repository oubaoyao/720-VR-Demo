using MTFrame.MTAudio;
using UnityEngine;

/// <summary>
/// 音效管理器
/// </summary>
public class AudioManager
{
    //背景音效
    private static BGMAudio bgmAudio = new BGMAudio();
    //效果音效
    private static EffectAudio effectAudio = new EffectAudio();
    //对话音效
    private static SpeechAudio speechAudio = new SpeechAudio();

    public static void Init()
    {
        bgmAudio = new BGMAudio();
        bgmAudio.Init();

        effectAudio = new EffectAudio();
        effectAudio.Init();

        speechAudio = new SpeechAudio();
        speechAudio.Init();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    public static AudioObject PlayAudio(string path, Transform parent, AudioEnunType audioType, float volume = 1, bool loop = false)
    {
        AudioObject audio = default(AudioObject);
        switch (audioType)
        {
            case AudioEnunType.BGM:
                audio = bgmAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            case AudioEnunType.Effset:
                audio = effectAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            case AudioEnunType.Speech:
                audio = speechAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            default:
                break;
        }

        return audio;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    /// <param name="audioModeType"></param>
    public static AudioObject PlayAudio(string path, Transform parent, AudioEnunType audioType, AudioParamete audioData)
    {
        AudioObject audio = default(AudioObject);
        switch (audioType)
        {
            case AudioEnunType.BGM:
                audio = bgmAudio.Play(path, parent, audioData);
                break;
            case AudioEnunType.Effset:
                audio = effectAudio.Play(path, parent, audioData);
                break;
            case AudioEnunType.Speech:
                audio = speechAudio.Play(path, parent, audioData);
                break;
            default:
                break;
        }
        return audio;
    }


    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    public static AudioObject PlayAudio(string[] paths, Transform parent, AudioEnunType audioType, float volume = 1, bool loop = false)
    {
        string path = paths[Random.Range(0, paths.Length)];
        AudioObject audio = default(AudioObject);
        switch (audioType)
        {
            case AudioEnunType.BGM:
                audio = bgmAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            case AudioEnunType.Effset:
                audio = effectAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            case AudioEnunType.Speech:
                audio = speechAudio.Play(path, parent, new AudioParamete() { volume = volume, loop = loop, audio3DValue = 0 });
                break;
            default:
                break;
        }

        return audio;
    }

    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    /// <param name="audioModeType"></param>
    public static AudioObject PlayAudio(string[] paths, Transform parent, AudioEnunType audioType, AudioParamete audioData)
    {
        string path = paths[Random.Range(0, paths.Length)];
        AudioObject audio = default(AudioObject);
        switch (audioType)
        {
            case AudioEnunType.BGM:
                audio = bgmAudio.Play(path, parent, audioData);
                break;
            case AudioEnunType.Effset:
                audio = effectAudio.Play(path, parent, audioData);
                break;
            case AudioEnunType.Speech:
                audio = speechAudio.Play(path, parent, audioData);
                break;
            default:
                break;
        }
        return audio;
    }



    /// <summary>
    /// 暂停音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    public static void PauseAudio(string path, Transform parent, AudioEnunType audioType)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.Pause(path, parent);
                break;
            case AudioEnunType.Effset:
                effectAudio.Pause(path, parent);
                break;
            case AudioEnunType.Speech:
                speechAudio.Pause(path, parent);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    public static void StopAudio(string path, Transform parent, AudioEnunType audioType)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.Stop(path, parent);
                break;
            case AudioEnunType.Effset:
                effectAudio.Stop(path, parent);
                break;
            case AudioEnunType.Speech:
                speechAudio.Stop(path, parent);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 暂停所有音效
    /// </summary>
    /// <param name="audioType"></param>
    public static void PauseAllAudio(AudioEnunType audioType)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.PauseAllAudio();
                break;
            case AudioEnunType.Effset:
                effectAudio.PauseAllAudio();
                break;
            case AudioEnunType.Speech:
                speechAudio.PauseAllAudio();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 播放所有暂停音效
    /// </summary>
    /// <param name="audioType"></param>
    public static void PlayAllPauseAudio(AudioEnunType audioType)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.PlayAllPauseAudio();
                break;
            case AudioEnunType.Effset:
                effectAudio.PlayAllPauseAudio();
                break;
            case AudioEnunType.Speech:
                speechAudio.PlayAllPauseAudio();
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 停止所有音效
    /// </summary>
    /// <param name="audioType"></param>
    public static void StopAllAudio(AudioEnunType audioType)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.StopAllAudio();
                break;
            case AudioEnunType.Effset:
                effectAudio.StopAllAudio();
                break;
            case AudioEnunType.Speech:
                speechAudio.StopAllAudio();
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 设置声音大小
    /// </summary>
    /// <param name="audioType"></param>
    /// <param name="volume"></param>
    public static void SetVolume(AudioEnunType audioType, float volume)
    {
        switch (audioType)
        {
            case AudioEnunType.BGM:
                bgmAudio.SetVolume(volume);
                break;
            case AudioEnunType.Effset:
                effectAudio.SetVolume(volume);
                break;
            case AudioEnunType.Speech:
                speechAudio.SetVolume(volume);
                break;
            default:
                break;
        }
    }

}