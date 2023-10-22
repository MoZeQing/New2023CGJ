﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using XNode;
using System;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [Header("左侧信息栏")]
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text APText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text hopeText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text loveText;
        [SerializeField] private Text familyText;
        [SerializeField] private Text timeText;
        [SerializeField] private Text rentText;
        [SerializeField] private Text abilityText;
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
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private LittleCat mLittleCat = null;
        [SerializeField] private RectTransform mCanvas = null;
        [SerializeField] private CanvasGroup mCanvasGroup = null;

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
            sleepBtn.onClick.AddListener(OnSleep);
            bathBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Bath));
            restBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Rest));
            tvBtn.onClick.AddListener(() => Behaviour(BehaviorTag.TV));

            mCanvasGroup = this.GetComponent<CanvasGroup>();
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
                mLittleCat.ShowLittleCat();
            }
            if (mCanvasGroup.alpha < 1)
            {
                mCanvasGroup.alpha = Mathf.Lerp(mCanvasGroup.alpha, 1, 0.3f);
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
                    GameEntry.UI.OpenUIForm(UIFormId.TitleForm, "你没有足够的体力");
                    return;
                }
                if (GameEntry.Utils.Ap < playerData.ap)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.TitleForm, "你没有足够的行动力");
                    return;
                }
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
                GameEntry.Utils.Energy += 40;
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
        //不允许在回调中再设置回调，会导致回调错误
        //也就是说，在SetComplete方法中设置的方法不能有Behaviour等会设置回调的方法
        private void OnSleep()
        {
            InDialog = false;
            GameEntry.Utils.TimeTag = TimeTag.Night;
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
            if (!GameEntry.Dialog.StoryUpdate(PassDay))
                Behaviour(BehaviorTag.Sleep);
        }
        private void PassDay()
        {
            GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            GameEntry.Utils.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
            GameEntry.Utils.TimeTag = TimeTag.Morning;
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Morning));
            Invoke(nameof(OnFaded), 4f);
        }
        private void OnMorning()
        {
            if (!GameEntry.Dialog.StoryUpdate())
                Behaviour(BehaviorTag.Morning);
            else
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
        }
        private void OnComplete()
        {
            InDialog=false;
            if (mBehaviorTag == BehaviorTag.Sleep)
            {
                rightCanvas.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(false);
                GameEntry.Utils.Day++;
                GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
                GameEntry.Utils.TimeTag = TimeTag.Morning;
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Morning));
                Invoke(nameof(OnFaded), 4f);
            }
            else if (mBehaviorTag == BehaviorTag.Morning)
            {
                GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Work));
            }
            else
            {
                dialogBox.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(true);
                rightCanvas.gameObject.SetActive(true);
            }
        }
        private void OnFaded()
        {
            mCanvasGroup.alpha = 0;
            Behaviour(BehaviorTag.Morning);
        }
        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = charData.favor.ToString();
            hopeText.text= charData.hope.ToString();
            loveText.text =  charData.love.ToString();
            familyText.text =  charData.family.ToString();
            abilityText.text=charData.ability.ToString();
        }
        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            rentText.transform.parent.gameObject.SetActive(GameEntry.Utils.Rent != 0);
            rentText.text = string.Format("距离下一次欠款缴纳还有{0}天\r\n下一次交纳欠款：{1}",6-(playerData.day + 20) % 7, GameEntry.Utils.Rent.ToString());
            APText.text = string.Format("{0}/{1}", playerData.ap, playerData.maxAp);
            energyText.text = string.Format("{0}/{1}", playerData.energy, playerData.maxEnergy);
            moneyText.text=string.Format("{0}", playerData.money.ToString());
            timeText.text = string.Format("{0}月{1}日 星期{2}", (4 + (playerData.day + 20) / 28) % 12 + 1, (playerData.day + 19) % 28 + 1, AssetUtility.GetWeekCN((playerData.day + 20) % 7));
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Lock));
            mLittleCat.ShowLittleCat();
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            Behaviour(BehaviorTag.Click);
        }
    }
}