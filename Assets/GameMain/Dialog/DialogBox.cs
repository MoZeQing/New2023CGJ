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
    [Header("UI¶Ô»°ÇøÓò")]
    [SerializeField] protected Button dialogBtn;
    [SerializeField] protected Text dialogText;
    [SerializeField] protected Text nameText;
    [SerializeField] protected Transform optionCanvas;
    [SerializeField] protected GameObject btnPre;
    [Range(0.01f,0.5f)]
    [SerializeField] protected float charSpeed=0.05f;

    protected DialogData m_DialogData = null;
    protected BaseData m_Data = null;

    protected ChatTag mChatTag;
    protected int mIndex;

    protected bool optionFlag;
    protected List<GameObject> m_Btns = new List<GameObject>();

    protected Action OnComplete;

    public bool IsSkip { get; set; } = false;
    public bool IsNext { get; set; } = true;
    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }

    protected float _time = 0f;
    public float SkipSpeed { get; set; } = 0.05f;

    private void Update()
    {
        IsSkip = Input.GetKey(KeyCode.LeftControl);
        _time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Next();
        if (IsSkip)
        {
            if (_time > SkipSpeed)
            {
                Next();
                _time = 0;
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
    protected virtual void Next()
    {
        if (optionFlag)
            return;
        if (IsNext == false)
            return;
        if (m_Data == null)
            return;
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
                }
            }
        }
        else
        {
            ClearButtons();
            nameText.text = string.Empty;
            dialogText.text = string.Empty;
            mIndex = 0;
            m_DialogData = null;
            m_Data = null;
            if (OnComplete != null)
                OnComplete();
            OnComplete = null;
        }
    }
    protected virtual void InitDailog(StartData startData)
    {
        Next();
    }
    protected virtual void ShowOption(OptionData optionData)
    {
        optionFlag = true;
        GameObject go = GameObject.Instantiate(btnPre, optionCanvas);
        go.GetComponent<OptionItem>().OnInit(optionData, Option_Onclick);
        m_Btns.Add(go);
    }
    protected virtual void ShowChat(ChatData chatData)
    {
        stage.ShowCharacter(chatData);
        stage.SetBackground(chatData.background);
        nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;

        if (DOTween.IsTweening(dialogText))
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.text = chatData.text;
            m_Data = chatData;
        }
        else
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true).OnComplete(() => m_Data = chatData);
            SkipSpeed = charSpeed * (chatData.text.Length) + 0.1f;
        }

        GameEntry.Utils.RunEvent(chatData.eventData);
    }
    public virtual void SetDialog(DialogueGraph dialogueGraph)
    {
        XNodeSerializeHelper helper = new XNodeSerializeHelper();
        DialogData dialogData = new DialogData(helper, dialogueGraph);
        SetDialog(dialogData);
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
    public virtual void SetComplete(Action action)
    { 
        OnComplete=action;
    }
}
