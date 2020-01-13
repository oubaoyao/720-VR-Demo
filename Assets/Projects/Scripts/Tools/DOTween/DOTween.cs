using MTFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate float TweenLogicBehavior(TweenObject tweenObject);
/// <summary>
/// DOTween 核心系统
/// </summary>
public class DOTween : MainBehavior
{
    private static DOTween instance;
    //单例
    public static DOTween Instance
    {
        get
        {
            instance = FindObjectOfType<DOTween>();
            if (instance == null)
            {
                instance = new GameObject("[DOTween]").AddComponent<DOTween>();
            }
            return instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        GameObject.DontDestroyOnLoad(this.gameObject);
        ReadAnimationCurve();
    }

    public void Init()
    {
    }

    /// <summary>
    /// 所有tweens
    /// </summary>
    private List<TweenObject> tweens= new List<TweenObject>();

    /// <summary>
    /// 动画曲线
    /// </summary>
    private Dictionary<TweenType, AnimationCurve> animationCurveDic = new Dictionary<TweenType, AnimationCurve>();


    private void FixedUpdate()
    {

        for (int i = 0; i < tweens.Count; i++)
        {
            tweens[i].UpdateLogic();
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tweenOgject"></param>
    internal void AddTween(TweenObject tweenOgject)
    {
        tweens.Add(tweenOgject);
    }
    /// <summary>
    /// 去除
    /// </summary>
    /// <param name="tweenOgject"></param>
    internal void RemoveTween(TweenObject tweenOgject)
    {
        tweens.Remove(tweenOgject);
    }

    /// <summary>
    /// 读取曲线
    /// </summary>
    private void ReadAnimationCurve()
    {
        string path = Application.streamingAssetsPath + "/DOTween/AnimationCurveDatas.txt";
        FileManager.ReadWeb(path, (fileObject) =>
        {
            if (!fileObject.isError)
            {
                Debug.Log("DOTween文件不存在");
                return;
            }
            string data = System.Text.ASCIIEncoding.UTF8.GetString(fileObject.Buffet);
            if(!data.IsJson())
            {
                Debug.Log("DOTween数据错误");
                return;
            }

            List<TweenAnimationCurveData> tweenAnimationCurveDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TweenAnimationCurveData>>(data);

            for (int i = 0; i < tweenAnimationCurveDatas.Count; i++)
            {
                AnimationCurve animationCurve = new AnimationCurve();

                animationCurve.keys = tweenAnimationCurveDatas[i].keyframes;

                animationCurveDic.Add(tweenAnimationCurveDatas[i].tweenType, animationCurve);
            }
        });
    }
    /// <summary>
    /// 获取动画曲线
    /// </summary>
    /// <param name="tweenType"></param>
    /// <returns></returns>
    public AnimationCurve GetEase(TweenType tweenType)
    {
        if (animationCurveDic.ContainsKey(tweenType))
        {
            return animationCurveDic[tweenType]; 
        }
        return null;
    }

}


public enum TweenMode
{
    /// <summary>
    /// 受到unity时间线的影响
    /// </summary>
    UnityTimeLineImpact,
    /// <summary>
    /// 不受到unity时间线的影响
    /// </summary>
    NoUnityTimeLineImpact,
}

