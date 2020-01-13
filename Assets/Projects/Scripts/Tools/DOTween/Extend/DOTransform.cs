
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拓展Transform方法
/// </summary>
public static class DOTransform
{

    #region 世界坐标移动

    //暂存的数据用于避免重复开启
    private static Dictionary<Transform, TweenObject> MoveTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveXTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveYTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> MoveZTransformDic = new Dictionary<Transform, TweenObject>();
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetPos"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOMove(this Transform obj, Vector3 targetPos, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {

        if (MoveTransformDic.ContainsKey(obj))
        {
            MoveTransformDic[obj].Stop();//暂停上一个
            MoveTransformDic.Remove(obj);//清除上一个
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

        MoveTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOMoveX(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3(targetValue, obj.position.y, obj.position.z);

        if (MoveXTransformDic.ContainsKey(obj))
        {
            MoveXTransformDic[obj].Stop();//暂停上一个
            MoveXTransformDic.Remove(obj);//清除上一个
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

        MoveXTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOMoveY(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3( obj.position.x, targetValue,obj.position.z);

        if (MoveYTransformDic.ContainsKey(obj))
        {
            MoveYTransformDic[obj].Stop();//暂停上一个
            MoveYTransformDic.Remove(obj);//清除上一个
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

        MoveYTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOMoveZ(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = new Vector3( obj.position.x,obj.position.y, targetValue);

        if (MoveZTransformDic.ContainsKey(obj))
        {
            MoveZTransformDic[obj].Stop();//暂停上一个
            MoveZTransformDic.Remove(obj);//清除上一个
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

        MoveZTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }


    #endregion

    #region 自身坐标移动

    //暂存的数据用于避免重复开启
    private static Dictionary<Transform, TweenObject> LocalMoveTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveXTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveYTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> LocalMoveZTransformDic = new Dictionary<Transform, TweenObject>();
    /// <summary>
    /// 拓展的Transform移动方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetPos"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOLocalMove(this Transform obj, Vector3 targetPos, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {

        if (LocalMoveTransformDic.ContainsKey(obj))
        {
            LocalMoveTransformDic[obj].Stop();//暂停上一个
            LocalMoveTransformDic.Remove(obj);//清除上一个
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

        LocalMoveTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOLocalMoveX(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.x = targetValue;

        if (LocalMoveXTransformDic.ContainsKey(obj))
        {
            LocalMoveXTransformDic[obj].Stop();//暂停上一个
            LocalMoveXTransformDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        float posX = obj.localPosition.x;//记录最初角度数据
        Vector3 startPos = obj.localPosition;//记录开始角度数据
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

        LocalMoveXTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOLocalMoveY(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.y = targetValue;

        if (LocalMoveYTransformDic.ContainsKey(obj))
        {
            LocalMoveYTransformDic[obj].Stop();//暂停上一个
            LocalMoveYTransformDic.Remove(obj);//清除上一个
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

        LocalMoveYTransformDic.Add(obj, tweenObject);
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
    public static TweenObject DOLocalMoveZ(this Transform obj, float targetValue, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 targetPos = obj.localPosition;
        targetPos.z = targetValue;

        if (LocalMoveZTransformDic.ContainsKey(obj))
        {
            LocalMoveZTransformDic[obj].Stop();//暂停上一个
            LocalMoveZTransformDic.Remove(obj);//清除上一个
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

        LocalMoveZTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }
    #endregion


    #region 缩放

    //暂存的数据用于避免重复开启
    private static Dictionary<Transform, TweenObject> SizeTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> SizeXTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> SizeYTransformDic = new Dictionary<Transform, TweenObject>();
    private static Dictionary<Transform, TweenObject> SizeZTransformDic = new Dictionary<Transform, TweenObject>();
    /// <summary>
    /// 拓展的Transform缩放方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetSize"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOSize(this Transform obj, Vector3 targetSize, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {

        if (SizeTransformDic.ContainsKey(obj))
        {
            SizeTransformDic[obj].Stop();//暂停上一个
            SizeTransformDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;

        Vector3 size = obj.localScale;//记录最初缩放
        Vector3 startSize = obj.localScale;//记录开始缩放
        float sizeSpace = Vector3.Distance(size, targetSize);//记录最初距离

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

            size = Vector3.MoveTowards(size, targetSize, fpsTimeDir * sizeSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(size, targetSize) / sizeSpace);//通过正常值得出当前进度

            obj.localScale = Vector3.LerpUnclamped(startSize, targetSize, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localScale = targetSize;
        };

        SizeTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }

    /// <summary>
    /// 拓展的Transform缩放方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetSize"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOSizeX(this Transform obj, float targetSize, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 _targetSize = new Vector3(targetSize, obj.localScale.y, obj.localScale.z);

        if (obj.localScale == _targetSize)
        {
            return new TweenObject((tween) => { return 1; });
        }
        if (SizeXTransformDic.ContainsKey(obj))
        {
            SizeXTransformDic[obj].Stop();//暂停上一个
            SizeXTransformDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 size = obj.localScale;//记录最初位置
        Vector3 startSize = obj.localScale;//记录开始位置
        float sizeSpace = Vector3.Distance(size, _targetSize);//记录最初距离

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

            size = Vector3.MoveTowards(size, _targetSize, fpsTimeDir * sizeSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(size, _targetSize) / sizeSpace);//通过正常值得出当前进度

            obj.localScale = Vector3.LerpUnclamped(startSize, _targetSize, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localScale = _targetSize;
        };

        SizeXTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform缩放方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetSize"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOSizeY(this Transform obj, float targetSize, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
        Vector3 _targetSize = new Vector3( obj.localScale.x, targetSize, obj.localScale.z);

        if (SizeYTransformDic.ContainsKey(obj))
        {
            SizeYTransformDic[obj].Stop();//暂停上一个
            SizeYTransformDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 size = obj.localScale;//记录最初位置
        Vector3 startSize = obj.localScale;//记录开始位置
        float sizeSpace = Vector3.Distance(size, _targetSize);//记录最初距离

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

            size = Vector3.MoveTowards(size, _targetSize, fpsTimeDir * sizeSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(size, _targetSize) / sizeSpace);//通过正常值得出当前进度

            obj.localScale = Vector3.LerpUnclamped(startSize, _targetSize, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localScale = _targetSize;
        };

        SizeYTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }
    /// <summary>
    /// 拓展的Transform缩放方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetSize"></param>
    /// <param name="needTime"></param>
    /// <param name="tweenMode"></param>
    /// <returns></returns>
    public static TweenObject DOSizeZ(this Transform obj, float targetSize, float needTime, TweenMode tweenMode = TweenMode.UnityTimeLineImpact)
    {
            Vector3 _targetSize = new Vector3( obj.localScale.x, obj.localScale.y, targetSize);

        if (SizeZTransformDic.ContainsKey(obj))
        {
            SizeZTransformDic[obj].Stop();//暂停上一个
            SizeZTransformDic.Remove(obj);//清除上一个
        }

        float timeNow = Time.realtimeSinceStartup;


        Vector3 size = obj.localScale;//记录最初位置
        Vector3 startSize = obj.localScale;//记录开始位置
        float sizeSpace = Vector3.Distance(size, _targetSize);//记录最初距离

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

            size = Vector3.MoveTowards(size, _targetSize, fpsTimeDir * sizeSpace / needTime);//没有运用动画曲线正常的值

            float f = 1 - (Vector3.Distance(size, _targetSize) / sizeSpace);//通过正常值得出当前进度

            obj.localScale = Vector3.LerpUnclamped(startSize, _targetSize, tween.GetAnimationCurveValue());//通过当前进度得到动画曲线的值

            timeNow = Time.realtimeSinceStartup;//保存这一帧的时间

            return f;
        });

        tweenObject.onComplete += () =>
        {
            obj.localScale = _targetSize;
        };

        SizeZTransformDic.Add(obj, tweenObject);
        return tweenObject;
    }


    #endregion
}