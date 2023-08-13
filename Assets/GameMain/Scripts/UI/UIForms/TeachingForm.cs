using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using XNode;
using UnityEditor.UI;
using UnityEditor.SceneManagement;
using static UnityEditor.ShaderData;
using System.Reflection;
using System;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [SerializeField] private GameObject mBtnPrefab;
        [SerializeField] private Transform mCanvas;
        [Header("左侧信息栏")]
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Text timeText;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text APText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text moodText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Button talkBtn;
        [SerializeField] private Button touchBtn;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button storyBtn;
        [SerializeField] private Button sleepBtn;
        [Header("对话区域")]
        [SerializeField] private Transform middleCanvas;
        [SerializeField] private Button dialogBtn;
        [SerializeField] private Text dialogText;
        [SerializeField] private Text nameText;
        [SerializeField] private Transform option;
        [Header("主控")]
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button loadBtn;
        [SerializeField] private GameObject energyObj;
        [SerializeField] private GameObject apObj;

        public CharSO charSO;
        private LittleCat mLittleCat = null;
        private Cat mCat = null;
        private DialogForm mDialogForm = null;
        [SerializeField] private ActionGraph mActionGraph = null;
        private ActionNode mActionNode = null;
        private BehaviorTag mBehaviorTag;
        //Dialog区域
        private int _index;
        private DialogueGraph m_Dialogue = null;
        private ChatTag chatTag;
        private Node m_Node = null;
        private List<GameObject> m_Btns = new List<GameObject>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dialogBtn.onClick.AddListener(Next);

            //mActionGraph = (ActionGraph)userData;
            mActionNode = mActionGraph.ActionNode();
            //GameEntry.Event.Subscribe(MainFormEventArgs.EventId, MainEvent);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, GetCat);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Subscribe(GamePosEventArgs.EventId, GamePosEvent);
            GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);

            talkBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Talk));
            playBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Play));
            storyBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Story));
            sleepBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Sleep));

            mCanvas.transform.localPosition = new Vector3(0f, 0f, 0f);

            //初始化角色
            GameEntry.Entity.ShowLittleCat(new LittleCharData(GameEntry.Entity.GenerateSerialId(), 10009)
            {
                Position = new Vector3(0f, 4.6f),
                TeachingForm = this
            });
            GameEntry.Entity.ShowCat(new LittleCharData(GameEntry.Entity.GenerateSerialId(), 10008)
            {
                Position = new Vector3(0f, 4.6f),
                TeachingForm = this
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(1))
            {
                HideGUI();
                dialogText.text = string.Empty;
                nameText.text = string.Empty;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            //GameEntry.Event.Unsubscribe(MainFormEventArgs.EventId, MainEvent);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, GetCat);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Unsubscribe(GamePosEventArgs.EventId, GamePosEvent);
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
        }

        public void ShowGUI()
        {
            mLittleCat.HideLittleCat();
            mCat.ShowCat();
            rightCanvas.gameObject.SetActive(true);
            leftCanvas.gameObject.SetActive(true);
            middleCanvas.gameObject.SetActive(false);
            apObj.gameObject.SetActive(false);
            energyObj.gameObject.SetActive(false);
        }

        public void HideGUI()
        {
            mLittleCat.ShowLittleCat();
            mCat.HideCat();
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            middleCanvas.gameObject.SetActive(false);
            apObj.gameObject.SetActive(false);
            energyObj.gameObject.SetActive(false);
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            Behaviour(BehaviorTag.Click);
        }
        public void Behaviour(BehaviorTag behaviorTag)
        {
            mBehaviorTag = behaviorTag;
            List<Trigger> triggers = new List<Trigger>();
            PlayerData playerData = new PlayerData();
            switch (behaviorTag)
            {
                case BehaviorTag.Click:
                    triggers = mActionNode.Click;
                    playerData = mActionNode.ClickData;
                    break;
                case BehaviorTag.Talk:
                    triggers = mActionNode.Talk;
                    playerData = mActionNode.TalkData;
                    break;
                case BehaviorTag.Touch:
                    triggers = mActionNode.Touch;
                    break;
                case BehaviorTag.Play:
                    triggers = mActionNode.Play;
                    break;
                case BehaviorTag.Sleep:
                    triggers = mActionNode.Sleep;
                    break;
            }
            //判断            
            if (GameEntry.Utils.Energy < playerData.energy)
            {
                energyObj.gameObject.SetActive(true);
                return;
            }
            energyObj.gameObject.SetActive(false);
            if (GameEntry.Utils.Ap < playerData.ap)
            {
                apObj.gameObject.SetActive(true);
                return;//播放错误界面
            }
            apObj.gameObject.SetActive(false);
            //结算
            GameEntry.Utils.Energy-=playerData.energy;
            GameEntry.Utils.Money-=playerData.money;
            GameEntry.Utils.MaxEnergy-=playerData.maxEnergy;
            GameEntry.Utils.Ap-=playerData.ap;
            GameEntry.Utils.MaxAp-=playerData.maxAp;

            GameEntry.Utils.Mood += 20;
            GameEntry.Utils.Favor += 2;
            GameEntry.Utils.Hope+=1;
            //剧情

            //检测正确性
            List<ChatNode> chatNodes = new List<ChatNode>();
            for (int i = 0; i < triggers.Count; i++)
            {
                if (GameEntry.Utils.Check(triggers[i]))
                {
                    if (mActionNode.GetPort(string.Format("{0} {1}",behaviorTag.ToString(), i)) != null)
                    {
                        NodePort nodePort = mActionNode.GetPort(string.Format("{0} {1}", behaviorTag.ToString(), i));
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
                leftCanvas.gameObject.SetActive(false);
                rightCanvas.gameObject.SetActive(false);
                middleCanvas.gameObject.SetActive(true);
                mCat.ShowCat();
                mLittleCat.HideLittleCat();
            }
            else
            {
                Debug.LogWarningFormat("错误，不存在有效的对话文件，请检查文件以及条件，错误文件：{0}", mActionNode.name);
            }

        }
        private void SetAction(string actionPath)
        {
            ActionGraph actionGraph = (ActionGraph)Resources.Load<ActionGraph>(string.Format("ActionData/{0}", actionPath));
            SetAction(actionGraph);
        }
        private void SetAction(ActionGraph action)
        {
            foreach (Node node in action.nodes)
            {
                if (node.GetType().ToString() == "ActionNode")
                {
                    this.mActionNode = (ActionNode)node;
                }
            }
        }
        /// <summary>
        /// 回调获取对话界面角色（固定单角色）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GetCat(object sender, GameEventArgs args)
        {
            ShowEntitySuccessEventArgs showEntity = (ShowEntitySuccessEventArgs)args;
            LittleCat littleCat = null;
            if (showEntity.Entity.TryGetComponent<LittleCat>(out littleCat))
            { 
                mLittleCat= littleCat;
            }
            Cat cat = null;
            if (showEntity.Entity.TryGetComponent<Cat>(out cat))
            {
                mCat = cat;
            }
            if (mCat != null && mLittleCat != null)
            {
                HideGUI();
            }
        }
        private void LevelEvent(object sender, GameEventArgs args)
        {
            LevelEventArgs level = (LevelEventArgs)args;
            switch (level.MainState)
            {
                case MainState.Teach:
                    break;
                case MainState.Dialog:
                    break;
            }
        }

        private void GamePosEvent(object sender, GameEventArgs args)
        { 
            GamePosEventArgs gamePos= (GamePosEventArgs)args;
            switch (gamePos.GamePos)
            {
                case GamePos.Up:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Down:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 1920f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Left:
                    mCanvas.transform.DOLocalMove(new Vector3(1920f, 0, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
            }
        }
        //Dialog区域
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
                if (chatData.charSO != null)
                {

                }
                if (chatData.actionData.actionTag != ActionTag.None)
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
                //角色控制
                if (chatNode.GetPort(string.Format("chatDatas {0}", _index)) != null)
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
                _index = 0;
                m_Dialogue = null;
                m_Node = null;
                ShowGUI();
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
            m_Node = chatNode;
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

        /// <summary>
        /// 上下跳动
        /// </summary>
        private void Jump()
        {
            mCat.transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 0.4f);
        }
        /// <summary>
        /// 左右抖动
        /// </summary>
        private void Shake()
        {
            mCat.transform.DOShakePosition(0.4f, new Vector3(0.25f, 0, 0));
        }
        /// <summary>
        /// 下蹲
        /// </summary>
        private void Squat()
        {
            mCat.transform.DOPunchPosition(new Vector3(0, -0.5f, 0), 0.4f);
        }

        private void Option_Onclick(object sender, EventArgs e)
        {
            OptionData optionData = (OptionData)sender;
            Next(optionData);
        }

        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = string.Format("好感:{0}", charData.favor.ToString());
            moodText.text= string.Format("心情:{0}", charData.mood.ToString());
        }

        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            APText.text = string.Format("行动点：{0}/{1}", playerData.ap, playerData.maxAp);
            energyText.text = string.Format("体力：{0}/{1}", playerData.energy, playerData.maxEnergy);
            moneyText.text=string.Format("金钱:{0}", playerData.money.ToString());
        }
    }
}