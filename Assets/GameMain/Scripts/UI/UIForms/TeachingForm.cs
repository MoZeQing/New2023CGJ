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
using System;
using static UnityEngine.GraphicsBuffer;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [Header("左侧信息栏")]
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text APText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text moodText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text loveText;
        [SerializeField] private Text familyText;
        [SerializeField] private Text timeText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Button cleanBtn;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button bathBtn;
        [SerializeField] private Button tvBtn;
        [SerializeField] private Button touchBtn;
        [SerializeField] private Button restBtn;
        [SerializeField] private Button sleepBtn;
        [Header("主控")]
        [SerializeField] private Transform mainCanvas;
        [SerializeField] private GameObject energyTips;
        [SerializeField] private GameObject apTips;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private LittleCat mLittleCat = null;
        [SerializeField] private RectTransform mCanvas = null;

        [SerializeField] private Button sleepBtn2;

        private DialogForm mDialogForm = null;
        [SerializeField] public ActionGraph mActionGraph = null;
        private ActionNode mActionNode = null;
        private BehaviorTag mBehaviorTag;
        private ProcedureMain mProcedureMain = null;
        private bool InDialog;
        //Dialog区域
        private List<GameObject> m_Btns = new List<GameObject>();

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
            GameEntry.Utils.UpdateData();

            cleanBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Clean));
            touchBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Touch));
            playBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Play));
            sleepBtn.onClick.AddListener(Sleep);
            bathBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Bath));
            restBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Rest));
            tvBtn.onClick.AddListener(() => Behaviour(BehaviorTag.TV));
            sleepBtn2.onClick.AddListener(Sleep);

            this.transform.localScale = Vector3.one * 0.01f;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(1)&&!InDialog)
            {
                GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
                dialogBox.gameObject.SetActive(false);
                stage.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(false);
                rightCanvas.gameObject.SetActive(false);
                apTips.gameObject.SetActive(false);
                energyTips.gameObject.SetActive(false);
                mLittleCat.ShowLittleCat();
            }
        }
        private void OnDisable()
        {
            mLittleCat.ShowLittleCat();
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);

            touchBtn.onClick.RemoveAllListeners();
            playBtn.onClick.RemoveAllListeners();
            sleepBtn.onClick.RemoveAllListeners();
            cleanBtn.onClick.RemoveAllListeners();
            tvBtn.onClick.RemoveAllListeners();
            restBtn.onClick.RemoveAllListeners();
            bathBtn.onClick.RemoveAllListeners();
        }
        public void Behaviour(BehaviorTag behaviorTag)
        {
            mActionNode = mActionGraph.ActionNode();
            mBehaviorTag = behaviorTag;
            List<ParentTrigger> triggers = new List<ParentTrigger>();
            PlayerData playerData = new PlayerData();
            CharData charData = new CharData();
            switch (behaviorTag)
            {
                case BehaviorTag.Click:
                    triggers = mActionNode.Click;
                    playerData = mActionNode.ClickData;
                    charData = mActionNode.ClickCharData;
                    break;
                case BehaviorTag.Clean:
                    triggers = mActionNode.Clean;
                    playerData= mActionNode.CleanData;
                    charData= mActionNode.CleanCharData;
                    break;
                case BehaviorTag.Bath:
                    triggers = mActionNode.Bath;
                    playerData= mActionNode.BathData;
                    charData= mActionNode.BathCharData;
                    break;
                case BehaviorTag.Rest:
                    triggers = mActionNode.Rest;
                    playerData= mActionNode.RestData;
                    charData= mActionNode.RestCharData;
                    break;
                case BehaviorTag.TV:
                    triggers = mActionNode.TV;
                    playerData= mActionNode.TVData;
                    charData= mActionNode.TVCharData;
                    break;
                case BehaviorTag.Talk:
                    triggers = mActionNode.Talk;
                    playerData = mActionNode.TalkData;
                    charData= mActionNode.TalkCharData;
                    break;
                case BehaviorTag.Touch:
                    triggers = mActionNode.Touch;
                    playerData = mActionNode.TouchData;
                    charData= mActionNode.TouchCharData;
                    break;
                case BehaviorTag.Play:
                    triggers = mActionNode.Play;
                    playerData = mActionNode.PlayData;
                    charData= mActionNode.PlayCharData;
                    break;
                case BehaviorTag.Sleep:
                    triggers = mActionNode.Sleep;
                    charData= mActionNode.SleepCharData;
                    break;
                case BehaviorTag.Morning:
                    triggers = mActionNode.Morning;
                    charData=mActionNode.MorningCharData;
                    break;
            }
            if (behaviorTag != BehaviorTag.Sleep)
            {
                if (GameEntry.Utils.Energy < playerData.energy)
                {
                    energyTips.gameObject.SetActive(true);
                    return;
                }
                energyTips.gameObject.SetActive(false);
                if (GameEntry.Utils.Ap < playerData.ap)
                {
                    apTips.gameObject.SetActive(true);
                    return;
                }
                apTips.gameObject.SetActive(false);
                GameEntry.Utils.Energy -= playerData.energy;
                GameEntry.Utils.Money -= playerData.money;
                GameEntry.Utils.MaxEnergy -= playerData.maxEnergy;
                GameEntry.Utils.Ap -= playerData.ap;
                GameEntry.Utils.MaxAp -= playerData.maxAp;

                GameEntry.Utils.Mood += charData.mood;
                GameEntry.Utils.Favor += charData.favor;
                GameEntry.Utils.Hope += charData.hope;
                GameEntry.Utils.Love+= charData.love;
                GameEntry.Utils.Family += charData.family;
            }
            else
            {
                GameEntry.Utils.Energy += 60;
                GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            }

            List<ChatNode> chatNodes = new List<ChatNode>();
            for (int i = 0; i < triggers.Count; i++)
            {
                if (GameEntry.Utils.Check(triggers[i]))
                {
                    if (mActionNode.GetPort(string.Format("{0} {1}", behaviorTag.ToString(), i)) != null)
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
                dialogBox.gameObject.SetActive(true);
                dialogBox.SetDialog(chatNode);
                InDialog = true;
                if (mBehaviorTag == BehaviorTag.Sleep)
                    dialogBox.SetComplete(OnSleep);//回调
                else if (mBehaviorTag == BehaviorTag.Morning)
                    dialogBox.SetComplete(OnGameChangeState);
                else
                    dialogBox.SetComplete(OnComplete);
                leftCanvas.gameObject.SetActive(false);
                rightCanvas.gameObject.SetActive(false);
                stage.gameObject.SetActive(true);
                mLittleCat.HideLittleCat();
            }
            else
            {
                Debug.LogWarningFormat("错误，不存在合法的对话剧情，请检查{0}的{1}", mActionNode.name, behaviorTag.ToString());
            }
        }
        private void Sleep()
        {
            InDialog = false;
            GameEntry.Utils.TimeTag = TimeTag.Night;
            if (!GameEntry.Dialog.StoryUpdate())
                Behaviour(BehaviorTag.Sleep);
            else
                OnGameChangeState();
        }
        private void OnComplete()
        {
            InDialog=false;
            dialogBox.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            rightCanvas.gameObject.SetActive(true);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
        }
        private void OnSleep()
        {
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
            InDialog = false;
            GameEntry.Utils.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, GameEntry.Utils.Day);//用这个this传参来调整黑幕
            mLittleCat.HideLittleCat();
            stage.gameObject.SetActive(false);
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
            //播放一个睡觉效果
            Invoke(nameof(OnChangeDay), 1f);
        }
        private void OnChangeDay()
        {
            InDialog = false;
            GameEntry.Utils.TimeTag = TimeTag.Morning;
            if (!GameEntry.Dialog.StoryUpdate())
                Behaviour(BehaviorTag.Morning);
            else
                OnGameChangeState();
        }
        private void OnGameChangeState()
        {
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Work));
        }
        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = string.Format("好感:{0}", charData.favor.ToString());
            moodText.text= string.Format("心情:{0}", charData.mood.ToString());
            loveText.text = string.Format("爱情:{0}", charData.love.ToString());
            familyText.text = string.Format("亲情:{0}", charData.family.ToString());
        }
        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            APText.text = string.Format("行动点：{0}/{1}", playerData.ap, playerData.maxAp);
            energyText.text = string.Format("体力：{0}/{1}", playerData.energy, playerData.maxEnergy);
            moneyText.text=string.Format("金钱:{0}", playerData.money.ToString());
            timeText.text = string.Format("{0}月{1}日 星期{2}", 5 + (playerData.day + 20) / 28, (playerData.day + 20) % 28, AssetUtility.GetWeekCN((playerData.day + 20) % 7));
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Lock));
            mLittleCat.ShowLittleCat();
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
            Behaviour(BehaviorTag.Click);
        }
    }
}