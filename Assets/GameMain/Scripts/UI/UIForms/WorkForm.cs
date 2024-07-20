using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using DG.Tweening;

namespace GameMain
{
    public class WorkForm : MonoBehaviour
    {
        [SerializeField] private BaseStage stage;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Button recipeBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Text timeText;
        [SerializeField] private OrderList orderList;
        [SerializeField] private Transform modeCanvas;
        [SerializeField] private Button commonBtn;
        [SerializeField] private Image modeTitle;
        [SerializeField] public bool IsGuide { get; set; }

        private List<OrderData> mOrderDatas;
        private LevelData mLevelData;
        private int mOrderCount;
        private float nowTime;
        private WorkData workData;

        /// <summary>
        /// 是否正在播放剧情
        /// </summary>
        public bool IsDialog { get; set; }=false;
        public bool IsNext
        {
            get
            {
                return dialogBox.IsNext;
            }
            set
            { 
                dialogBox.IsNext = value;
            }
        }
        private bool IsRecipe { get; set; }


        private void Start()
        {
            orderList.IsShowItem = false;
            IsDialog = true;
        }

        private void OnEnable()
        {
            GameEntry.Sound.PlaySound(300);

            recipeBtn.onClick.AddListener(() => 
            {
                GameEntry.UI.OpenUIForm(UIFormId.RecipeForm);
            });
            guideBtn.onClick.AddListener(() => 
            {
                GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 1);               
            });

            commonBtn.onClick.AddListener(SetData);
            mLevelData = GameEntry.Dialog.loadedLevelSOs[0].levelData;//默认订单
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void OnDisable()
        {
            recipeBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void Update()
        {
            if (GameEntry.Dialog.InDialog)
                return;
            if (IsDialog)
                return;
            if (modeCanvas.gameObject.activeSelf)
                return;
            if (GameEntry.UI.HasUIForm(UIFormId.RecipeForm) || GameEntry.UI.HasUIForm(UIFormId.GuideForm))
                return;
            nowTime -= Time.deltaTime;
            if (nowTime > 0)
                timeText.text = Math.Floor(nowTime).ToString();
            else
                timeText.text = "∞";
            if (nowTime <= 0&&nowTime>-1)
            {
                //如果时间不足，结束订单时间
                GameEntry.Event.FireNow(this, LevelEventArgs.Create());
                GamePosUtility.Instance.GamePosChange(GamePos.Up);
                dialogBox.SetDialog(mLevelData.afterWork);
                dialogBox.SetComplete(OnAfterWorkComplete);
                orderList.ClearItems();
                IsDialog = true;
            }
        }
        private void SetData()
        {
            //modeTitle.sprite = GameEntry.Utils.modeSprites[(int)mLevelData.levelTag];
            float power = 1f + (float)((GameEntry.Utils.CharData.StaminaLevel - 1f) / 6f);
            nowTime = mLevelData.levelTime*power;
            modeCanvas.gameObject.SetActive(false);
            orderList.IsShowItem = true;
            mOrderDatas = mLevelData.GetOrderDatas();
            orderList.ShowItem(mOrderDatas);
            orderList.IsShowItem = false;
        }
        public void OnLevel()
        {
            List<LevelSO> levels = new List<LevelSO>();
            foreach (LevelSO level in GameEntry.Dialog.loadedLevelSOs)
            {
                if (GameEntry.Utils.Check(level.trigger)&&!level.isRandom)
                {
                    levels.Add(level);
                }
            }
            if (levels.Count != 0)
            {
                OnLevel(levels[UnityEngine.Random.Range(0, levels.Count)]);
            }
            else
            {
                OnAfterWorkComplete();
            }
        }

        public void OnLevel(LevelSO levelSO)
        {
            GameEntry.Utils.AddFlag(levelSO.name);
            OnLevel(levelSO.levelData);
            if (GameEntry.Dialog.loadedLevelSOs.Contains(levelSO))
                GameEntry.Dialog.loadedLevelSOs.Remove(levelSO);
        }

        public void OnLevel(string levelName)
        {
            foreach (LevelSO level in GameEntry.Dialog.loadedLevelSOs)
            {
                if (level.name==levelName)
                {
                    OnLevel(level.levelData);
                    return;
                }
            }
        }

        private void OnLevel(LevelData levelData)
        { 
            mLevelData= levelData;
            GameEntry.Utils.isClose=mLevelData.isClose;
            mOrderCount = 0;
            IsDialog = true;
            modeCanvas.gameObject.SetActive(true);
            GamePosUtility.Instance.GamePosChange(GamePos.Up);
            dialogBox.SetDialog(mLevelData.foreWork);
            dialogBox.SetComplete(OnForeWorkComplete);
            GameEntry.Event.FireNow(this, LevelEventArgs.Create());
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.ForeSpecial));
        }

        private void OnForeWorkComplete()
        {
            GamePosUtility.Instance.GamePosChange(GamePos.Down);
            orderList.IsShowItem= true;
            nowTime = mLevelData.levelTime;
            mOrderDatas = mLevelData.GetOrderDatas();
            orderList.IsShowItem = false;
            IsDialog= false;
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.Special));
            GameEntry.Utils.GameState = GameState.Special;
        }

        private void OnAfterWorkComplete()
        {
            GameEntry.Event.Fire(workData, GameStateEventArgs.Create(GameState.AfterSpecial));
        }

        private void OnOrderEvent(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (mLevelData != null)
            {
                if (mOrderDatas.Contains(args.OrderData))
                {
                    mOrderCount++;
                }
                if (mOrderCount == mOrderDatas.Count)
                {
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    dialogBox.SetDialog(mLevelData.afterWork);
                    dialogBox.SetComplete(OnAfterWorkComplete);
                    workData = new WorkData();
                    workData.Power = nowTime / (float)mLevelData.levelTime;
                    workData.Money = mLevelData.levelMoney;
                    workData.OrderCount=mLevelData.orderDatas.Count;
                }
            }  
        }
    }
    [System.Serializable]
    public class WorkData
    {
        public int Income
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public int Administration
        {
            get;
            set;
        }
        public float Power
        {
            get;
            set;
        }
        public int Money
        {
            get;
            set;
        }
        public int Financial
        {
            get;
            set;
        }
        public int OrderCount
        {
            get;
            set;
        }
        public RandomEvent RandomEvent
        {
            get;
            set;
        } = new RandomEvent();
        public List<OrderData> orderDatas = new List<OrderData>();
        public List<OrderData> levelDatas = new List<OrderData>();       
    }
}
