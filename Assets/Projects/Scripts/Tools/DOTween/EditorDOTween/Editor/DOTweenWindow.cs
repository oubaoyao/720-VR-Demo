using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DOTweenWindow : EditorWindow
{
    string bugReporterName = "";
    string description = "";
    GameObject buggyGameObject;
    static Rect windowRect = new Rect(300, 300, 600, 500);

    private static List<TweenAnimationCurveData> tweenAnimationCurveDatas = new List<TweenAnimationCurveData>();
    private static List<TweenAnimationInfo> tweenAnimationInfos = new List<TweenAnimationInfo>();
    private static TweenAnimationInfo tweenAnimationInfo;

    private static Rect rectGroup = new Rect(0, 0, 400, 300);
    private static float verticalSliderValue = 0;
    private bool isSetData;

    public static string tweenTypePath;
    private static bool isAdd;
    private static List<TweenType> tweenTypes = new List<TweenType>();
    private static Rect addRectGroup = new Rect(5, 0, 380, 150);
    private static float addVerticalSliderValue = 0;

    private static GUISkin _skin;

    private static Texture2D whiteTexture2D, grayTexture2D;

    private bool isOpen;
    //利用构造函数来设置窗口名称
    DOTweenWindow()
    {
        this.titleContent = new GUIContent("DOTweenWindow");

    }

    //添加菜单栏用于打开窗口
    [MenuItem("Tool/DOTweenWindow")]
    static void showWindow()
    {
        EditorWindow.GetWindowWithRect(typeof(DOTweenWindow), windowRect);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        tweenAnimationCurveDatas.Clear();
        tweenAnimationInfos.Clear();
        
        string path = Application.streamingAssetsPath + "/DOTween/AnimationCurveDatas.txt";
        FileInfo fileInfo = new FileInfo(path);
        string data = "";
        if (fileInfo.Exists)
        {
            data = File.ReadAllText(path);
            tweenAnimationCurveDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TweenAnimationCurveData>>(data);

            for (int i = 0; i < tweenAnimationCurveDatas.Count; i++)
            {
                AnimationCurve animationCurve = new AnimationCurve();

                animationCurve.keys = tweenAnimationCurveDatas[i].keyframes;

                tweenAnimationInfos.Add(new TweenAnimationInfo { tweenTypeData = tweenAnimationCurveDatas[i].tweenType, animationCurve = animationCurve });
            }
        }
        else
        {
            fileInfo.Directory.Create();
            Save();
        }

    }

    /// <summary>
    ///保存
    /// </summary>
    public void Save()
    {
        tweenAnimationCurveDatas.Clear();
        
        string path = Application.streamingAssetsPath + "/DOTween/AnimationCurveDatas.txt";

        for (int i = 0; i < tweenAnimationInfos.Count; i++)
        {
            tweenAnimationCurveDatas.Add(new TweenAnimationCurveData() { tweenType = tweenAnimationInfos[i].tweenTypeData, keyframes = tweenAnimationInfos[i].animationCurve.keys });
        }
        string data = Newtonsoft.Json.JsonConvert.SerializeObject(tweenAnimationCurveDatas);
        File.WriteAllText(path, data);
    }


    private void GetInit()
    {
        Init();
        isOpen = true;
    }

    void OnGUI()
    {
        if (!isOpen)
            GetInit();

        GUI.skin = Resources.Load<GUISkin>("MainWindow");

        if (whiteTexture2D == null)
        {
            whiteTexture2D = new Texture2D(800, 600);
            Color[] colors = new Color[800 * 600];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(1, 1, 1, 1);
            }
            whiteTexture2D.SetPixels(colors);
            whiteTexture2D.Apply();
        }

        if (grayTexture2D == null)
        {
            grayTexture2D = new Texture2D(800, 600);
            Color[] colors = new Color[800 * 600];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            grayTexture2D.SetPixels(colors);
            grayTexture2D.Apply();
        }

        GUILayout.BeginVertical();

        //绘制标题
        DrawBox(new Rect(5, 10, 590, 30), "DOTweenWindow", 24, FontStyle.Bold, TextAnchor.MiddleCenter);
        //绘制背景
        DrawBox(new Rect(5, 45, 590, 455), "", 0, FontStyle.Normal, TextAnchor.LowerCenter);

        //绘制背景
        DrawLabel(new Rect(20, 50, 200, 20), "当前曲线动画数量:", 16, FontStyle.Bold);
        DrawLabel(new Rect(300, 50, 100, 20), tweenAnimationInfos.Count.ToString() + "/个");



        //绘制背景
        DrawLabel(new Rect(20, 90, 200, 20), "所有曲线动画列表:", 16, FontStyle.Bold);

            AllAnimationCurveList();
        
        if(isAdd)
            AddTweenAnimationInfo();

        if (isSetData)
            SetTweenAnimationData();


        if (DrawButton(new Rect(400, 450, 80, 30), "初始化", 16, FontStyle.Bold, TextAnchor.MiddleCenter))
        {
            GetInit();
        }
        if (DrawButton(new Rect(500, 450, 80, 30), "保存", 16, FontStyle.Bold, TextAnchor.MiddleCenter))
        {
            Save();
            this.Close();
        }
    }

    private void OnDisable()
    {
        isOpen = false;
    }
    private void AllAnimationCurveList()
    {
        Clip(new Rect(190, 90, 400, 300), () =>
        {
            DrawBox(new Rect(0, 0, 400, 300), "", 0, FontStyle.Normal, TextAnchor.MiddleCenter);
            if (rectGroup.height > 300)
            {
                DrawVerticalSlider(new Rect(380, 5, 12, 290), ref verticalSliderValue, 1, 0);
                rectGroup.y = -(rectGroup.height - 300) * verticalSliderValue;
            }
            Group(rectGroup, () =>
            {
                AllTweenAnimation();
            });
        });
        if (DrawButton(new Rect(500, 400, 80, 30), "添加"))
        {
               GetTweenType();
               isAdd = true;
            isSetData = false;
        }
    }
    private void AllTweenAnimation()
    {
        int x = 0;
        int y = 0;
        int w = 0;
        int h = 0;

        for (int i = 0; i < tweenAnimationInfos.Count; i++)
        {
            x = 5;
            y = 2 * (i + 1) + 30 * i;
            w = 370;
            h = 30;
            if (!isAdd)
            {
                if (DrawButton(new Rect(x, y, w, h), tweenAnimationInfos[i].tweenTypeData.ToString()))
                {
                    tweenAnimationInfo = tweenAnimationInfos[i];
                    isSetData = true;
                }
            }
            else
            {
                DrawBox(new Rect(x, y, w, h), tweenAnimationInfos[i].tweenTypeData.ToString());
            }
        }
        rectGroup.height = y + 30;
    }


    private static void GetTweenType()
    {
        tweenTypes.Clear();
        foreach (TweenType  tweenType in Enum.GetValues(typeof (TweenType)))
        {
            if(tweenAnimationInfos.Find(p=>p.tweenTypeData==tweenType)==null)
             tweenTypes.Add(tweenType);
        }
    }
    private void AddTweenAnimationInfo()
    {
        Group(new Rect(100, 200, 400, 200), () =>
        {
            DrawBox(new Rect(0, 0, 400, 200), "", 0, FontStyle.Normal, TextAnchor.MiddleCenter);
            DrawLabel(new Rect(5, 0, 390, 30), "添加曲线动画类型", 20, FontStyle.Bold, TextAnchor.MiddleCenter);

            Group(new Rect(5, 30, 400, 170), () =>
            {
                if (addRectGroup.height >= 170)
                {
                    DrawVerticalSlider(new Rect(375, 5, 12, 160), ref addVerticalSliderValue, 1, 0);
                    addRectGroup.y = -(addRectGroup.height - 160) * addVerticalSliderValue;
                }
                else
                {
                    addRectGroup.y = 0;
                }
                Group(addRectGroup, () =>
                {
                    AllTweenType();
                });
            });

            if (DrawButton(new Rect(370, 0, 30, 30), "X", 16, FontStyle.Bold, TextAnchor.LowerLeft))
            {
                isAdd = false;
            }
        });
    }
    private void AllTweenType()
    {
        int x = 0;
        int y = 0;
        int w = 0;
        int h = 0;

        for (int i = 0; i < tweenTypes.Count; i++)
        {
            x = 5;
            y = 5 * (i + 1) + 25 * i;
            w = 360;
            h = 25;
            if (DrawButton(new Rect(x, y, w, h), tweenTypes[i].ToString()))
            {
                tweenAnimationInfo = new TweenAnimationInfo() { tweenTypeData = tweenTypes[i], animationCurve = new AnimationCurve() };
                tweenAnimationInfos.Add(tweenAnimationInfo);
                isAdd = false;
            }
        }
        addRectGroup.height = y + 25;
    }

    private void SetTweenAnimationData()
    {
        Group(new Rect(100, 200, 400, 200), () =>
        {
            DrawBox(new Rect(0, 0, 400, 200), "", 0, FontStyle.Normal, TextAnchor.MiddleCenter);
            DrawLabel(new Rect(5, 0, 390, 30), "编辑曲线动画数据", 20, FontStyle.Bold, TextAnchor.MiddleCenter);

            Group(new Rect(5, 30, 400, 170), () =>
            {
                DrawLabel(new Rect(10, 10, 290, 30), tweenAnimationInfo.tweenTypeData.ToString(), 20, FontStyle.Bold, TextAnchor.MiddleLeft);
                DrawLabel(new Rect(10, 55, 80, 30), "曲线:", 20, FontStyle.Bold, TextAnchor.MiddleLeft);
                tweenAnimationInfo.animationCurve = EditorGUI.CurveField(new Rect(100, 55, 290, 30), tweenAnimationInfo.animationCurve);
            });

            if (DrawButton(new Rect(300, 160, 80, 30), "保存", 16, FontStyle.Bold, TextAnchor.LowerLeft))
            {
                isSetData = false;
            }
            if (DrawButton(new Rect(200, 160, 80, 30), "删除", 16, FontStyle.Bold, TextAnchor.LowerLeft))
            {
                isSetData = false;
                tweenAnimationInfos.Remove(tweenAnimationInfo);
            }
        });
    }

    //绘制Box
    private static void DrawBox(Rect rect, string text, int fontSize = 16, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        GUI.skin.box.fixedWidth = rect.width;
        GUI.skin.box.fixedHeight = rect.height;
        GUI.skin.box.fontSize = fontSize;
        GUI.skin.box.fontStyle = fontStyle;
        GUI.skin.box.alignment = alignment;

        GUI.Box(rect, text);
    }

    //绘制Ladel
    private static void DrawLabel(Rect rect, string text, int fontSize = 16, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        GUI.skin.label.fontSize = fontSize;
        GUI.skin.label.fontStyle = fontStyle;
        GUI.skin.label.alignment = alignment;
        GUI.Label(rect, text);
    }

    //绘制Button
    private static bool DrawButton(Rect rect, string text, int fontSize = 16, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        GUI.skin.button.fixedWidth = rect.width;
        GUI.skin.button.fixedHeight = rect.height;
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.button.fontStyle = fontStyle;
        GUI.skin.button.alignment = alignment;
        return GUI.Button(rect, text);
    }

    //绘制TextField
    private static string DrawTextField(Rect rect, ref string textField, int fontSize = 16, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        GUI.skin.textField.fixedWidth = rect.width;
        GUI.skin.textField.fixedHeight = rect.height;
        GUI.skin.textField.fontSize = fontSize;
        GUI.skin.textField.fontStyle = fontStyle;
        GUI.skin.textField.alignment = alignment;
        return EditorGUI.TextArea(rect, textField, GUI.skin.textField);
    }


    //绘制Button
    private static float DrawVerticalSlider(Rect rect, ref float sliderValue, int maxValue = 10, int minValue = 0, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleCenter)
    {
        GUI.skin.verticalSlider.fixedWidth = rect.width;
        GUI.skin.verticalSlider.fixedHeight = rect.height;
        GUI.skin.verticalSlider.fontStyle = fontStyle;
        GUI.skin.verticalScrollbar.alignment = alignment;
        sliderValue = GUI.VerticalSlider(rect, sliderValue, minValue, maxValue);
        return verticalSliderValue;
    }

    private static void Clip(Rect rect, System.Action action)
    {
        GUI.BeginClip(rect);
        action?.Invoke();
        GUI.EndClip();
    }
    private static void Group(Rect rect, System.Action action)
    {
        GUI.BeginGroup(rect);
        action?.Invoke();
        GUI.EndGroup();
    }
}