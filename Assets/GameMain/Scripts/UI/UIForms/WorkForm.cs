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
        //将work里面的东西拿过来
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private Button recipeBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Text timeText;
        [SerializeField] private OrderList orderList;
        [SerializeField] private Transform modeCanvas;
        [SerializeField] private Button commonBtn;

        private LevelData mLevelData;
        private int mOrderCount;
        private float nowTime;
        private WorkData mWorkData;
        /// <summary>
        /// 是否正在播放剧情
        /// </summary>
        public bool IsDialog { get; set; }=false;


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
                OnWorkComplete();
            }
        }
        private void SetData()
        {
            //modeTitle.sprite = GameEntry.Utils.modeSprites[(int)mLevelData.levelTag];
            float power = 1f + (float)((GameEntry.Cat.StaminaLevel - 1f) / 6f);
            nowTime = mLevelData.levelTime*power;
            modeCanvas.gameObject.SetActive(false);
            orderList.IsShowItem = true;
            orderList.ShowItem(mLevelData.GetOrderDatas());
            orderList.IsShowItem = false;
        }
        public void OnLevel()
        {
            LevelData levelData = GameEntry.Level.GetLevelData();
            if (levelData != null)
                SetLevelData(levelData);
            else
                SetLevelData(GameEntry.Level.GetRandLevelData());
        }
        public void SetLevelData(LevelData levelData)
        { 
            mLevelData= levelData;
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
            IsDialog= false;
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.Special));
            GameEntry.Utils.GameState = GameState.Special;
        }
        private void OnWorkComplete()
        {
            GameEntry.Event.FireNow(this, LevelEventArgs.Create());
            GamePosUtility.Instance.GamePosChange(GamePos.Up);
            dialogBox.SetDialog(mLevelData.afterWork);
            dialogBox.SetComplete(OnAfterWorkComplete);
            orderList.ClearItems();

            mWorkData = new WorkData();
            mWorkData.Power = nowTime / (float)mLevelData.levelTime;
            mWorkData.Money = mLevelData.levelMoney;
            mWorkData.OrderCount = mLevelData.orderDatas.Count;

            IsDialog = true;
        }
        private void OnAfterWorkComplete()
        {
            GameEntry.Event.Fire(mWorkData, GameStateEventArgs.Create(GameState.AfterSpecial));
            GameEntry.UI.OpenUIForm(UIFormId.SettleForm, mWorkData);
        }

        private void OnOrderEvent(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (mLevelData != null)
            {
                if (mLevelData.GetOrderDatas().Contains(args.OrderData))
                {
                    mOrderCount++;
                    if (args.Income == 0)
                        return;
                    mWorkData.orderDatas.Add(args.OrderData);
                    mWorkData.Income += args.Income;
                }
                if (mOrderCount == mLevelData.GetOrderDatas().Count)
                {
                    OnWorkComplete();
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
        public List<OrderData> orderDatas = new List<OrderData>();
        public List<OrderData> levelDatas = new List<OrderData>();       
    }
}
