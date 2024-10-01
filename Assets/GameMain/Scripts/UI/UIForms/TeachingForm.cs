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
    public class TeachingForm : MonoBehaviour
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
        [SerializeField] private Text rentText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Transform buttonCanvas;
        [SerializeField] private Transform exitBtn;
        [Header("主控")]
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private CanvasGroup mCanvasGroup = null;

        [SerializeField] private GameObject behaviorBtn;
        private CatStateData mCatStateData = null; 
        private BehaviorTag mBehaviorTag;
        public bool InDialog { get; set; }
        //Dialog区域
        private List<GameObject> m_Btns = new List<GameObject>();

        private void Update()
        {
            if (mCanvasGroup.alpha < 1)
            {
                mCanvasGroup.alpha = Mathf.Lerp(mCanvasGroup.alpha, 1, 0.3f);
            }
        }
        public void ShowButtons()
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
                if (behaviorData.behaviorTag == BehaviorTag.Sleep)
                    continue;
                GameObject go = GameObject.Instantiate(behaviorBtn, buttonCanvas);
                Button button=go.GetComponent<Button>();
                Text text = go.transform.Find("Text").GetComponent<Text>();
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
                if (GameEntry.Player.Energy < behavior.valueData.energy)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的体力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                if (GameEntry.Player.Ap < behavior.valueData.ap)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的行动力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                BuffData buffData = GameEntry.Buff.GetBuff();
                GameEntry.Player.Energy -= (int)Mathf.Clamp((behavior.valueData.energy*buffData.EnergyMulti+buffData.EnergyPlus),0,9999999);

                if(behavior.valueData.favor!=0)
                    GameEntry.Cat.Favor += (int)(behavior.valueData.favor * buffData.FavorMulti + buffData.FavorPlus);
                if (behavior.valueData.stamina != 0)
                    GameEntry.Cat.Stamina += (int)(behavior.valueData.stamina  + buffData.StaminaPlus);
                if (behavior.valueData.wisdom != 0)
                    GameEntry.Cat.Wisdom += (int)(behavior.valueData.wisdom  + buffData.WisdomPlus);
                if (behavior.valueData.charm != 0)
                    GameEntry.Cat.Charm += (int)(behavior.valueData.charm  + buffData.CharmPlus);
                GameEntry.Event.FireNow(this, BehaviorEventArgs.Create(behaviorTag));
            }
            else
            {
                GameEntry.Player.Ap = GameEntry.Player.MaxAp;
            }

            switch (behaviorTag)
            {
                case BehaviorTag.Read://智慧
                    OnRead(behavior);
                    break;
                case BehaviorTag.Sport://体魄
                    OnSport(behavior);
                    break;
                case BehaviorTag.Augur://魅力
                    OnAugur(behavior);
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

        //操作区域
        private void OnAugur(BehaviorData behavior)
        {
            BuffData buffData = GameEntry.Buff.GetBuff();
            ValueData valueData = new ValueData(behavior.valueData);
            valueData.charm = (int)(behavior.valueData.charm + buffData.CharmPlus);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm1, OnComplete, valueData);
        }
        private void OnSport(BehaviorData behavior)
        {
            BuffData buffData = GameEntry.Buff.GetBuff();
            ValueData valueData = new ValueData(behavior.valueData);
            valueData.stamina = (int)(behavior.valueData.stamina + buffData.StaminaPlus);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm2, OnComplete, valueData);
        }
        private void OnRead(BehaviorData behavior)
        {
            BuffData buffData = GameEntry.Buff.GetBuff();
            ValueData valueData = new ValueData(behavior.valueData);
            valueData.wisdom = (int)(behavior.valueData.wisdom + buffData.WisdomPlus);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm3, OnComplete, valueData);
        }
        public void OnSleep()
        {
            InDialog = false;
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Player.Energy = (int)(GameEntry.Player.MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus);
            GameEntry.Utils.GameState = GameState.Midnight;
            if (!GameEntry.Dialog.StoryUpdate(OnPassDay))
                Behaviour(BehaviorTag.Sleep);
        }
        private void OnPassDay()
        {
            GameEntry.Player.Ap = GameEntry.Player.MaxAp;
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            GameEntry.Player.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
            GameEntry.Utils.GameState = GameState.Morning;
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Morning));
            Invoke(nameof(OnFaded), 4f);
        }
        private void OnComplete()
        {
            InDialog=false;
            if (mBehaviorTag == BehaviorTag.Sleep)
            {
                rightCanvas.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(false);
                GameEntry.Player.Day++;
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
                GameEntry.Player.Ap -= behavior.valueData.ap;
                dialogBox.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(true);
                rightCanvas.gameObject.SetActive(true);
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
        public void OnCatDataEvent(object sender, GameEventArgs e) 
        {
            CatDataEventArgs charDataEvent = (CatDataEventArgs)e;
            CatData catData=charDataEvent.CatData;
            favorText.text = catData.favor.ToString();
            staminaText.text = catData.stamina.ToString();
            wisdomText.text = catData.wisdom.ToString();
            charmText.text=catData.charm.ToString();
            staminaLevelText.text = $"Lv.{catData.StaminaLevel}";
            wisdomLevelText.text = $"Lv.{catData.WisdomLevel}";
            charmLevelText.text = $"Lv.{catData.CharmLevel}";
        }
        public void OnPlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            favorText.text = GameEntry.Cat.Favor.ToString();
            staminaText.text = GameEntry.Cat.Stamina.ToString();
            wisdomText.text = GameEntry.Cat.Wisdom.ToString();
            charmText.text = GameEntry.Cat.Charm.ToString();
            staminaLevelText.text = $"Lv.{GameEntry.Cat.StaminaLevel}";
            wisdomLevelText.text = $"Lv.{GameEntry.Cat.WisdomLevel}";
            charmLevelText.text = $"Lv.{GameEntry.Cat.CharmLevel}";
            GameEntry.Sound.PlaySound(UnityEngine.Random.Range(24,27));
            Behaviour(BehaviorTag.Click);
        }
    }
}