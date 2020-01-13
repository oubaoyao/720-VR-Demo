using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MTFrame
{
    /// <summary>
    /// UI面板基类
    /// </summary>
    [ExecuteInEditMode]
    public abstract class BasePanel : UIBehavior, ISerializeButton
    {
        public List<string> SerializeButtonName
        {
            get
            {
                return new List<string>()
            {
                "InitFind"
            };
            }
        }

        public List<Action> SerializeButtonMethod
        {
            get
            {
                return new List<Action>()
            {
                InitFind
            };
            }
        }

        /// <summary>
        /// 打开动画时间
        /// </summary>
        public float openTweenTime = 0.5f;
        /// <summary>
        /// 隐藏动画时间
        /// </summary>
        public float hideTweenTime = 0.5f;
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen=true;



        private UIBasePanelTween _currentTween;
        /// <summary>
        /// 当前动画
        /// </summary>
        protected UIBasePanelTween currentTween
        {
            get
            {
                if (_currentTween == null)
                    _currentTween = new GradualTween(this);
                return _currentTween;
            }
            set { _currentTween = value; }
        }
        /// <summary>
        /// Panel动画列表
        /// </summary>
        private List<UIBasePanelTween> tweenList = new List<UIBasePanelTween>();

        /// <summary>
        /// 上一级面板
        /// </summary>
        protected BasePanel lastUIPanel;


        //画布组
        private CanvasGroup canvasGroup;
        /// <summary>
        ///画布组（控制整体可屏蔽图像透明度）
        /// </summary>
        public CanvasGroup CanvasGroup
        {
            get
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                   canvasGroup = gameObject.AddComponent<CanvasGroup>();
                };
                return canvasGroup;
            }
        }
        

        protected override void Awake()
        {
            base.Awake();

            name = GetType().Name;

            InitFind();//初始化查找
            InitEvent();//初始化事件
        }


        /// <summary>
        /// 初始化(注意继承基类的初始化方法需要在重写基类的下面)
        /// ｛
        /// base .Init(***)
        /// ****
        /// ****
        /// ｝
        /// </summary>
        /// <param name="basePanel"></param>
        public virtual void Init(BasePanel basePanel = null)
        {
            lastUIPanel = basePanel;

        }
        /// <summary>
        /// 初始化查找
        /// </summary>
        public virtual void InitFind()
        {
        }
        /// <summary>
        /// 初始化事件
        /// </summary>
        public virtual void InitEvent()
        {

        }

        /// <summary>
        /// 打开
        /// </summary>
        public virtual void Open()
        {
            IsOpen = true;
            currentTween?.Open(openTweenTime);//调用当前打开动画
            OpenTrigger();
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide()
        {
            IsOpen = false;
            currentTween?.Hide(hideTweenTime);//调用当前关闭动画
            CloseTrigger();
        }
        /// <summary>
        /// 开启面板下所有可触发的图片
        /// </summary>
        public virtual void OpenTrigger()
        {
            CanvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// 关闭面板下所有可触发的图片
        /// </summary>
        public virtual void CloseTrigger()
        {
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 设置Panel动画
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ToSetTween<T>() where T : UIBasePanelTween
        {
            if (currentTween != null && !typeof(T).IsAssignableFrom(currentTween.GetType()))
                currentTween.Exit();

            UIBasePanelTween tween = tweenList.Find((o) => typeof(T).IsAssignableFrom(o.GetType()));
            if (tween == null)
            {
                tween = (T)Activator.CreateInstance(typeof(T), new object[] { this });
                tweenList.Add(tween);
            }

            if (currentTween == null ||
                (currentTween != null && tween.GetType().Name != currentTween.GetType().Name))
            {
                currentTween = tween;
                if (!IsOpen)
                    currentTween.Init();
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            UIManager.RemovePanel(this);
        }
    }
}