using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using DG.Tweening;
using XNode.Examples.LogicToy;
using System.Runtime.InteropServices.ComTypes;
using Dialog;

public class DialogBox : MonoBehaviour
{
    [SerializeField] protected BaseStage stage;
    [Header("UI对话区域")]
    [SerializeField] private UIBtnsControl uIBtnsControl;
    [SerializeField] protected Button dialogBtn;
    [SerializeField] protected Text dialogText;
    [SerializeField] protected Text nameText;
    [SerializeField] protected Transform optionCanvas;
    [SerializeField] protected GameObject btnPre;
    [Range(0.1f, 0.5f)]
    [SerializeField] protected float charSpeed = 0.05f;

    // 黑屏和白色文本相关
    [Header("黑屏与白色文本")]
    [SerializeField] private CanvasGroup blackScreenCanvasGroup; // 黑屏 CanvasGroup
    [SerializeField] private Text blackScreenText; // 显示黑屏下的白色文字

    protected DialogData m_DialogData = null;
    protected BaseData m_Data = null;

    protected ChatTag mChatTag;
    protected int mIndex;

    protected bool optionFlag;
    protected List<GameObject> m_Btns = new List<GameObject>();

    protected Action OnComplete;

    public bool IsSkip { get; set; } = false;
    public bool IsNext { get; set; } = true;
    protected float _time = 0f;
    public float SkipSpeed { get; set; } = 20f;
    private bool isBlackScreenActive = false; // 标记当前是否处于黑屏状态
    public float autoPlayDelay = 1f; // 自动播放结束时的延迟
    public bool isAutoPlay;
    public bool isSkipbtn;
    private bool isTextComplete = false; // 标识当前对话文本是否播放完毕
    private float autoPlayTimer = 0f; // 自动播放的计时器

    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }

    private void Update()
    {
        IsSkip = Input.GetKey(KeyCode.LeftControl);
        if(IsSkip)
        {
            uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipUsing;
        }
        else
        {
            if(!uIBtnsControl.isSkip)
            {
                uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipNormal;
            }
        }
        _time += Time.deltaTime;

        // 手动点击或者按下 Space / Return 键进入下一步
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Next();

        // 快速跳过逻辑
        if (IsSkip || isSkipbtn)
        {
            if (_time > (1 / SkipSpeed))
            {
                Next();
                _time = 0;
            }
        }

        // 自动播放逻辑
        if (isAutoPlay && isTextComplete)
        {
            autoPlayTimer += Time.deltaTime;
            if (autoPlayTimer >= autoPlayDelay)
            {
                Next();
                autoPlayTimer = 0f;
                isTextComplete = false; // 重置状态
            }
        }
    }

    public void ClearButtons()
    {
        foreach (GameObject go in m_Btns)
        {
            Destroy(go);
        }
        m_Btns.Clear();
    }

    protected virtual void Option_Onclick(object sender, EventArgs e)
    {
        BaseData optionData = sender as BaseData;
        if (optionData == null)
            return;
        optionFlag = false;
        ClearButtons();
        m_Data = optionData;
        Next();
    }

    public virtual void Next()
    {
        if (optionFlag || !IsNext || m_Data == null)
            return;

        try
        {
            if (m_Data.After.Count != 0)
            {
                foreach (BaseData after in m_Data.After)
                {
                    string dialogTag = after.GetType().Name;
                    switch (dialogTag)
                    {
                        case "StartData":
                            StartData startData = (StartData)after;
                            InitDailog(startData);
                            break;
                        case "ChatData":
                            ChatData chatData = (ChatData)after;
                            ShowChat(chatData);
                            break;
                        case "OptionData":
                            OptionData optionData = (OptionData)after;
                            ShowOption(optionData);
                            break;
                        case "BlackData":
                            BlackData blackData = (BlackData)after;
                            ShowBlackChat(blackData);
                            break;
                        default:
                            Debug.LogWarning($"Unknown dialog tag '{dialogTag}' encountered in Next().");
                            break;
                    }
                }
            }
            else
            {
                CompleteDialog();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error occurred in Next(): {ex.Message}\nBaseData identifier: {m_Data.Identifier}\nStack Trace: {ex.StackTrace}");
        }
        isTextComplete = false;
    }

    virtual public void CompleteDialog()
    {
        ClearButtons();
        nameText.text = string.Empty;
        dialogText.text = string.Empty;
        blackScreenText.text = string.Empty; // 清空黑屏文字
        blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            blackScreenCanvasGroup.gameObject.SetActive(false);
            isBlackScreenActive = false; // 结束黑屏状态
        });
        mIndex = 0;
        m_DialogData = null;
        m_Data = null;
        OnComplete?.Invoke();
        OnComplete = null;
    }

    protected virtual void InitDailog(StartData startData)
    {
        Next();
    }

    protected virtual void ShowOption(OptionData optionData)
    {
        try
        {
            optionFlag = true;
            GameObject go = Instantiate(btnPre, optionCanvas);
            go.GetComponent<OptionItem>().OnInit(optionData, Option_Onclick);
            m_Btns.Add(go);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing option: {ex.Message}\nOptionData identifier: {optionData.Identifier}");
        }
    }

    protected virtual void ShowChat(ChatData chatData)
    {
        try
        {
            if (isBlackScreenActive)
            {
                // 如果当前是黑屏状态，需要退出黑屏
                blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    blackScreenCanvasGroup.gameObject.SetActive(false);
                    isBlackScreenActive = false;
                    DisplayChat(chatData);
                });
            }
            else
            {
                DisplayChat(chatData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing chat: {ex.Message}\nChatData identifier: {chatData.Identifier}");
        }
    }

    private void DisplayChat(ChatData chatData)
    {
        stage.ShowCharacter(chatData);
        nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;

        if (DOTween.IsTweening(dialogText))
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.text = chatData.text;
            m_Data = chatData;
            isTextComplete = true; // 文本直接显示完毕
        }
        else
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true)
                .OnComplete(() =>
                {
                    m_Data = chatData;
                    isTextComplete = true; // 文本播放完毕，设置标识
                });
        }
    }

    protected virtual void ShowBlackChat(BlackData blackData)
    {
        // 清空普通对话框的内容，防止黑屏结束后显示之前的内容
        dialogText.text = string.Empty;
        nameText.text = string.Empty;

        if (!isBlackScreenActive)
        {
            // 初次进入黑屏状态，执行黑屏渐入效果
            blackScreenCanvasGroup.gameObject.SetActive(true);
            blackScreenCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                // 黑屏完全显示后，开始显示文字
                UpdateBlackScreenText(blackData);
            });
            isBlackScreenActive = true;
        }
        else
        {
            // 如果已经处于黑屏状态，直接更新文字
            UpdateBlackScreenText(blackData);
        }

        m_Data = blackData;
    }



    private void UpdateBlackScreenText(BlackData blackData)
    {
        blackScreenText.DOKill(); // 确保没有之前的动画干扰
        blackScreenText.text = string.Empty;

        // 使用 DOText 方法逐字显示文字
        blackScreenText.DOText(blackData.text, charSpeed * blackData.text.Length, true)
            .OnComplete(() =>
            {
                // 文本播放完毕，设置标识
                isTextComplete = true;
            });


    }

    public virtual void SetDialog(DialogData dialogData)
    {
        IsSkip = false;
        OnComplete = null;
        m_DialogData = dialogData;
        m_Data = dialogData.GetStartData();
        mIndex = 0;
        Next();
    }
    public virtual void SetDialog(DialogueGraph dialogueGraph)
    {
        XNodeSerializeHelper helper = new XNodeSerializeHelper();
        DialogData dialogData = helper.Serialize(dialogueGraph);
        SetDialog(dialogData);
    }
    public virtual void SetComplete(Action action)
    {
        OnComplete = action;
    }

    public void ChangeAutoPlay()
    {
        if (isAutoPlay)
        {
            isAutoPlay = false;
        }
        else
        {
            isAutoPlay = true;
        }
    }
    public void ChangeSkipPlay()
    {
        if (isSkipbtn)
        {
            isSkipbtn = false;
        }
        else
        {
            isSkipbtn = true;
        }
    }
}
