using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework.Sound;
using GameFramework.DataTable;
using GameFramework.Event;

namespace GameMain
{
    public class MainForm : UIFormLogic
    {
        [SerializeField] private Button downButton;
        [SerializeField] private Button upButton;
        [SerializeField] private Button catButton;
        [SerializeField] private Button recipeButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Text dayText;
        [SerializeField] private Text levelText;
        [SerializeField] private Transform canvasTrans;
        [SerializeField] private DialogForm dialogForm;
        [SerializeField] private GameObject mRecipeForm;
        [SerializeField] private GameObject mSettingForm;

        [SerializeField] private Text EspressoText;
        [SerializeField] private Text ConPannaText;
        [SerializeField] private Text MochaText;
        [SerializeField] private Text WhiteCoffeeText;
        [SerializeField] private Text CafeAmericanoText;
        [SerializeField] private Text LatteText;

        [SerializeField] private Button mDebugButton;

        //[SerializeField] private Text Timer;//计时器

        //private float mOrderTime;//倒计时
        //private bool mOnOrderTime;

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int m_RandomValue;

        public DialogForm DialogForm
        {
            get;
            private set;
        }
        public WorkForm WorkForm
        {
            get;
            private set;
        }
        private void Debug()
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CafeAmericano)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Latte)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ConPanna)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Mocha)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.WhiteCoffee)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ProcedureMain main = (ProcedureMain)userData;
            main.MainForm = this;

            upButton.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);
            catButton.onClick.AddListener(Cat);
            recipeButton.onClick.AddListener(Recipe);
            settingButton.onClick.AddListener(() => mSettingForm.SetActive(true));

            mDebugButton.onClick.AddListener(Debug);

            this.DialogForm = GetComponentInChildren<DialogForm>(true);
            this.WorkForm = GetComponentInChildren<WorkForm>(true);

            //playSoundParams.Loop = true;
            //playSoundParams.VolumeInSoundGroup = 0.3f;
            //playSoundParams.Priority = 64;
            //playSoundParams.SpatialBlend = 0f;
            //GameEntry.Sound.PlaySound("Assets/GameMain/Audio/BGM/maou_bgm_acoustic52.mp3", "BGM", playSoundParams);
            GameEntry.Sound.PlaySound(19);

            GameEntry.Event.Subscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, UpdateOrder);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //if (mOnOrderTime)
            //{
            //    mOrderTime -= Time.deltaTime;
            //    if (mOrderTime < 0)
            //    {
            //        GameEntry.Event.FireNow(this, ClockEventArgs.Create(false));
            //    }
            //}   
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, UpdateOrder);
        }
        public void Up()
        {
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/page_turn.mp3", "Sound");

            Camera.main.transform.DOMove(new Vector3(0, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, -800, 0), 1f).SetEase(Ease.OutExpo);
        }

        public void Down()
        {
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/page_turn.mp3", "Sound");

            Camera.main.transform.DOMove(new Vector3(0, -3.4f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutExpo);
        }

        private void Cat()
        {
            m_RandomValue = Random.Range(0, 30);

            if(m_RandomValue <= 10)
            {
                GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/Yudachi.mp3", "Sound");
            }
            else
            {
                GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/cat.mp3", "Sound");
            }
        }

        private void Recipe()
        {
            mRecipeForm.gameObject.SetActive(!mRecipeForm.gameObject.activeSelf);
        }
        /// <summary>
        /// 锁定该界面的UI
        /// </summary>
        public void LockGUI()
        {
            downButton.gameObject.SetActive(false);
            upButton.gameObject.SetActive(false);
            settingButton.gameObject.SetActive(false);
        }
        /// <summary>
        /// 解锁该界面的UI
        /// </summary>
        public void UnlockGUI()
        {
            downButton.gameObject.SetActive(true);
            upButton.gameObject.SetActive(true);
            settingButton.gameObject.SetActive(true);
        }

        private void LevelEvent(object sender, GameEventArgs e)
        {
            LevelEventArgs args = (LevelEventArgs)e;
            //mOnOrderTime = false;
            switch (args.MainState)
            {
                case MainState.Foreword:
                    LockGUI();
                    Up();
                    break;
                case MainState.Game:
                    //mOrderTime = args.LevelData.OrderData.OrderTime;
                    //mOnOrderTime= true;
                    UnlockGUI();
                    Down();
                    break;
                case MainState.Text:
                    LockGUI();
                    Up();
                    break;
                case MainState.Change:
                    LockGUI();
                    break;
            }
            dayText.text = string.Format("第：{0}天", args.LevelData.Day.ToString());
            levelText.text= string.Format("第：{0}单", args.LevelData.Index.ToString());
        }

        private void UpdateOrder(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.Check())
                return;
            OrderData orderData = args.OrderData;
            EspressoText.text = orderData.Espresso.ToString();
            ConPannaText.text = orderData.ConPanna.ToString();
            MochaText.text = orderData.Mocha.ToString();
            WhiteCoffeeText.text = orderData.WhiteCoffee.ToString();
            CafeAmericanoText.text = orderData.CafeAmericano.ToString();
            LatteText.text = orderData.Latte.ToString();
        }
    }
}

