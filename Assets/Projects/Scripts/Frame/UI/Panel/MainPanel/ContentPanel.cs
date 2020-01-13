using MTFrame;
using MTFrame.MTKinect;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ContentPanel : BasePanel
{
    public static ContentPanel Instance;

    public Text Title;

    public Transform L, R;

    public Transform ButtonGroup, ContentGroup;

    public MenuButton UP, Down;

    public MenuButton contentButton;

    public ContentControl[] contentControls;
    public List<MenuButton> menuButtons = new List<MenuButton>();


    private int index = 0;
    private DirectoryPathData directoryPathData;
    private List<FileInfo> fileInfos = new List<FileInfo>();
    private List<Texture2D> texture2Ds = new List<Texture2D>();

//______________________________________________________________________________//

    //新添加
    public CanvasGroup NobelMetalCanvas;
    public Image IntroduceImage;
    public List<Sprite> IntroduceSprites = new List<Sprite>();
    private string[] Cur_NobleMetal_Name = { "36克心莲银镯", "标准金条", "标准银条" };
    public Animator JiXiangWuAnimator;
    public List<RawImage> ContentRawImage = new List<RawImage>();

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    public override void InitFind()
    {
        base.InitFind();
        Title = FindTool.FindChildComponent<Text>(transform, "Title");

        L = FindTool.FindChildComponent<Transform>(transform, "BG_Group/L");
        R = FindTool.FindChildComponent<Transform>(transform, "BG_Group/R");

        ButtonGroup = FindTool.FindChildComponent<Transform>(transform, "BG_Group/BG/ButtonGroup/ButtonMask/ButtonGroup");
        ContentGroup = FindTool.FindChildComponent<Transform>(transform, "BG_Group/BG/Mask/ContentGroup");

        UP = FindTool.FindChildComponent<MenuButton>(transform, "BG_Group/BG/ButtonGroup/UP");
        Down = FindTool.FindChildComponent<MenuButton>(transform, "BG_Group/BG/ButtonGroup/Down");

        contentControls = GetComponentsInChildren<ContentControl>();

        NobelMetalCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "NobelMetalUI");
        IntroduceImage = NobelMetalCanvas.transform.GetChild(0).GetComponent<Image>();
        JiXiangWuAnimator = NobelMetalCanvas.GetComponentInChildren<Animator>();
        for (int i = 0; i < contentControls.Length; i++)
        {
            RawImage rawImage = contentControls[i].gameObject.transform.GetChild(1).GetComponent<RawImage>();
            ContentRawImage.Add(rawImage);
        }
    }

    public override void InitEvent()
    {
        base.InitEvent();

        UP.OnClick += UpSwitchButton;
        Down.OnClick += DownSwitchButton;

    }

    int buttonIndex;
    private void UpSwitchButton(BaseButton obj)
    {
        SwitchButton(-1);
    }
    private void DownSwitchButton(BaseButton obj)
    {
        SwitchButton(1);
    }

    private void SwitchButton(int v)
    {
        buttonIndex += v;
        if (buttonIndex < 0)
            buttonIndex = 0;
        if (buttonIndex > menuButtons.Count - 3)
            buttonIndex = menuButtons.Count - 3;
        foreach (var item in menuButtons)
        {
            item.Close();
        }

        ButtonGroup.DOLocalMoveY(((100 + 20) * buttonIndex)+(100 + 20)*1.5f, 0.5f).OnComplete(()=>
        {
            for (int i = buttonIndex; i < buttonIndex + 3; i++)
            {
                menuButtons[i].Open();
            }
        });
    }


    public override void Open()
    {
        base.Open();
        NobelMetalCanvas.alpha = 0;
        transform.DOLocalMoveX(0, openTweenTime);
        UP.Open();
        Down.Open();
        foreach (Transform item in ContentGroup)
        {
           
            item.gameObject.SetActive(true);
        }
        PlayerManager.Instance.onPlayerGestureEvent += OnPlayerGesture;

    }

    public override void Hide()
    {
        base.Hide();
        NobleMetalControl.Instance.Hide();
        transform.DOLocalMoveX(-1920, hideTweenTime);
        UP.Close();
        Down.Close();

        for (int i = 0; i < contentControls.Length; i++)
        {
            contentControls[i].Close ();
        }
        for (int i = 0; i < menuButtons.Count; i++)
        {
            GameObject.Destroy(menuButtons[i].gameObject);
        }
        menuButtons.Clear();
        PlayerManager.Instance.onPlayerGestureEvent -= OnPlayerGesture;

        buttonIndex = 0;
        ButtonGroup.DOLocalMoveY((0) + (100 + 20) * 1.5f, 0.5f);
    }


    private void OnPlayerGesture(long userid, string gesture)
    {
        if (userid == PlayerManager.Instance.GetPrimaryPlay().UserID)
        {
            switch (gesture)
            {
                case "右挥手":
                    ContentGroup.DOLocalMoveX(-1100, 0.5f).Ease(TweenType.EaselnOutBack).OnComplete(() =>
                    {
                        for (int i = 0; i < ContentRawImage.Count; i++)
                        {
                            ContentRawImage[i].enabled = false;
                        }
                        ContentGroup.localPosition = Vector3.zero;
                        SetContent(1);
                    });
                    break;
                case "左挥手":
                    ContentGroup.DOLocalMoveX(1100, 0.5f).Ease(TweenType.EaselnOutBack).OnComplete(() =>
                    {
                        for (int i = 0; i < ContentRawImage.Count; i++)
                        {
                            ContentRawImage[i].enabled = false;
                        }
                        ContentGroup.localPosition = Vector3.zero;
                        SetContent(-1);
                    });
                    break;
            }
        }
    }

    private void SetContent(int v)
    {
        index += v;
        if (index < 0)
            index = fileInfos.Count - 1;
        else if (index >= fileInfos.Count)
            index = 0;

        texture2Ds.Clear();
        foreach (var item in contentControls)
        {
            texture2Ds.Add(item.texture2D);
        }

        if (fileInfos.Count == 1)
        {
            for (int i = 0; i < contentControls.Length; i++)
            {

                contentControls[i].SetContent(fileInfos[index]);
                Debug.Log(fileInfos[index]);
            }
            return;
        }

        for (int i = 0; i < contentControls.Length; i++)
        {
            int _index = index + i;
            if (_index >= fileInfos.Count)
                _index = 0 + (_index - fileInfos.Count);
            if (i == 1)
            {
                contentControls[i].SetContent(fileInfos[_index], texture2Ds[i + v]);
            }
            else
            {
                contentControls[i].SetContent(fileInfos[_index]);
            }
            //Debug.Log(fileInfos[_index]);
        }

    }

    public void SetPanel(string _name)
    {
        //Debug.Log("1111");
        index = 0;
        Open();
        directoryPathData = MainData.Instance.directoryPathDatas.Find(p => p.directoryInfo.Name == _name);
        //Debug.Log("directoryPathData:" + directoryPathData.ToString());
        SetButton();
    }

    private void SetButton()
    {
        //Debug.Log("2222"+ directoryPathData.filePathData[0].fileInfos[0]);
       
        FileInfo fileInfo = directoryPathData.filePathData[0].fileInfos[0];
        Title.text = directoryPathData.filePathData[0].directoryInfo.Name;
        fileInfos = directoryPathData.filePathData[0].fileInfos;
        Debug.Log("文件个数:" + fileInfos.Count);
        Debug.Log(directoryPathData.filePathData[0].directoryInfo.Name);
        if(directoryPathData.filePathData[0].directoryInfo.Name.Contains("贵金属展示"))
        {
            Debug.Log("第一个文件夹是贵金属展示");
            foreach (Transform item in ContentGroup)
            {
                item.gameObject.SetActive(false);
            }
            NobleMetalControl.Instance.Open();
        }
        else
        {
            SetContent(-1);
        }
        

        int v = 0;
        foreach (var item in directoryPathData.filePathData)
        {
            MenuButton menuButton = GameObject.Instantiate(contentButton, ButtonGroup);
            menuButtons.Add(menuButton);
            menuButton.Init(item.directoryInfo.Name);
            menuButton.OnClick += OnClick;
            if (v > 2)
            {
                menuButton.Close();
            }

            v++;
        }

        if (v > 2)
        {
            UP.gameObject.SetActive(true);
            Down.gameObject.SetActive(true);
        }
        else
        {
            UP.gameObject.SetActive(false);
            Down.gameObject.SetActive(false);
        }
    }

    private void OnClick(BaseButton obj)
    {
        FilePathData filePathData = directoryPathData.filePathData.Find(p => p.directoryInfo.Name == (obj as MenuButton).GetName());
        Title.text = filePathData.directoryInfo.Name;
        if (filePathData.directoryInfo.Name.Contains("贵金属展示"))
        {
            foreach (Transform item in ContentGroup)
            {
                item.gameObject.SetActive(false);
            }
            NobleMetalControl.Instance.Open();
            //Debug.Log("显示模型");
        }
        else
        {
            //Debug.Log("按钮点击，子物体显示");
            foreach (Transform item in ContentGroup)
            {
                item.gameObject.SetActive(true);
            }
            NobleMetalControl.Instance.Hide();
            if (filePathData != null)
                fileInfos = filePathData.fileInfos;
            index = 0;
            SetContent(-1);
        }

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UP.OnClick -= UpSwitchButton;
        Down.OnClick -= DownSwitchButton;
    }

    public void SwitchIntroduceImageAndTiltle(int i)
    {
        if(IntroduceSprites[i])
        {
            Title.text = Cur_NobleMetal_Name[i];
            IntroduceImage.sprite = IntroduceSprites[i];
        }
        else
        {
            Debug.LogError("图片资源丢失");
        }

    }
}