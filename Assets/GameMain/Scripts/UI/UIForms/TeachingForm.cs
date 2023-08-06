using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using XNode;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [Header("左侧信息栏")]
        public Transform leftCanvas;
        public Text moneyText;
        public Text favourText;
        public Text APText;
        public Text moodText;
        [Header("右侧操作栏")]
        public Transform rightCanvas;
        public Button touchBtn;
        public Button talkBtn;
        public Button playBtn;
        public Button storyBtn;
        public Button sleepBtn;
        //[Header("中间操作区域")]
        //public Transform middleText;
        //public Text nameText;
        //public Text dialogText;

        public CharSO charSO;
        private Cat mCat = null;
        private ActionGraph mActionGraph = null;
        private ActionNode mActionNode = null;
        private SceneTag sceneTag;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //mActionGraph = (ActionGraph)userData;
            //GameEntry.Event.Subscribe(MainFormEventArgs.EventId, MainEvent);

            touchBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Touch));
            talkBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Talk));
            playBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Play));
            storyBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Story));
            sleepBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Sleep));

            Vector3 pos = Vector3.zero;
            switch (sceneTag)
            {
                case SceneTag.Working:
                    pos = Vector3.zero;
                    break;
            }
            //显示猫猫
            GameEntry.Entity.ShowCat(new CatData(GameEntry.Entity.GenerateSerialId(), 10008, charSO)
            {
                Position = pos
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(1))
            { 
                //按下鼠标右键关闭界面
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            //GameEntry.Event.Unsubscribe(MainFormEventArgs.EventId, MainEvent);
        }
        private void Behaviour(BehaviorTag behaviorTag)
        {
            List<Trigger> triggers = new List<Trigger>();
            switch (behaviorTag)
            {
                case BehaviorTag.Click:
                    triggers = mActionNode.click;
                    break;
            }
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
                GameEntry.UI.OpenUIForm(UIFormId.DialogForm, chatNode);
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

            List<ChatNode> chatNodes = new List<ChatNode>();
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
                }
            }
        }
        //private void MainEvent(object sender, GameEventArgs args)
        //{
        //    MainFormEventArgs mainFormEvent = (MainFormEventArgs)args;
        //    MainFormTag tag = mainFormEvent.MainFormTag;
        //    switch (tag)
        //    {
        //        case MainFormTag.Up:
        //            this.transform.DOLocalMove(new Vector3(1920f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
        //            break;
        //        case MainFormTag.Down:
        //            this.transform.DOLocalMove(new Vector3(1920, 800f, 0f), 1f).SetEase(Ease.OutExpo);
        //            break;
        //        case MainFormTag.Left:
        //            this.transform.DOLocalMove(new Vector3(3840f, 800f, 0f), 1f).SetEase(Ease.OutExpo);
        //            break;
        //        case MainFormTag.Right:
        //            this.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
        //            break;
        //    }
        //}
    }
}