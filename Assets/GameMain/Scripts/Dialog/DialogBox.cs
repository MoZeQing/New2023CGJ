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
    [SerializeField] private BaseStage stage;
    [Header("UI对话区域")]
    [SerializeField] private Button dialogBtn;
    [SerializeField] private Text dialogText;
    [SerializeField] private Text nameText;
    [SerializeField] private Transform optionCanvas;
    [SerializeField] private GameObject btnPre;
    [Range(0.01f,0.1f)]
    [SerializeField] private float charSpeed=0.05f;

    private int _index;
    private DialogueGraph m_Dialogue = null;
    private ChatTag chatTag;
    private Node m_Node = null;
    private List<GameObject> m_Btns = new List<GameObject>();
    private Action OnComplete;
    private bool mIsSkip;
    private Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();
    private bool optionFlag;
    //规定一个最小的动画效果

    public bool IsNext { get; set; } = true;
    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }

    private float _time = 0f;
    public float SkipSpeed { get; set; } = 0.05f;

    private void Update()
    {
        mIsSkip = Input.GetKey(KeyCode.LeftControl);
        _time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Next();
        if (mIsSkip)
        {
            if (_time > SkipSpeed)
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
            case ChatTag.Trigger:
                TriggerNode triggerNode = (TriggerNode)m_Node;
                Next(triggerNode);
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
    private void Next(ChatNode chatNode)
    {
        if (_index < chatNode.chatDatas.Count)
        {
            //角色控制
            ChatData chatData = chatNode.chatDatas[_index];

            stage.ShowCharacter(chatData);
            stage.SetBackground(chatData.background);

            nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;
            dialogText.DOPause();
            dialogText.text = string.Empty;
            dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true);
            SkipSpeed = charSpeed * (chatData.text.Length)+0.1f;

            for (int i = 0; i < chatData.eventDatas.Count; i++)
            {
                GameEntry.Utils.RunEvent(chatData.eventDatas[i]);
            }
            //角色控制
            _index++;
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
    private void Next(TriggerNode triggerNode)
    {
        if (triggerNode == null)
            return;
        string output = "b";
        for (int i = 0; i < triggerNode.triggerDatas.Count; i++)
        {
            TriggerData data = triggerNode.triggerDatas[i];
            if (GameEntry.Utils.Check(data.trigger))
            {
                foreach (EventData eventData in data.events)
                {
                    if (eventData.eventTag == EventTag.Play)
                    {
                        if (eventData.value1 == string.Empty)
                            output = string.Format("triggerDatas {0}", i);
                    }
                    GameEntry.Utils.RunEvent(eventData);
                }
            }
        }
        if (!NextNode(triggerNode, output))
        {
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
        mIsSkip = false;
        OnComplete = null;
        _index = 0;
        m_Node = chatNode;
        chatTag = ChatTag.Chat;
        Next();
    }
    public void SetDialog(DialogueGraph graph)
    {
        mIsSkip = false;
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
        NextNode(m_Node, string.Format("optionDatas {0}", optionData.index));
    }
}
