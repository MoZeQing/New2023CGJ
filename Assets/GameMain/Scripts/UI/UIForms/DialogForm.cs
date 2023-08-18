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
using System.Runtime.InteropServices;

namespace GameMain
{
    public class DialogForm : UIFormLogic
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text dialogText;
        [SerializeField] private Button dialogBtn;
        [SerializeField] private Image character;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private GameObject mBtnPrefab;

        private int _index;
        private DialogueGraph m_Dialogue = null;
        private ChatTag chatTag;
        private Node m_Node = null;
        private List<GameObject> m_Btns = new List<GameObject>();
        private bool hasQuestion = false;
        private MainState mMainState;
        private DialogStage mDialogStage = null;
        private int mChace = 0;
        // Start is called before the first frame update
        // Update is called once per frame

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dialogBtn.onClick.AddListener(Next);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, SetDialog);
            GameEntry.Event.Subscribe(GamePosEventArgs.EventId, GamePosEvent);

            DialogueGraph dialogueGraph = (DialogueGraph)userData;
            SetDialog(dialogueGraph);
            mChace = GameEntry.Entity.GenerateSerialId();

            GameEntry.Entity.ShowDialogStage(new DialogStageData(mChace, 10010)
            { 
                
            });
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (Input.GetKey(KeyCode.LeftControl))
                Next();
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                Next();
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, SetDialog);
            GameEntry.Event.Unsubscribe(GamePosEventArgs.EventId, GamePosEvent);
        }
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
                //角色控制
                ChatData chatData = chatNode.chatDatas[_index];
                nameText.text = chatData.charName;
                dialogText.text = chatData.text;
                mDialogStage.ShowCharacter(chatData);
                //角色控制
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
                //if (mMainState == MainState.Foreword || mMainState == MainState.Text)
                //{
                //    character.color = Color.clear;
                //    GameEntry.Event.FireNow(this, DialogEventArgs.Create(""));
                //}
                //else if (mMainState == MainState.Game)
                //{
                //}

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
        
        private void Option_Onclick(object sender,EventArgs e)
        {
            OptionData optionData = (OptionData)sender;
            Next(optionData);
        }
        private void GamePosEvent(object sender, GameEventArgs args)
        {
            GamePosEventArgs gamePos = (GamePosEventArgs)args;
            switch (gamePos.GamePos)
            {
                case GamePos.Up:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f);
                    break;
                case GamePos.Down:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 1920f, 0f), 1f);
                    break;
                case GamePos.Left:
                    mCanvas.transform.DOLocalMove(new Vector3(1920f, 0, 0f), 1f);
                    break;
            }
        }
    }
}
