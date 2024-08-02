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

public class DialogBox : MonoBehaviour
{
    [Header("图片")]
    [SerializeField] private Sprite autoSpr1;
    [SerializeField] private Sprite autoSpr2;
    [SerializeField] private Sprite skipSpr1;
    [SerializeField] private Sprite skipSpr2;
    [Header("锚点")]
    [SerializeField] private Transform memoryPlane;
    [SerializeField] private Transform memoryCanvas;
    [SerializeField] private BaseStage stage;
    [Header("UI对话区域")]
    [SerializeField] private Button skipBtn;
    [SerializeField] private Button autoBtn;
    [SerializeField] private Button memoryBtn;
    [SerializeField] private Button exitMemoryBtn;
    [SerializeField] private Button dialogBtn;
    [SerializeField] private Text dialogText;
    [SerializeField] private Text nameText;
    [SerializeField] private Transform optionCanvas;
    [SerializeField] private GameObject btnPre;
    [SerializeField] private GameObject memoryPre;
    [Range(0.01f,0.1f)]
    [SerializeField] private float charSpeed=0.05f;

    private int _index;
    private DialogueGraph m_Dialogue = null;
    private ChatTag chatTag;
    private Node m_Node = null;
    private List<GameObject> m_Btns = new List<GameObject>();
    private Action OnComplete;

    private Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();
    private bool optionFlag;
    //规定一个最小的动画效果

    private bool mIsAuto = false;
    private bool mIsSkip = false;

    public bool IsAuto 
    { 
        get
        {
            return mIsAuto;
        }
        set
        {
            if (mIsSkip)
            {
                IsSkip = false;
            }
            mIsAuto = value;
            autoBtn.GetComponent<Image>().sprite = mIsAuto ? autoSpr1 : autoSpr2;
        }
    }
    public bool IsSkip 
    { 
        get 
        {
            return mIsSkip;
        } 
        set 
        { 
            if (mIsAuto)
            { 
                IsAuto= false;
            }
            mIsSkip = value;
            skipBtn.GetComponent<Image>().sprite = mIsSkip ? skipSpr1: skipSpr2;
        } 
    }
    public bool IsMemory { get; set; } = false;
    public bool IsNext { get; set; } = true;
    private void Start()
    {
        skipBtn.onClick.AddListener(() => IsSkip = !IsSkip);
        autoBtn.onClick.AddListener(() => IsAuto = !IsAuto);
        dialogBtn.onClick.AddListener(Next);
        memoryBtn.onClick.AddListener(ShowMemoryPlane);
        exitMemoryBtn.onClick.AddListener(ExitMemoryPlane);
        charSpeed = GameEntry.Utils.word;
    }

    private void OnDestroy()
    {
        skipBtn.onClick.RemoveAllListeners();
        autoBtn.onClick.RemoveAllListeners();
        dialogBtn.onClick.RemoveAllListeners();
        memoryBtn.onClick.RemoveAllListeners();
        exitMemoryBtn.onClick.RemoveAllListeners();
    }

    private float _time = 0f;
    public float SkipSpeed { get; set; } = 0.05f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            IsSkip = true;
        if(Input.GetKeyUp(KeyCode.LeftControl))
            IsSkip= false;
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
        if (IsAuto)
        {
            if (_time > SkipSpeed+1f)
            {
                Next();
                _time = 0;
            }
        }
    }
    public void ShowButtons(List<OptionData> options)
    {
        if (optionFlag) 
            return;
        optionFlag = true;
        ClearButtons();
        foreach (OptionData option in options)
        {
            GameObject go = GameObject.Instantiate(btnPre, optionCanvas);
            go.GetComponent<OptionItem>().OnInit(option, Option_Onclick);
            m_Btns.Add(go);
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
    public void Next()
    {
        if (IsMemory)
            return;
        if (IsNext == false)
            return;
        if (m_Node == null)
            return;
        switch (chatTag)
        {
            case ChatTag.Start:
                StartNode startNode = (StartNode)m_Node;
                Next(startNode);
                break;
            case ChatTag.Chat:
                ChatNode chatNode = (ChatNode)m_Node;
                Next(chatNode);
                break;
            case ChatTag.Option:
                OptionNode optionNode = (OptionNode)m_Node;
                ShowButtons(optionNode.optionDatas);
                break;
        }
    }
    private void Next(StartNode startNode)
    {
        if (!NextNode(startNode, "start"))
        {
            if (OnComplete != null)
                OnComplete();
            OnComplete = null;
        }
    }
    private bool flag=false;
    private void Next(ChatNode chatNode)
    {
        if (_index < chatNode.chatDatas.Count)
        {
            ChatData chatData = chatNode.chatDatas[_index];
            if (flag)
            {
                dialogText.DOKill();
                dialogText.text = chatData.text;
                flag = false;
                _index++;
                return;
            }
            //角色控制

            stage.ShowCharacter(chatData);
            stage.SetBackground(chatData.background);

            nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;
            dialogText.DOPause();
            dialogText.text = string.Empty;
            if (IsSkip)
            {
                dialogText.text = chatData.text;
                _index++;
            }
            else
            {
                flag = true;
                dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true).OnComplete(() =>
                {
                    flag = false;
                    _index++;
                });
            }

            SkipSpeed = charSpeed * chatData.text.Length;

            AddMemoryItem(chatData);

            for (int i = 0; i < chatData.eventDatas.Count; i++)
            {
                GameEntry.Utils.RunEvent(chatData.eventDatas[i]);
            }
            //角色控制
        }
        else
        {
            if (NextNode(chatNode, string.Format("chatDatas {0}", _index-1)))
                return;
            //播放完毕
            nameText.text = string.Empty;
            dialogText.text = string.Empty;
            _index = 0;
            m_Dialogue = null;
            m_Node = null;
            if (OnComplete != null)
                OnComplete();
            OnComplete = null;
        }
    }
    private bool NextNode(Node node, string nodeName)
    {
        //如果没有中途跳转
        if (node.GetPort(nodeName) != null)
        {
            NodePort nodePort = node.GetPort(nodeName);
            if (nodePort.Connection != null)
            {
                Node nextNode = nodePort.Connection.node;
                switch (nextNode.GetType().ToString())
                {
                    case "ChatNode":
                        m_Node = nextNode;
                        chatTag = ChatTag.Chat;
                        break;
                    case "OptionNode":
                        m_Node = nextNode;
                        chatTag = ChatTag.Option;
                        break;
                    case "TriggerNode":
                        m_Node = nextNode;
                        chatTag = ChatTag.Trigger;
                        break;
                }
                _index = 0;
                Next();
                return true;
            }
        }
        return false;
    }
    public void SetDialog(ChatNode chatNode, Action action)
    {
        OnComplete = null;
        SetComplete(action);
        SetDialog(chatNode);
    }
    public void SetDialog(DialogueGraph dialogueGraph, Action action)
    {
        OnComplete = null;
        SetComplete(action);
        SetDialog(dialogueGraph);
    }
    public void SetDialog(ChatNode chatNode)
    {
        IsSkip = false;
        OnComplete = null;
        _index = 0;
        m_Node = chatNode;
        chatTag = ChatTag.Chat;
        Next();
    }
    public void SetDialog(DialogueGraph graph)
    {
        IsSkip = false;
        OnComplete = null;
        m_Dialogue = graph;
        _index = 0;
        foreach (Node node in m_Dialogue.nodes)
        {
            if (node.GetType().ToString() == "StartNode")
            {
                m_Node = node;
                chatTag = ChatTag.Start;
            }
        }
        Next();
    }
    public void SetComplete(Action action)
    { 
        OnComplete=action;
    }
    private void Option_Onclick(object sender, EventArgs e)
    {
        OptionData optionData = (OptionData)sender;
        if (optionData == null)
            return;
        optionFlag = false;
        ClearButtons();
        foreach (EventData eventData in optionData.eventDatas)
        {
            GameEntry.Utils.RunEvent(eventData);
        }
        OptionNode optionNode = (OptionNode)m_Node;
        NextNode(m_Node, string.Format("optionDatas {0}", optionNode.optionDatas.IndexOf(optionData)));
        AddMemoryItem(optionNode.optionDatas, optionData);
    }

    private void AddMemoryItem(ChatData chatData)
    {
        GameObject go = Instantiate(memoryPre, memoryCanvas);
        MemoryItem memoryItem=go.GetComponent<MemoryItem>();
        memoryItem.SetData(chatData);
    }

    private void AddMemoryItem(List<OptionData> optionDatas,OptionData index)
    {
        GameObject go = Instantiate(memoryPre, memoryCanvas);
        MemoryItem memoryItem = go.GetComponent<MemoryItem>();
        memoryItem.SetData(optionDatas,index);
    }

    private void ShowMemoryPlane()
    {
        IsMemory = true;
        memoryPlane.gameObject.SetActive(true);
        memoryPlane.transform.localPosition = Vector3.down * 1080f;
        memoryPlane.transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.OutExpo);
    }

    private void ExitMemoryPlane()
    {
        IsMemory = false;
        memoryPlane.transform.localPosition = Vector3.zero;
        memoryPlane.transform.DOLocalMoveY(-1080f, 0.5f).SetEase(Ease.OutExpo).OnComplete(()=>
        {
            memoryPlane.gameObject.SetActive(false);
        });
    }
}
