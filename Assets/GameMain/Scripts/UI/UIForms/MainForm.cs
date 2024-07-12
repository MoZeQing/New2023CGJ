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
        [Header("¹Ì¶¨ÇøÓò")]
        [SerializeField] private Button teachBtn;
        [SerializeField] private Button teachBtn1;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Animator mAnimator;
        [SerializeField] private TeachingForm mTeachingForm;
        [SerializeField] private Image backgroundImg;
        [Header("Ö÷¿Ø")]
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Button upgradeBtn;
        [SerializeField] private Button friendBtn;
        [SerializeField] private Button recipeBtn;
        [SerializeField] private Button closetBtn;
        [SerializeField] private Button outBtn;
        [SerializeField] private Button buffBtn;
        [SerializeField] private CanvasGroup canvasGroup;
        private PlaySoundParams playSoundParams = PlaySoundParams.Create();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            teachBtn.onClick.AddListener(ChangeTeach);
            teachBtn1.onClick.AddListener(ChangeTeach);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            saveBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm));
            friendBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.FriendForm));
            recipeBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm));
            upgradeBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.UpgradeForm));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
            outBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.MapForm));
            buffBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.BuffForm));
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            canvasGroup.interactable = true;
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            BackgroundUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                if (!mAnimator.GetBool("Into"))
                    return;
                ChangeTeach();
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            teachBtn.onClick.RemoveAllListeners();
            teachBtn1.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
            saveBtn.onClick.RemoveAllListeners();
            optionBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            friendBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();
            closetBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.RemoveAllListeners();
            outBtn.onClick.RemoveAllListeners();
            buffBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
        }
        private void Change(GamePos gamePos)
        {
            Debug.Log(gamePos);
            switch (gamePos)
            {
                case GamePos.Left:
                    mCanvas.transform.DOLocalMoveX(1920f, 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Right:
                    mCanvas.transform.DOLocalMoveX(-1920f, 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Up:
                    mCanvas.transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
                    GameEntry.Utils.UpdateData();
                    break;
            }
        }
        private void ChangeTeach()
        {
            mAnimator.SetBool("Into", !mAnimator.GetBool("Into"));
            canvasGroup.interactable = !mAnimator.GetBool("Into");
            teachBtn1.interactable = mAnimator.GetBool("Into");
            if (mAnimator.GetBool("Into"))
            {
                mTeachingForm.Click_Action();
            }
        }
        private void OnGameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Night || args.GameState == GameState.Afternoon || args.GameState == GameState.Morning)
            {
                canvasGroup.interactable = true;
            }
        }
        private WeatherTag weatherTag;
        private void BackgroundUpdate()
        {
            //GameState gameState = GameEntry.Utils.GameState;
            //WeatherTag weatherTag = WeatherTag.Morning;
            //if (GameEntry.Utils.IsRain)
            //{
            //    weatherTag = WeatherTag.Rain;
            //}
            //else
            //{
            //    switch (gameState)
            //    {
            //        case GameState.Night:
            //            weatherTag = WeatherTag.Night;
            //            break;
            //        case GameState.Afternoon:
            //            weatherTag = WeatherTag.Afternoon;
            //            break;
            //        case GameState.Morning:
            //            weatherTag = WeatherTag.Morning;
            //            break;
            //    }
            //}
            if (weatherTag == GameEntry.Utils.WeatherTag)
                return;
            weatherTag=GameEntry.Utils.WeatherTag;
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            backgroundImg.sprite = Resources.Load<Sprite>(weather.AssetName);
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }
    }

    public enum WeatherTag
    {
        Morning=1,
        Afternoon=2,
        Night=3,
        Rain=4
    }
}

public class GamePosUtility
{
    private static GamePosUtility instance;
    private GamePosUtility() { }
    public static GamePosUtility Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GamePosUtility();
            }
            return instance;
        }
    }

    public GamePos GamePos
    {
        get;
        set;
    }

    public void GamePosChange(GamePos gamePos)
    {
        GamePos= gamePos;
        switch (GamePos)
        {
            case GamePos.Up:
                Camera.main.transform.DOMove(new Vector3(0f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Down:
                Camera.main.transform.DOMove(new Vector3(0f, -6.2f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Left:
                Camera.main.transform.DOMove(new Vector3(-19.2f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Right:
                Camera.main.transform.DOMove(new Vector3(19.2f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
        }
    }
}