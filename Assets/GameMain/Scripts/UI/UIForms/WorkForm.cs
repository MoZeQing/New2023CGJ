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
        [SerializeField] private Transform recipeCanvas;
        [SerializeField] private Button upBtn;
        [SerializeField] private Button downBtn;
        [SerializeField] private Button recipeBtn;
        [SerializeField] private Text timeText;
        [SerializeField] private OrderList orderList;

        [SerializeField] private Button testBtn;
        [SerializeField] private Button test2Btn;

        [SerializeField] public bool IsGuide { get; set; }

        private List<LevelSO> levelSOs= new List<LevelSO>();
        private LevelData mLevelData;
        private float nowTime;
        private float levelTime;
        private bool isSpecial;
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

        private void Start()
        {
            levelTime= 180;//3����
            nowTime = levelTime;
            isSpecial = false;
            IsDialog = false;
        }

        private void OnEnable()
        {
            //upBtn.onClick.AddListener(()=>GamePosUtility.Instance.GamePosChange(GamePos.Up));
            //downBtn.onClick.AddListener(() => GamePosUtility.Instance.GamePosChange(GamePos.Down));
            recipeBtn.onClick.AddListener(() => recipeCanvas.gameObject.SetActive(true));
            testBtn.onClick.AddListener(OnLevel);
            //test2Btn.onClick.AddListener(() => GameEntry.Utils.RunEvent(new EventData(EventTag.NextDay)));

            levelSOs = new List<LevelSO>(Resources.LoadAll<LevelSO>("LevelData"));
            mLevelData = levelSOs[0].levelData;
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);

        }

        private void OnDisable()
        {
            //upBtn.onClick.RemoveAllListeners();
            //downBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                recipeCanvas.gameObject.SetActive(false);
            }
            if (IsGuide)
                return;
            if (IsDialog)
                return;
            nowTime -= Time.deltaTime;
            if (nowTime > 0)
                timeText.text = Math.Floor(nowTime).ToString();
            else
                timeText.text = "∞";
            if (nowTime <= 0&&nowTime>-1)
            {
                if (isSpecial)
                {
                    GameEntry.Event.FireNow(this, LevelEventArgs.Create());
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    dialogBox.SetDialog(mLevelData.failWork);
                    dialogBox.Next();
                    dialogBox.SetComplete(OnAfterWorkComplete);
                    IsDialog= true;
                }
                else
                {
                    OnLevel();
                    IsDialog = true;
                }
            }
        }

        private void OnLevel()
        {
            GameEntry.Event.FireNow(this, LevelEventArgs.Create());
            foreach (LevelSO level in levelSOs)
            {
                //if (level.week != GameEntry.Utils.Week)
                //    continue;
                if (GameEntry.Utils.Check(level.trigger))
                {
                    OnLevel(level.levelData);
                    return;
                }
            }
        }

        public void OnLevel(string levelName)
        {
            foreach (LevelSO level in levelSOs)
            {
                //if (level.week != GameEntry.Utils.Week)
                //    continue;
                if (level.name==levelName)
                {
                    OnLevel(level.levelData);
                    return;
                }
            }
        }

        public void OnLevel(LevelData levelData)
        { 
            mLevelData= levelData;
            GamePosUtility.Instance.GamePosChange(GamePos.Up);
            dialogBox.SetDialog(mLevelData.foreWork);
            dialogBox.Next();
            dialogBox.SetComplete(OnForeWorkComplete);
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.ForeSpecial));
        }

        private void OnForeWorkComplete()
        {
            GamePosUtility.Instance.GamePosChange(GamePos.Down);
            nowTime = mLevelData.orderData.OrderTime;
            orderList.IsShowItem= true;
            orderList.ShowItem(mLevelData.orderData);
            orderList.IsShowItem = false;
            IsDialog= false;
            isSpecial = true;
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.Special));
        }

        private void OnAfterWorkComplete()
        {
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.AfterSpecial));
        }

        private void OnOrderEvent(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (mLevelData != null)
            {
                if (mLevelData.orderData == args.OrderData)
                {
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    dialogBox.SetDialog(mLevelData.afterWork);
                    dialogBox.Next();
                    dialogBox.SetComplete(OnAfterWorkComplete);
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

        public int Financial
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
    }
}
