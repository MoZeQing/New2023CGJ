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

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int r;

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

            this.DialogForm = GetComponentInChildren<DialogForm>(true);
            this.WorkForm = GetComponentInChildren<WorkForm>(true);

            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 0.3f;
            playSoundParams.Priority = 64;
            playSoundParams.SpatialBlend = 0f;
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/BGM/maou_bgm_acoustic52.mp3", "BGM", playSoundParams);

            GameEntry.Event.Subscribe(MainFormEventArgs.EventId, MainFormEvent);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(MainFormEventArgs.EventId, MainFormEvent);
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
            r = Random.Range(0, 30);

            if(r == 0)
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
        private void UpdateTime()
        {
            //dayText.text;
            //levelText.text;
        }
        private void MainFormEvent(object sender, GameEventArgs e)
        {
            MainFormEventArgs args = (MainFormEventArgs)e;
            switch (args.MainFormTag)
            {
                case MainFormTag.Lock:
                    LockGUI();
                    break;
                case MainFormTag.Unlock:
                    UnlockGUI();
                    break;
                case MainFormTag.Up:
                    Up();
                    break;
                case MainFormTag.Down:
                    Down();
                    break;
            }
        }
    }

}

