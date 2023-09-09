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

        [SerializeField] private bool IsGuide;

        private List<LevelSO> levelSOs= new List<LevelSO>();
        private LevelData mLevelData;
        private WorkData workData=new WorkData();
        private float nowTime;
        private float levelTime;
        private bool isSpecial;
        private bool isDialog;

        private void Start()
        {
            workData.Income= 0;
            levelTime= 180;//3����
            nowTime = levelTime;
            isSpecial = false;
            isDialog = false;
            if (IsGuide == true)
            {
                orderList.IsShowItem = false;
                isDialog = true;
            }
        }

        private void OnEnable()
        {
            upBtn.onClick.AddListener(()=>GamePosUtility.Instance.GamePosChange(GamePos.Up));
            downBtn.onClick.AddListener(() => GamePosUtility.Instance.GamePosChange(GamePos.Down));
            recipeBtn.onClick.AddListener(() => recipeCanvas.gameObject.SetActive(true));
            testBtn.onClick.AddListener(OnLevel);

            levelSOs = new List<LevelSO>(Resources.LoadAll<LevelSO>("LevelData"));
        }

        private void OnDisable()
        {
            upBtn.onClick.RemoveAllListeners();
            downBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                recipeCanvas.gameObject.SetActive(false);
            }
            if (isDialog)
                return;
            nowTime -= Time.deltaTime;
            timeText.text = Math.Floor(nowTime).ToString();
            if (nowTime <= 0)
            {
                if (isSpecial)
                {
                    GameEntry.Event.FireNow(this, LevelEventArgs.Create());
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    dialogBox.SetDialog(mLevelData.failWork);
                    dialogBox.SetComplete(OnAfterWorkComplete);
                    isDialog= true;
                }
                else
                {
                    OnLevel();
                    isDialog = true;
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

        public void OnLevel(LevelData levelData)
        { 
            mLevelData= levelData;
            GamePosUtility.Instance.GamePosChange(GamePos.Up);
            dialogBox.SetDialog(mLevelData.foreWork);
            dialogBox.SetComplete(OnForeWorkComplete);
        }

        private void OnForeWorkComplete()
        {
            GamePosUtility.Instance.GamePosChange(GamePos.Down);
            nowTime = 30f;
            orderList.IsShowItem= true;
            orderList.ShowItem(mLevelData.orderData);
            orderList.IsShowItem = false;
            isDialog= false;
            isSpecial = true;
        }

        private void OnAfterWorkComplete()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettleForm, workData);
        }

        private void OnOrderEvent(object sender, GameEventArgs e)
        { 
            OrderEventArgs args= (OrderEventArgs)e;
            if (mLevelData.orderData == args.OrderData)
            {
                GamePosUtility.Instance.GamePosChange(GamePos.Up);
                dialogBox.SetDialog(mLevelData.afterWork);
                dialogBox.SetComplete(OnAfterWorkComplete);
            }
            if (args.Income == 0)
                return;
            workData.orderDatas.Add(args.OrderData);
            workData.Income += args.Income;
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
        public RandomEvent RandomEvent
        {
            get;
            set;
        }
        public List<OrderData> orderDatas = new List<OrderData>();
    }
}
