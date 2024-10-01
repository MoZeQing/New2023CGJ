using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using DG.Tweening;
using Dialog;

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
        [SerializeField] private Text workText;
        [SerializeField] private Image tipsImg;
        [SerializeField] private Text tipsText;

        private LevelData mLevelData;
        private int mOrderCount;
        private float nowTime;
        private WorkData mWorkData;
        private List<OrderData> orderDatas= new List<OrderData>();
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
            GameEntry.Event.Subscribe(WorkEventArgs.EventId, OnWorkTipsEvent);
        }

        private void OnDisable()
        {
            recipeBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Unsubscribe(WorkEventArgs.EventId, OnWorkTipsEvent);
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
            orderDatas = mLevelData.GetRandOrderDatas();
            orderList.IsShowItem = true;
            orderList.ShowItem(orderDatas);
            orderList.IsShowItem = false;
            mWorkData = new WorkData();
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
            if (!string.IsNullOrEmpty(mLevelData.foreWork))
            {
                DialogData foreWorkDialog = GameEntry.Dialog.GetDialogData(mLevelData.foreWork);
                dialogBox.SetDialog(foreWorkDialog);
                dialogBox.SetComplete(OnForeWorkComplete);
                GameEntry.Event.FireNow(this, LevelEventArgs.Create());
                GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.ForeSpecial));
            }
            else
            {
                OnForeWorkComplete();
            }
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
            if (!string.IsNullOrEmpty(mLevelData.afterWork))
            {
                GameEntry.Event.FireNow(this, LevelEventArgs.Create());
                GamePosUtility.Instance.GamePosChange(GamePos.Up);
                DialogData afterWorkDialog = GameEntry.Dialog.GetDialogData(mLevelData.afterWork);
                dialogBox.SetDialog(afterWorkDialog);
                dialogBox.SetComplete(OnAfterWorkComplete);
            }
            else
            {
                OnAfterWorkComplete();
            }

            orderList.ClearItems();
            mWorkData.Power = nowTime / (float)mLevelData.levelTime;
            mWorkData.Money = mLevelData.levelMoney;
            mWorkData.OrderCount = mLevelData.orderDatas.Count;

            IsDialog = true;
        }
        private void OnAfterWorkComplete()
        {
            GameEntry.Event.Fire(mWorkData, GameStateEventArgs.Create(GameState.AfterSpecial));
        }
        private void OnWorkTipsEvent(object sender, GameEventArgs e)
        { 
            WorkEventArgs args=e as WorkEventArgs;
            switch (args.WorkTips)
            {
                case WorkTips.None:
                    tipsImg.gameObject.SetActive(false);
                    workText.text = args.Text;
                    break;
                case WorkTips.Tips:
                    tipsImg.gameObject.SetActive(true);
                    tipsText.text = args.Text;
                    tipsText.color = Color.clear;
                    tipsText.DOKill();
                    Sequence sequence = DOTween.Sequence(tipsText);
                    sequence.Append(tipsText.DOColor(Color.black, 0.5f));
                    sequence.Append(tipsText.DOColor(Color.clear, 0.5f));
                    sequence.SetLoops(2);
                    sequence.OnComplete(() =>
                    {
                        tipsText.text = string.Empty;
                        tipsImg.gameObject.SetActive(false);
                    });
                    break;
            }
        }
        private void OnOrderEvent(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (mLevelData != null)
            {
                if (orderDatas.Contains(args.OrderData))
                {
                    mOrderCount++;
                    if (args.Income != 0)
                    {
                        mWorkData.orderDatas.Add(args.OrderData);
                        mWorkData.Income += args.Income;
                    }
                }
                if (mOrderCount == orderDatas.Count)
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
