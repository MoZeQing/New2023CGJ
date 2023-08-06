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
    public partial class MainForm : UIFormLogic
    {
        [Header("固定区域")]
        [SerializeField] private Button downButton;
        [SerializeField] private Button upButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button catButton;
        [SerializeField] private Button recipeButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Text dayText;
        [SerializeField] private Text levelText;
        [SerializeField] private Transform workingTrans;
        [SerializeField] private Transform teachingTrans;
        [SerializeField] private GameObject mRecipeForm;
        [SerializeField] private GameObject mSettingForm;

        [SerializeField] private Button mDebugButton;

        [SerializeField] private Text Timer;//计时器

        private float mOrderTime;//倒计时
        private bool mOnOrderTime;

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int m_RandomValue;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ProcedureMain main = (ProcedureMain)userData;
            main.MainForm = this;

            upButton.onClick.AddListener(() => Move(MainFormTag.Up));
            downButton.onClick.AddListener(() => Move(MainFormTag.Down));
            leftButton.onClick.AddListener(() => Move(MainFormTag.Left));
            rightButton.onClick.AddListener(() => Move(MainFormTag.Right));
            //catButton.onClick.AddListener(Cat);
            recipeButton.onClick.AddListener(Recipe);
            settingButton.onClick.AddListener(() => mSettingForm.SetActive(true));

            mDebugButton.onClick.AddListener(Debug);

            GameEntry.Sound.PlaySound(19);

            GameEntry.Event.Subscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, UpdateOrder);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (mOnOrderTime)
            {
                mOrderTime -= Time.deltaTime;
                Timer.text = Mathf.Floor(mOrderTime).ToString();
                if (mOrderTime < 0)
                {
                    GameEntry.Event.FireNow(this, ClockEventArgs.Create(false));
                    mOnOrderTime = false;
                }
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, LevelEvent);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, UpdateOrder);
        }
        public void Move(MainFormTag tag)
        {
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/page_turn.mp3", "Sound");
            switch (tag)
            {
                case MainFormTag.Up:
                    teachingTrans.transform.DOLocalMove(new Vector3(0f, 0f, 0f),1f).SetEase(Ease.OutExpo);
                    workingTrans.transform.DOLocalMove(new Vector3(0f, -800f, 0f), 1f).SetEase(Ease.OutExpo);
                    Camera.main.transform.DOMove(new Vector3(0, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Down:
                    teachingTrans.transform.DOLocalMove(new Vector3(0f, 800f, 0f), 1f).SetEase(Ease.OutExpo);
                    workingTrans.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
                    Camera.main.transform.DOMove(new Vector3(0, -3.4f, -8f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Left:
                    teachingTrans.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
                    workingTrans.transform.DOLocalMove(new Vector3(1920f, -800f, 0f), 1f).SetEase(Ease.OutExpo);
                    Camera.main.transform.DOMove(new Vector3(-19.2f, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Right:
                    teachingTrans.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
                    workingTrans.transform.DOLocalMove(new Vector3(-1920f, -800f, 0f), 1f).SetEase(Ease.OutExpo);
                    Camera.main.transform.DOMove(new Vector3(19.2f, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
                    break;
            }
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
                    Move(MainFormTag.Up);
                    break;
                case MainState.Game:
                    mOrderTime = 2000f;
                    mOnOrderTime = true;
                    UnlockGUI();
                    Move(MainFormTag.Down);
                    break;
                case MainState.Text:
                    mOnOrderTime = false;
                    LockGUI();
                    Move(MainFormTag.Up);
                    break;
                case MainState.Change:
                    LockGUI();
                    break;
            }
            dayText.text = string.Format("第：{0}天", args.LevelData.Day.ToString());
            levelText.text= string.Format("第：{0}单", args.LevelData.Index.ToString());
        }
    }
}