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
        [SerializeField] private Button hardBtn;
        [SerializeField] private Button commonBtn;
        [SerializeField] private Button easyBtn;
        [SerializeField] private bool isGuide;

        [SerializeField] public bool IsGuide { get; set; }

        private List<LevelSO> levelSOs= new List<LevelSO>();
        private List<LevelSO> mRandomSos = new List<LevelSO>();
        private LevelData mLevelData;
        private int mOrderCount;
        private float nowTime;
        private float levelTime;
        private bool isSpecial;
        private bool isChoice;
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
            orderList.IsShowItem = false;
            isChoice = false;
            isSpecial = false;
            IsDialog = false;
        }

        private void OnEnable()
        {
            //upBtn.onClick.AddListener(()=>GamePosUtility.Instance.GamePosChange(GamePos.Up));
            //downBtn.onClick.AddListener(() => GamePosUtility.Instance.GamePosChange(GamePos.Down));
            recipeBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.RecipeForm));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm));
            //testBtn.onClick.AddListener(OnLevel);
            //test2Btn.onClick.AddListener(() => GameEntry.Utils.RunEvent(new EventData(EventTag.NextDay)));

            if (!isGuide)
            {
                Debug.Log(GameEntry.Utils.Energy);
                hardBtn.onClick.AddListener(() => SetData(180));
                if (GameEntry.Utils.Energy < 60) hardBtn.interactable = false;
                commonBtn.onClick.AddListener(() => SetData(135));
                if (GameEntry.Utils.Energy < 40) commonBtn.interactable = false;
                easyBtn.onClick.AddListener(() => SetData(90));
                if (GameEntry.Utils.Energy < 20) easyBtn.interactable = false;
            }
            levelSOs = new List<LevelSO>(Resources.LoadAll<LevelSO>("LevelData"));
            mLevelData = levelSOs[0].levelData;
            foreach (LevelSO level in mRandomSos)
            {
                if (level.isRandom) mRandomSos.Add(level);
            }
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void OnDisable()
        {
            //upBtn.onClick.RemoveAllListeners();
            //downBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            mRandomSos.Clear();
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void Update()
        {
            if (isChoice == false)
                return;
            if (GameEntry.Dialog.InDialog)
                return;
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
                    //dialogBox.Next();
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
        private void SetData(int time)
        {
            levelTime = time;//3����
            nowTime = levelTime;
            GameEntry.Utils.Energy -= (time - 120) / 3 + 20;
            Debug.Log((time - 120) / 3 + 20);
            modeCanvas.gameObject.SetActive(false);
            isChoice = true;
            orderList.IsShowItem = true;
        }
        private void OnLevel()
        {
            GameEntry.Event.FireNow(this, LevelEventArgs.Create());
            List<LevelSO> levels = new List<LevelSO>();
            foreach (LevelSO level in levelSOs)
            {
                if (GameEntry.Utils.Check(level.trigger)&&!level.isRandom)
                {
                    levels.Add(level);
                }
            }
            if (levels.Count != 0)
            {
                OnLevel(levels[UnityEngine.Random.Range(0, levels.Count)].levelData);
            }
            else//合法则随机选择，非法则全随机
            {
                OnLevel(mRandomSos[UnityEngine.Random.Range(0, mRandomSos.Count)].levelData);
            }
        }

        public void OnLevel(string levelName)
        {
            foreach (LevelSO level in levelSOs)
            {
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
            mOrderCount = 0;
            GamePosUtility.Instance.GamePosChange(GamePos.Up);
            dialogBox.SetDialog(mLevelData.foreWork);
            //dialogBox.Next();
            dialogBox.SetComplete(OnForeWorkComplete);
            GameEntry.Event.Fire(this, GameStateEventArgs.Create(GameState.ForeSpecial));
        }

        private void OnForeWorkComplete()
        {
            GamePosUtility.Instance.GamePosChange(GamePos.Down);
            nowTime = mLevelData.orderTime;
            orderList.IsShowItem= true;
            orderList.ShowItem(mLevelData.orderDatas);
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
                if (mLevelData.orderDatas.Contains(args.OrderData))
                {
                    if (args.Income == 0)
                        return;
                    mOrderCount++;
                    GameEntry.Utils.friends[mLevelData.charSO.name] += mLevelData.favor / mLevelData.orderDatas.Count;
                }
                if (mOrderCount == mLevelData.orderDatas.Count)
                {
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    dialogBox.SetDialog(mLevelData.afterWork);
                    //dialogBox.Next();
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
        public List<OrderData> levelDatas = new List<OrderData>();       
    }
}
