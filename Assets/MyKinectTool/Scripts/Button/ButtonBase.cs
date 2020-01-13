using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTFrame.MTKinect
{
    public class ButtonBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerClickHandler,IPointerUpHandler
    {
        public bool isChoice;
        public bool isOn=true;
        [Header("是否点击")]
        public bool isClick;
        [Header("是否进入")]
        public bool isEnter;
        [Header("点击时间")]
        public float clickTime;

        public Color OnChoiceColor = Color.white;
        public Color OffChoiceColor = Color.white;
        public Color ClickColor=Color.white;
        public Color UpColor = Color.white;


        private Color[] currentColors;

        private MaskableGraphic[] maskableGraphics=new MaskableGraphic[]{ };
        
        public UnityEvent OnClick;
        public UnityEvent OnChoice;
        public UnityEvent OffChoice;
        public UnityEvent OnUp;
        public UnityEvent OnDown;
        

        public void Start()
        {
            maskableGraphics = GetComponentsInChildren<MaskableGraphic>();
            currentColors = new Color[maskableGraphics.Length];
            for (int i = 0; i < maskableGraphics.Length; i++)
            {
                currentColors[i] = maskableGraphics[i].color;
            }
            //Init();
        }

        public virtual  void OnPointerDown(PointerEventData eventData)
        {
            if (!isOn)
                return;
            isClick = true;
            isChoice = !isChoice;
            OnDown.Invoke();

            for (int i = 0; i < maskableGraphics.Length; i++)
            {
                if (!(maskableGraphics[i] as Text))
                maskableGraphics[i].color = (currentColors[i]+ ClickColor)/2;
            }
           
        }
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!isOn)
                return;
            isEnter = true;
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!isOn)
                return;
            isEnter = false;
            isClick = false;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isOn)
                return;
            OnClick.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isOn)
                return;
            isClick = false;
            OnUp.Invoke();
            SetChoice();
        }

        public void SetChoice()
        {
            if (isChoice)
            {
                OnChoice.Invoke();
                for (int i = 0; i < maskableGraphics.Length; i++)
                {
                    if (!(maskableGraphics[i] as Text))
                        maskableGraphics[i].color =( currentColors[i] + OnChoiceColor)/2 ;
                }
            }
            else
            {
                OffChoice.Invoke();
                for (int i = 0; i < maskableGraphics.Length; i++)
                {
                    if (!(maskableGraphics[i] as Text))
                        maskableGraphics[i].color = (currentColors[i]+ OffChoiceColor) / 2;
                }
            }
        }
        //初始化按钮
        public void Init(bool _isChoice=false)
        {
            isChoice = _isChoice;
            if (isChoice)
            {
                for (int i = 0; i < maskableGraphics.Length; i++)
                {
                    maskableGraphics[i].color = (currentColors[i] + OnChoiceColor) / 2;
                }
            }
            else
            {
                for (int i = 0; i < maskableGraphics.Length; i++)
                {
                    if (!(maskableGraphics[i] as Text))
                        maskableGraphics[i].color = (currentColors[i] + OffChoiceColor) / 2;
                }
            }
        }

        public void InitEvent()
        {
            Init();
            OnClick.RemoveAllListeners();
            OnChoice.RemoveAllListeners();
            OffChoice.RemoveAllListeners();
            OnUp.RemoveAllListeners();
            OnDown.RemoveAllListeners();
        }
    }    
}        
         
         