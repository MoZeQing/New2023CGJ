using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using XNode.Examples.LogicToy;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private BaseStage stage;
    [Header("UI对话区域")]
    [SerializeField] private Button dialogBtn;
    [SerializeField] private Text dialogText;
    [SerializeField] private Text nameText;
    [SerializeField] private Transform optionCanvas;
    [SerializeField] private GameObject btnPre;

    private int _index;
    private DialogueGraph m_Dialogue = null;
    private ChatTag chatTag;
    private Node m_Node = null;
    private List<GameObject> m_Btns = new List<GameObject>();
    private Action OnComplete;
    private Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();

    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dialogText.text = string.Empty;
            nameText.text = string.Empty;
        }
    }
    public void ShowButtons(List<OptionData> options)
    {
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
        NextNode(startNode, "start");
    }
    private void Next(ChatNode chatNode)
    {
        if (_index < chatNode.chatDatas.Count)
        {
            //角色控制
            ChatData chatData = chatNode.chatDatas[_index];

            stage.ShowCharacter(chatData);

            nameText.text = chatData.charName;
            dialogText.text = chatData.text;

            //角色控制
            NextNode(chatNode, string.Format("chatDatas {0}", _index));
            _index++;
        }
        else
        {
            //播放完毕
            nameText.text = string.Empty;
            dialogText.text = string.Empty;
            _index = 0;
            m_Dialogue = null;
            m_Node = null;
            stage.gameObject.SetActive(false);
            OnComplete();
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
                    switch (eventData.eventTag)
                    {
                        case EventTag.Play:
                            if (eventData.value == string.Empty)
                                output = string.Format("triggerDatas {0}", i);
                            break;
                        case EventTag.AddMoney:
                            GameEntry.Utils.Money += int.Parse(eventData.value);
                            break;
                        case EventTag.AddFavor:
                            GameEntry.Utils.Favor += int.Parse(eventData.value);
                            break;
                        case EventTag.AddFlag:
                            GameEntry.Utils.AddFlag(eventData.value);
                            break;
                        case EventTag.RemoveFlag:
                            GameEntry.Utils.RemoveFlag(eventData.value);
                            break;
                    }
                }
            }
        }
        NextNode(triggerNode, output);
    }

    private void NextNode(Node node, string nodeName)
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
            }
        }
    }

    public void SetDialog(ChatNode chatNode, Action action)
    {
        SetComplete(action);
        SetDialog(chatNode);
    }
    public void SetDialog(DialogueGraph dialogueGraph, Action action)
    {
        SetComplete(action);
        SetDialog(dialogueGraph);
    }
    public void SetDialog(ChatNode chatNode)
    {
        _index = 0;
        m_Node = chatNode;
        chatTag = ChatTag.Chat;
        stage.gameObject.SetActive(true);
        Next();
    }
    public void SetDialog(DialogueGraph graph)
    {
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
        stage.gameObject.SetActive(true);
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
        ClearButtons();
        OptionNode optionNode = (OptionNode)m_Node;
        NextNode(m_Node, string.Format("optionDatas {0}", optionData.index));
    }
}
