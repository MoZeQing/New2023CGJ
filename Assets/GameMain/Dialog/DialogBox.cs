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
    [SerializeField] protected Image chatIconImg;
    [SerializeField] protected Transform optionCanvas;
    [SerializeField] protected GameObject btnPre;
    [Range(0.1f, 0.5f)]
    [SerializeField] protected float charSpeed = 0.05f;



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
    public float SkipSpeed { get; set; } = 5f;
    public float autoPlayDelay = 1f; // 自动播放结束时的延迟
    public bool isAutoPlay;
    public bool isSkipbtn;
    protected bool isTextComplete = false; // 标识当前对话文本是否播放完毕
    private float autoPlayTimer = 0f; // 自动播放的计时器

    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            IsSkip = !IsSkip;
        chatIconImg.GetComponent<Animator>().SetBool("Skipping", IsSkip);
        if (IsSkip)
        {
            uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipUsing;
        }
        else
        {
            uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipNormal;
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

        //try
        //{
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
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError($"Error occurred in Next(): {ex.Message}\nBaseData identifier: {m_Data.Id}\nStack Trace: {ex.StackTrace}");
        //}
        isTextComplete = false;
    }

    virtual public void CompleteDialog()
    {
        ClearButtons();
        IsSkip = false;
        isAutoPlay = false;
        nameText.text = string.Empty;
        dialogText.text = string.Empty;
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
            IsSkip = false;
            GameObject go = Instantiate(btnPre, optionCanvas);
            go.GetComponent<OptionItem>().OnInit(optionData, Option_Onclick);
            m_Btns.Add(go);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing option: {ex.Message}\nOptionData identifier: {optionData.Id}");
        }
    }

    protected virtual void ShowChat(ChatData chatData)
    {
        try
        {
            DisplayChat(chatData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing chat: {ex.Message}\nChatData identifier: {chatData.Id}");
        }
    }

    protected void DisplayChat(ChatData chatData)
    {
        stage.ShowCharacter(chatData);
        nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;

        if (IsSkip)
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.text = chatData.text;
            m_Data = chatData;
            isTextComplete = true; // 文本直接显示完毕
            chatIconImg.gameObject.SetActive(true);
            return;
        }
        if (DOTween.IsTweening(dialogText))
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.text = chatData.text;
            m_Data = chatData;
            isTextComplete = true; // 文本直接显示完毕
            chatIconImg.gameObject.SetActive(true);
        }
        else
        {
            dialogText.DOKill();
            chatIconImg.gameObject.SetActive(false);
            dialogText.text = string.Empty;
            dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true)
                .OnComplete(() =>
                {
                    chatIconImg.gameObject.SetActive(true);
                    m_Data = chatData;
                    isTextComplete = true; // 文本播放完毕，设置标识
                });
        }
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
        IsSkip = !IsSkip;
    }
}
