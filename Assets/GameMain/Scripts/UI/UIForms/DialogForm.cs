using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using UnityEngine.EventSystems;
using GameMain;
using DG.Tweening;

namespace GameMain
{
    public class DialogForm : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text dialogText;
        [SerializeField] private Button dialogBtn;
        [SerializeField] private Image character;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private GameObject mBtnPrefab;

        private ActionState mActionState;
        private ActionNode mActionNode;
        private CharData mCharData;
        private int _index;
        private DialogueGraph m_Dialogue = null;
        private ChatTag chatTag;
        private Node m_Node = null;
        private List<GameObject> m_Btns = new List<GameObject>();
        private bool hasQuestion = false;
        private MainState mMainState;
        // Start is called before the first frame update
        // Update is called once per frame
        public void ShowButtons(List<OptionData> options)
        {
            ClearButtons();
            foreach (OptionData option in options)
            {
                GameObject go = GameObject.Instantiate(mBtnPrefab, mCanvas);
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
            if (startNode.GetOutputPort("start") != null)
            {
                var node = startNode.GetOutputPort("start").Connection.node;
                switch (node.GetType().ToString())
                {
                    case "ChatNode":
                        m_Node = node;
                        chatTag = ChatTag.Chat;
                        break;
                    case "OptionNode":
                        m_Node = node;
                        chatTag = ChatTag.Option;
                        break;
                    case "TriggerNode":
                        m_Node = node;
                        chatTag = ChatTag.Trigger;
                        break;
                }
                Next();
            }
        }
        private void Next(ChatNode chatNode)
        {
            if (_index < chatNode.chatDatas.Count)
            {
                ChatData chatData = chatNode.chatDatas[_index];
                nameText.text = chatData.charName;
                dialogText.text = chatData.text;
                if (chatData.charSO != null)
                {
                    character.sprite = chatData.charSO.charData.diffs[(int)chatData.actionData.diffTag];
                    character.color = Color.white;
                    if (chatData.sound != SoundTag.None)
                    {
                        GameEntry.Sound.PlaySound(chatData.charSO.charData.audios[(int)chatData.actionData.soundTag]);
                    }
                }
                if (chatData.actionData.actionTag!=ActionTag.None)
                {
                    switch (chatData.actionData.actionTag)
                    {
                        case ActionTag.Jump:
                            Jump();
                            //跳动效果
                            break;
                        case ActionTag.Shake:
                            Shake();
                            //抖动效果
                            break;
                        case ActionTag.Squat:
                            Squat();
                            break;
                    }
                }
                if (chatNode.GetPort(string.Format("chatDatas {0}", _index))!=null)
                {
                    NodePort nodePort = chatNode.GetPort(string.Format("chatDatas {0}", _index));
                    if (nodePort.Connection != null)
                    {
                        Node node = nodePort.Connection.node;
                        switch (node.GetType().ToString())
                        {
                            case "ChatNode":
                                m_Node = node;
                                chatTag = ChatTag.Chat;
                                break;
                            case "OptionNode":
                                m_Node = node;
                                chatTag = ChatTag.Option;
                                break;
                            case "TriggerNode":
                                m_Node = node;
                                chatTag = ChatTag.Trigger;
                                break;
                        }
                        _index = 0;
                    }
                }
                _index++;
            }
            else
            {
                //播放完毕
                nameText.text = string.Empty;
                dialogText.text = string.Empty;
                _index= 0;
                m_Dialogue = null;
                m_Node = null;
                if (mMainState == MainState.Foreword || mMainState == MainState.Text)
                {
                    character.color = Color.clear;
                    GameEntry.Event.FireNow(this, DialogEventArgs.Create(""));
                }
                else if (mMainState == MainState.Game)
                {
                    mActionState = ActionState.Idle;
                }
                //这不是一个好的通信方式，因为事件最好是自己做了什么被监听
            }
        }
        private void Next(OptionData optionData)
        {
            if (optionData == null)
                return;
            ClearButtons();
            OptionNode optionNode = (OptionNode)m_Node;
            if (optionNode.GetPort(string.Format("optionDatas {0}", optionData.index)) != null)
            {
                NodePort nodePort = optionNode.GetPort(string.Format("optionDatas {0}", optionData.index));
                if (nodePort.Connection != null)
                {
                    Node node = nodePort.Connection.node;
                    switch (node.GetType().ToString())
                    {
                        case "ChatNode":
                            m_Node = node;
                            chatTag = ChatTag.Chat;
                            break;
                        case "OptionNode":
                            m_Node = node;
                            chatTag = ChatTag.Option;
                            break;
                        case "TriggerNode":
                            m_Node = node;
                            chatTag = ChatTag.Trigger;
                            break;
                    }
                    _index = 0;
                    Next();
                }
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
                                break;
                            case EventTag.AddFavor:
                                break;
                        }
                    }
                }
            }
            NextNode(triggerNode, output);
        }
        private void NextNode(TriggerNode node, string nodeName)
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
        public void SetDialog(ChatNode chatNode)
        {
            _index = 0;
            m_Node=chatNode;
            chatTag = ChatTag.Chat;
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
            Next();
        }
        public void SetDialog(string path)
        {
            m_Dialogue = (DialogueGraph)Resources.Load<DialogueGraph>(string.Format("DialogData/{0}", path));
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

        private void SetDialog(object sender, GameEventArgs e)
        {
            LevelEventArgs args = (LevelEventArgs)e;
            mMainState = args.MainState;
            switch (mMainState)
            {
                case MainState.Foreword:
                    SetDialog(args.LevelData.Foreword);
                    break;
                case MainState.Game:
                    SetAction(args.LevelData.ActionGraph);
                    break;
                case MainState.Text:
                    SetDialog(args.LevelData.Text);
                    break;
            }
        }
        private void SetAction(string actionPath)
        {
            ActionGraph actionGraph = (ActionGraph)Resources.Load<ActionGraph>(string.Format("ActionData/{0}", actionPath));
            SetAction(actionGraph);
        }
        private void SetAction(ActionGraph action)
        {
            Debug.Log(action.nodes.Count);
            foreach (Node node in action.nodes)
            {
                if (node.GetType().ToString() == "ActionNode")
                {
                    this.mActionNode = (ActionNode)node;
                    chatTag = ChatTag.Start;
                }
            }
            this.mCharData = action.charSO.charData;
            this.character.sprite = action.charSO.charData.diffs[(int)DiffTag.Idle];
            this.character.color = Color.white;

            List<ChatNode> chatNodes= new List<ChatNode>();
            for (int i = 0; i < mActionNode.idle.Count; i++)
            {
                if (GameEntry.Utils.Check(mActionNode.idle[i]))
                {
                    if (mActionNode.GetPort(string.Format("idle {0}", i)) != null)
                    {
                        NodePort nodePort = mActionNode.GetPort(string.Format("idle {0}", i));
                        if (nodePort.Connection != null)
                        {
                            ChatNode node = (ChatNode)nodePort.Connection.node;
                            chatNodes.Add(node);
                        }
                    }
                }
            }
            if (chatNodes.Count > 0)
            {
                ChatNode chatNode = chatNodes[UnityEngine.Random.Range(0, chatNodes.Count)];
                SetDialog(chatNode);
                mActionState = ActionState.Idle;
            }
            else
            {
                Debug.LogWarningFormat("错误，不存在有效的对话文件，请检查文件以及条件，错误文件：{0}", mActionNode.name);
            }
        }
        public void Click_Action()
        {
            if (mActionNode == null)
                return;
            if (mActionNode.click != null)
            {
                List<ChatNode> chatNodes = new List<ChatNode>();
                for (int i = 0; i < mActionNode.click.Count; i++)
                {
                    if (GameEntry.Utils.Check(mActionNode.click[i]))
                    {
                        if (mActionNode.GetPort(string.Format("click {0}", i)) != null)
                        {
                            NodePort nodePort = mActionNode.GetPort(string.Format("click {0}", i));
                            if (nodePort.Connection != null)
                            {
                                ChatNode node = (ChatNode)nodePort.Connection.node;
                                chatNodes.Add(node);
                            }
                        }
                    }
                }
                if (chatNodes.Count > 0)
                {
                    ChatNode chatNode = chatNodes[UnityEngine.Random.Range(0, chatNodes.Count)];
                    SetDialog(chatNode);
                    mActionState = ActionState.Click;
                }
                else
                {
                    Debug.LogWarningFormat("错误，不存在有效的对话文件，请检查文件以及条件，错误文件：{0}", mActionNode.name);
                }
            }
        }
        private void Coffee_Action()
        {
            if (mActionNode == null)
                return;
            if (mActionNode.coffee != null)
            {
                List<ChatNode> chatNodes = new List<ChatNode>();
                for (int i = 0; i < mActionNode.coffee.Count; i++)
                {
                    if (GameEntry.Utils.Check(mActionNode.coffee[i]))
                    {
                        if (mActionNode.GetPort(string.Format("coffee {0}", i)) != null)
                        {
                            NodePort nodePort = mActionNode.GetPort(string.Format("coffee {0}", i));
                            if (nodePort.Connection != null)
                            {
                                ChatNode node = (ChatNode)nodePort.Connection.node;
                                chatNodes.Add(node);
                            }
                        }
                    }
                }
                if (chatNodes.Count > 0)
                {
                    ChatNode chatNode = chatNodes[UnityEngine.Random.Range(0, chatNodes.Count)];
                    SetDialog(chatNode);
                    mActionState = ActionState.Coffee;
                }
            }
        }
        /// <summary>
        /// 上下跳动
        /// </summary>
        private void Jump()
        {
            character.rectTransform.DOPunchPosition(new Vector3(0, 100, 0),0.4f);
        }
        /// <summary>
        /// 左右抖动
        /// </summary>
        private void Shake()
        {
            character.rectTransform.DOShakePosition(0.4f, new Vector3(50, 0, 0));
        }
        /// <summary>
        /// 下蹲
        /// </summary>
        private void Squat()
        {
            character.rectTransform.DOPunchPosition(new Vector3(0, -100, 0),0.4f);
        }
        private void Start()
        {
            dialogBtn.onClick.AddListener(Next);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, SetDialog);
        }
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                Next();
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return))
                Next();
        }
        //Action相关功能的代码
        private void Update()
        {
            if (mMainState != MainState.Game)
                return;
            switch (mActionState)
            {
                case ActionState.Idle:
                    Coffee_Action();
                    break;
                case ActionState.Click:
                    break;
                case ActionState.Coffee:
                    break;
            }
        }
        private void OnDestroy()
        {
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, SetDialog);
        }

        private void Option_Onclick(object sender,EventArgs e)
        {
            OptionData optionData = (OptionData)sender;
            Next(optionData);
        }
    }
}
