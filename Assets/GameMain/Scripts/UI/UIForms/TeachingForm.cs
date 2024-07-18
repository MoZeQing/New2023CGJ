using System.Collections;
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
        [SerializeField] private Text energyText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text staminaText;
        [SerializeField] private Text wisdomText;
        [SerializeField] private Text charmText;
        [SerializeField] private Text staminaLevelText;
        [SerializeField] private Text wisdomLevelText;
        [SerializeField] private Text charmLevelText;
        [SerializeField] private Text dayText;//日期文本框
        [SerializeField] private Text apText;//行动力文本框
        [SerializeField] private Text weekText;//星期文本框
        [SerializeField] private Text rentText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Transform buttonCanvas;
        [SerializeField] private Transform exitBtn;
        [Header("主控")]
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private CanvasGroup mCanvasGroup = null;

        private DialogForm mDialogForm = null;
        [SerializeField] public ActionGraph mActionGraph = null;
        [SerializeField] private GameObject behaviorBtn;
        private CatStateData mCatStateData = null; 
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
            ShowButtons();
            mCanvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
        }
        //private void Start()
        //{
        //    GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
        //    GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
        //    GameEntry.Utils.UpdateData();
        //    ShowButtons();
        //    mCanvasGroup = this.GetComponent<CanvasGroup>();
        //}
        private void Update()
        {
            moneyText.text = string.Format("{0}", GameEntry.Player.Money.ToString());
            weekText.text = $"星期{AssetUtility.GetWeekCN(GameEntry.Utils.PlayerData.day % 7)}";
            apText.text = $"AP:{GameEntry.Utils.Ap}/{GameEntry.Utils.MaxAp}";
            dayText.text = $"第{GameEntry.Utils.PlayerData.day % 7 + 1}天";
            //if (Input.GetMouseButtonDown(1)&&!InDialog)
            //{
            //    GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
            //    dialogBox.gameObject.SetActive(false);
            //    stage.gameObject.SetActive(false);
            //}
            if (mCanvasGroup.alpha < 1)
            {
                mCanvasGroup.alpha = Mathf.Lerp(mCanvasGroup.alpha, 1, 0.3f);
            }
        }
        private void ShowButtons()
        {
            if (mCatStateData == GameEntry.Cat.GetCatState())
                return;
            mCatStateData = GameEntry.Cat.GetCatState();
            DestroyButtons();
            foreach (BehaviorData behaviorData in mCatStateData.behaviors)
            {
                if (behaviorData.behaviorTag == BehaviorTag.Morning)
                    continue;
                if (behaviorData.behaviorTag == BehaviorTag.Click)
                    continue;
                GameObject go = GameObject.Instantiate(behaviorBtn, buttonCanvas);
                Button button=go.GetComponent<Button>();
                Text text = go.transform.Find("Text").GetComponent<Text>();
                if(behaviorData.behaviorTag==BehaviorTag.Sleep)
                    button.onClick.AddListener(OnSleep);
                else
                    button.onClick.AddListener(() => Behaviour(behaviorData.behaviorTag));
                text.text = behaviorData.behaviorName.ToString();
                m_Btns.Add(go);
            }
        }
        private void DestroyButtons()
        {
            for (int i=0;i<buttonCanvas.childCount;i++)
            {
                Destroy(buttonCanvas.GetChild(i).gameObject);
            }
            m_Btns.Clear();
        }
        public void Behaviour(BehaviorTag behaviorTag)
        {
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Lock));
            mBehaviorTag = behaviorTag;
            BehaviorData behavior = GameEntry.Cat.GetBehavior(behaviorTag);
            if (behaviorTag != BehaviorTag.Sleep)
            {
                if (GameEntry.Utils.Energy < behavior.playerData.energy)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的体力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                if (GameEntry.Utils.Ap < behavior.playerData.ap)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的行动力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                BuffData buffData = GameEntry.Buff.GetBuff();
                GameEntry.Utils.Energy -= (int)Mathf.Clamp((behavior.playerData.energy*buffData.EnergyMulti+buffData.EnergyPlus),0,9999999);
                GameEntry.Utils.Money -= behavior.playerData.money;
                GameEntry.Utils.MaxEnergy -= behavior.playerData.maxEnergy;

                GameEntry.Utils.Favor += (int)(behavior.charData.favor * buffData.FavorMulti + buffData.FavorPlus);
                GameEntry.Event.FireNow(this, BehaviorEventArgs.Create(behaviorTag));
            }
            else
            {
                GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            }
            Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
            switch (behaviorTag)
            {
                case BehaviorTag.Read://智慧
                    behavior.charData.GetValueTag(dic);
                    GameEntry.UI.OpenUIForm(UIFormId.ActionForm3, OnComplete, dic);
                    break;
                case BehaviorTag.Sport://体魄
                    behavior.charData.GetValueTag(dic);
                    GameEntry.UI.OpenUIForm(UIFormId.ActionForm2, OnComplete, dic);
                    break;
                case BehaviorTag.Augur://魅力
                    behavior.charData.GetValueTag(dic);
                    GameEntry.UI.OpenUIForm(UIFormId.ActionForm1, OnComplete, dic);
                    break;
                default:
                    DialogueGraph dialogueGraph = behavior.dialogues[UnityEngine.Random.Range(0, behavior.dialogues.Count)];
                    dialogBox.gameObject.SetActive(true);
                    dialogBox.SetDialog(dialogueGraph);
                    InDialog = true;
                    dialogBox.SetComplete(OnComplete);
                    leftCanvas.gameObject.SetActive(false);
                    rightCanvas.gameObject.SetActive(false);
                    stage.gameObject.SetActive(true);
                    break;
            }
        }
        //不允许在回调中再设置回调，会导致回调错误
        //也就是说，在SetComplete方法中设置的方法不能有Behaviour等会设置回调的方法
        private void PassDay()
        {
            GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            GameEntry.Utils.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
            GameEntry.Utils.GameState = GameState.Morning;
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Morning));
            Invoke(nameof(OnFaded), 4f);
        }
        private void OnSleep()
        {
            InDialog = false;
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Utils.Energy = (int)(GameEntry.Utils.MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus);
            GameEntry.Utils.GameState = GameState.Midnight;
            if (!GameEntry.Dialog.StoryUpdate(PassDay))
                Behaviour(BehaviorTag.Sleep);
        }
        //private void OnMorning()
        //{
        //    if (!GameEntry.Dialog.StoryUpdate())
        //        Behaviour(BehaviorTag.Morning);
        //}
        private void OnComplete()
        {
            InDialog=false;
            if (mBehaviorTag == BehaviorTag.Sleep)
            {
                rightCanvas.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(false);
                GameEntry.Utils.Day++;
                GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
                Invoke(nameof(OnFaded), 4f);
            }
            else if (mBehaviorTag == BehaviorTag.Morning)
            {
                GameEntry.Utils.GameState = GameState.Work;
            }
            else
            {
                BehaviorData behavior = GameEntry.Cat.GetBehavior(mBehaviorTag);
                GameEntry.Utils.Ap -= behavior.playerData.ap;
                GameEntry.Utils.MaxAp -= behavior.playerData.maxAp;
                dialogBox.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(true);
                rightCanvas.gameObject.SetActive(true);
                stage.ShowDiff(DialogPos.Middle, DiffTag.MoRen);
                ShowButtons();
            }
        }
        private void OnFaded()
        {
            mCanvasGroup.alpha = 0;
            GameEntry.Utils.GameState = GameState.Morning;
            mBehaviorTag = BehaviorTag.Morning;
            if (!GameEntry.Dialog.StoryUpdate(OnComplete))
            {
                Behaviour(BehaviorTag.Morning);
            }
        }
        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = charData.favor.ToString();
            staminaText.text = charData.stamina.ToString();
            wisdomText.text = charData.wisdom.ToString();
            charmText.text=charData.charm.ToString();
            staminaLevelText.text = $"Lv.{charData.staminaLevel}";
            wisdomLevelText.text = $"Lv.{charData.wisdomLevel}";
            charmLevelText.text = $"Lv.{charData.charmLevel}";
        }
        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            //rentText.transform.parent.gameObject.SetActive(GameEntry.Utils.Rent != 0);
            //rentText.text = string.Format("距离下一次欠款缴纳还有{0}天\r\n下一次交纳欠款：{1}",6-(playerData.day + 20) % 7, GameEntry.Utils.Rent.ToString());
            BuffData buffData = GameEntry.Buff.GetBuff();
            //energyText.text = string.Format("{0}/{1}", playerData.energy, playerData.maxEnergy*buffData.EnergyMaxMulti+buffData.EnergyMaxPlus);
            moneyText.text=string.Format("{0}", playerData.money.ToString());
            weekText.text = $"星期{AssetUtility.GetWeekCN(playerData.day % 7)}";
            apText.text = $"AP:{GameEntry.Utils.Ap}/{GameEntry.Utils.MaxAp}";
            dayText.text = $"第{playerData.day% 7+1}天";
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            GameEntry.Sound.PlaySound(UnityEngine.Random.Range(24,27));
            Behaviour(BehaviorTag.Click);
        }
    }
}