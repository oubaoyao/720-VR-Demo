
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拓展Transform方法
/// </summary>
public static class DORectTransform
{

    #region 世界坐标移动

    //暂存的数据用于避免重复开启
    private static Dictionary<Transform, TweenObject> MoveCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveXCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveYCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveZCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetPos"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOMove(this RectTransform obj, Vector3 targetPos, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {

        if (MoveCanvasGroupDic.ContainsKey(obj))
        {
            MoveCanvasGroupDic[obj].Stop();//暂停上一个
            MoveCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        Vector3 pos = obj.position;//记录最初位置
        Vector3 startPos = obj.position;//记录开始位置
        float posSpace = Vector3.Distance(pos, targetPos);//记录最初距离

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            pos = Vector3.MoveTowards(pos, targetPos, fpsTimeDir * posSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(pos, targetPos) / posSpace);//通过正常值得出当前进度

            obj.position = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.position = targetPos;
        };

        MoveCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOMoveX(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3(targetValue, obj.position.y, obj.position.z);

        if (MoveXCanvasGroupDic.ContainsKey(obj))
        {
            MoveXCanvasGroupDic[obj].Stop();//暂停上一个
            MoveXCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 pos = obj.position;//记录最初位置
        Vector3 startPos = obj.position;//记录开始位置
        float posSpace = Vector3.Distance(pos, targetPos);//记录最初距离

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            pos = Vector3.MoveTowards(pos, targetPos, fpsTimeDir * posSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(pos, targetPos) / posSpace);//通过正常值得出当前进度

            obj.position = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.position = targetPos;
        };

        MoveXCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOMoveY(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3( obj.position.x, targetValue,obj.position.z);

        if (MoveYCanvasGroupDic.ContainsKey(obj))
        {
            MoveYCanvasGroupDic[obj].Stop();//暂停上一个
            MoveYCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 pos = obj.position;//记录最初位置
        Vector3 startPos = obj.position;//记录开始位置
        float posSpace = Vector3.Distance(pos, targetPos);//记录最初距离

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            pos = Vector3.MoveTowards(pos, targetPos, fpsTimeDir * posSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(pos, targetPos) / posSpace);//通过正常值得出当前进度

            obj.position = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.position = targetPos;
        };

        MoveYCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOMoveZ(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3( obj.position.x,obj.position.y, targetValue);

        if (MoveZCanvasGroupDic.ContainsKey(obj))
        {
            MoveZCanvasGroupDic[obj].Stop();//暂停上一个
            MoveZCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 pos = obj.position;//记录最初位置
        Vector3 startPos = obj.position;//记录开始位置
        float posSpace = Vector3.Distance(pos, targetPos);//记录最初距离

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            pos = Vector3.MoveTowards(pos, targetPos, fpsTimeDir * posSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(pos, targetPos) / posSpace);//通过正常值得出当前进度

            obj.position = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.position = targetPos;
        };

        MoveZCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }


    #endregion

    #region 自身坐标移动

    //暂存的数据用于避免重复开启
    private static Dictionary<Transform, TweenObject> LocalMoveCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveXCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveYCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveZCanvasGroupDic = new Dictionary<Transform, TweenObject>();
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetPos"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOLocalMove(this RectTransform obj, Vector3 targetPos, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        if (LocalMoveCanvasGroupDic.ContainsKey(obj))
        {
            LocalMoveCanvasGroupDic[obj].Stop();//暂停上一个
            LocalMoveCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        Vector3 pos = obj.localPosition;//记录最初位置
        Vector3 startPos = obj.localPosition;//记录开始位置
        float posSpace = Vector3.Distance(pos, targetPos);//记录最初距离

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            pos = Vector3.MoveTowards(pos, targetPos, fpsTimeDir * posSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(pos, targetPos) / posSpace);//通过正常值得出当前进度

            obj.localPosition = Vector3.LerpUnclamped(startPos, obj .forward*targetPos.z, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值
            obj.localPosition = Vector3.LerpUnclamped(startPos, obj.up * targetPos.y, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值
            obj.localPosition = Vector3.LerpUnclamped(startPos, obj.right  * targetPos.x, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localPosition = obj.forward * targetPos.z;
            obj.localPosition = obj.up * targetPos.y;
            obj.localPosition = obj.right * targetPos.x;
        };

        LocalMoveCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOLocalMoveX(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.x = targetValue;

        if (LocalMoveXCanvasGroupDic.ContainsKey(obj))
        {
            LocalMoveXCanvasGroupDic[obj].Stop();//暂停上一个
            LocalMoveXCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        float posX = obj.localPosition.x;//记录最初角度数据
        Vector3 startPos= obj.localPosition;//记录开始角度数据
        float posXSpace = Mathf.Abs(obj.localPosition.x - targetValue);//记录最初间隔

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            posX = Mathf.MoveTowards(posX, targetPos.x, fpsTimeDir * posXSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Mathf.Abs(targetPos.x - posX) / posXSpace);//通过正常值得出当前进度

            obj.localPosition = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值
            
            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localPosition = targetPos;
        };

        LocalMoveXCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOLocalMoveY(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.y = targetValue;
        
        if (LocalMoveYCanvasGroupDic.ContainsKey(obj))
        {
            LocalMoveYCanvasGroupDic[obj].Stop();//暂停上一个
            LocalMoveYCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        float posY = obj.localPosition.y;//记录最初角度数据
        Vector3 startPos = obj.localPosition;//记录开始角度数据
        float posYSpace = Mathf.Abs(obj.localPosition.y - targetValue);//记录最初间隔

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            posY = Mathf.MoveTowards(posY, targetPos.y, fpsTimeDir * posYSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Mathf.Abs(targetPos.y - posY) / posYSpace);//通过正常值得出当前进度
            
            obj.localPosition = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值
            
            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localPosition = targetPos;
        };

        LocalMoveYCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetValue"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOLocalMoveZ(this RectTransform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.z = targetValue;
        
        if (LocalMoveZCanvasGroupDic.ContainsKey(obj))
        {
            LocalMoveZCanvasGroupDic[obj].Stop();//暂停上一个
            LocalMoveZCanvasGroupDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        float posZ = obj.localPosition.z;//记录最初角度数据
        Vector3 startPos = obj.localPosition;//记录开始角度数据
        float posZSpace = Mathf.Abs(obj.localPosition.z - targetValue);//记录最初间隔

        TweenObject tweenObject = new TweenObject((tween) =>
        {
            float fpsTimeDir = 0;
            switch (tweenMode)
            {
                case TweenMode.UnityTimeLineImpact:
                    fpsTimeDir = Time.deltaTime;
                    break;
                case TweenMode.NoUnityTimeLineImpact:
                    fpsTimeDir = Time.realtimeSinceStartup - timeNow;
                    break;
                default:
                    break;
            }

            posZ = Mathf.MoveTowards(posZ, targetPos.z, fpsTimeDir * posZSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Mathf.Abs(targetPos.z - posZ) / posZSpace);//通过正常值得出当前进度


            obj.localPosition = Vector3.LerpUnclamped(startPos, targetPos, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值


            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localPosition = targetPos;
        };

        LocalMoveZCanvasGroupDic.Add(obj, tweenObject);
        return tweenObject;
    }
    #endregion


}