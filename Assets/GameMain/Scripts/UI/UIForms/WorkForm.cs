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

        private LevelData mLevelData;
        private int mOrderCount;
        private float nowTime;
        private float levelTime;
        private bool isSpecial;
        private bool isChoice;

        private bool flag=false;
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
                hardBtn.onClick.AddListener(() => SetData(120,0,0.8f,1.5f));
                //if (GameEntry.Utils.Energy < 60) hardBtn.interactable = false;
                commonBtn.onClick.AddListener(() => SetData(120,0,1f,1f));
                //if (GameEntry.Utils.Energy < 40) commonBtn.interactable = false;
                easyBtn.onClick.AddListener(() => SetData(120,0,1.5f,0.8f));
                //if (GameEntry.Utils.Energy < 20) easyBtn.interactable = false;
            }
            mLevelData = GameEntry.Dialog.loadedLevelSOs[0].levelData;
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void OnDisable()
        {
            //upBtn.onClick.RemoveAllListeners();
            //downBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                nowTime -= 10f;

            if(Input.GetKeyDown(KeyCode.Space))
                flag= !flag;

            if (flag)
                return;

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
                    //dialogBox.Next();
                    dialogBox.SetDialog(mLevelData.afterWork);
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
        private void SetData(int time,int energy,float orderPower,float pricePower)
        {
            BuffData buffData = GameEntry.Buff.GetBuff();
            levelTime = (int)(time*buffData.TimeMulti + buffData.TimePlus);//3����
            nowTime = levelTime;
            GameEntry.Utils.Energy -= energy;
            GameEntry.Utils.OrderPower = orderPower;
            GameEntry.Utils.PricePower = pricePower;
            modeCanvas.gameObject.SetActive(false);
            isChoice = true;
            orderList.IsShowItem = true;
        }
        private void OnLevel()
        {
            GameEntry.Event.FireNow(this, LevelEventArgs.Create());
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
                    if (GameEntry.Utils.GetFriends().ContainsKey(mLevelData.charSO.name))
                        GameEntry.Utils.AddFriendFavor(mLevelData.charSO.name, mLevelData.favor / mLevelData.orderDatas.Count);
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
