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
        [SerializeField] private Button teachBtn;
        [SerializeField] private Button teachBtn1;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Animator mAnimator;
        [SerializeField] private TeachingForm mTeachingForm;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image changeBackgroundImg;
        [SerializeField] private List<Button> littleCatBtns;
        [SerializeField] private Image[] apPoints;
        [SerializeField] private Image apProgress;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text dayText;//日期文本框
        [SerializeField] private Text apText;//行动力文本框
        [Header("主控")]
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button guideBtn;
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
            GameEntry.Utils.WeatherTag = WeatherTag.None;
            teachBtn.onClick.AddListener(ChangeTeach);
            teachBtn1.onClick.AddListener(ChangeTeach);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            saveBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 3));
            friendBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.FriendForm));
            recipeBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.RecipeForm));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
            outBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.MapForm));
            buffBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.BuffForm));
            GameEntry.Event.Subscribe(OutEventArgs.EventId, BackMain);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            canvasGroup.interactable = true;
        }
        private float nowTime;
        [SerializeField,Range(0,5f)] private float rateTime=5f;
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            moneyText.text = moneyText.text = string.Format("{0}", GameEntry.Utils.Money.ToString());
            apText.text = $"AP:{GameEntry.Utils.Ap}/{GameEntry.Utils.MaxAp}";
            dayText.text = $"第{GameEntry.Utils.PlayerData.day % 7 + 1}天";
            ApUpdate();
            if (GameEntry.Utils.PlayerData.guideID == 6)
            {
                teachBtn.interactable = false;
            }
            else
            {
                teachBtn.interactable = true;
            }
            nowTime -=Time.deltaTime;
            if (nowTime <= 0)
            {
                nowTime = rateTime;
                ShowLittleCat();
            }

            BackgroundUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                if (!mAnimator.GetBool("Into"))
                    return;
                ChangeTeach();
            }
            ApUpdate();
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
            outBtn.onClick.RemoveAllListeners();
            buffBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(OutEventArgs.EventId, BackMain);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);

            for (int i = 0; i < littleCatBtns.Count; i++)
            {
                littleCatBtns[i].onClick.RemoveAllListeners();
            }
        }
        private void ShowLittleCat()
        {
            //HideLittleCat();
            //DRLittleCat littleCat = GameEntry.DataTable.GetDataTable<DRLittleCat>().GetDataRow(GameEntry.Utils.Closet);
            //int index = Random.Range(0, littleCatBtns.Count);
            //Button button = littleCatBtns[index];
            //button.gameObject.SetActive(true);
            //button.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{littleCat.ClothingPath}_{index + 1}");
        }
        private void HideLittleCat()
        {
            //for (int i = 0; i < littleCatBtns.Count; i++)
            //{
            //    littleCatBtns[i].gameObject.SetActive(false);
            //}
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
        private OutingSceneState outSceneState;
        private void BackMain(object sender, GameEventArgs e)
        {
            OutEventArgs args = (OutEventArgs)e;
            //if (args.OutingSceneState == OutingSceneState.Home)
            //{
            //    DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            //    changeBackgroundImg.sprite = backgroundImg.sprite;
            //    changeBackgroundImg.gameObject.SetActive(true);
            //    changeBackgroundImg.color = Color.white;
            //    backgroundImg.sprite = Resources.Load<Sprite>(weather.AssetName);
            //    changeBackgroundImg.DOColor(Color.clear, 3f).OnComplete(() => changeBackgroundImg.gameObject.SetActive(false));
            //    GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
            //}
        }
        private void ChangeTeach()
        {
            HideLittleCat();
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
            GameState gameState = GameEntry.Utils.GameState;
            if (GameEntry.Utils.IsRain)
            {
                weatherTag = WeatherTag.Rain;
            }
            else
            {
                switch (gameState)
                {
                    case GameState.Night:
                        if (GameEntry.Utils.Ap <= 0)
                        {
                            weatherTag = WeatherTag.Night;
                        }
                        else
                        {
                            weatherTag = WeatherTag.Afternoon;
                        }
                        break;
                    case GameState.Morning:
                        weatherTag = WeatherTag.Morning;
                        break;
                }
            }
            if (weatherTag == GameEntry.Utils.WeatherTag)
                return;
            GameEntry.Utils.WeatherTag=weatherTag;
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            changeBackgroundImg.sprite = backgroundImg.sprite;
            changeBackgroundImg.gameObject.SetActive(true);
            changeBackgroundImg.color = Color.white;
            backgroundImg.sprite = Resources.Load<Sprite>(weather.AssetName);
            changeBackgroundImg.DOColor(Color.clear, 3f).OnComplete(() => changeBackgroundImg.gameObject.SetActive(false));
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }

        private void ApUpdate()
        {
            int maxAp = GameEntry.Utils.MaxAp;
            int ap = GameEntry.Utils.Ap;
            for (int i = 0; i < apPoints.Length; i++)
            {
                apPoints[i].gameObject.SetActive(maxAp - ap <= i);
            }
            apProgress.fillAmount = (float)ap / (float)maxAp;
        }
    }

    public enum WeatherTag
    {
        None=0,
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