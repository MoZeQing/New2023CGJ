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
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Transform mCanvas;
        [Header("Ö÷¿Ø")]
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Button friendBtn;
        [SerializeField] private Button recipeBtn;
        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int m_RandomValue;
        private GamePos mGamePos=GamePos.Up;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mCanvas.gameObject.SetActive(true);
            leftButton.onClick.AddListener(TurnLeft);
            rightButton.onClick.AddListener(TurnRight);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            saveBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm));
            friendBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.FriendForm));
            recipeBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.RecipeForm));

            GameEntry.Event.Subscribe(MainFormEventArgs.EventId, OnMainFormEvent);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            loadBtn.onClick.RemoveAllListeners();
            saveBtn.onClick.RemoveAllListeners();
            optionBtn.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
            rightButton.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            friendBtn.onClick.RemoveAllListeners();
            recipeBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(MainFormEventArgs.EventId, OnMainFormEvent);
        }
        private void TurnLeft()
        {
            switch (GamePosUtility.Instance.GamePos)
            {
                case GamePos.Up:
                    GamePosUtility.Instance.GamePosChange(GamePos.Left);
                    leftButton.interactable = false;
                    break;
                case GamePos.Right:
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    rightButton.interactable = true;
                    break;
            }
        }

        private void TurnRight() 
        {
            switch (GamePosUtility.Instance.GamePos)
            {
                case GamePos.Up:
                    GamePosUtility.Instance.GamePosChange(GamePos.Right);
                    rightButton.interactable = false;
                    break;
                case GamePos.Left:
                    GamePosUtility.Instance.GamePosChange(GamePos.Up);
                    leftButton.interactable = true;
                    break;
            }
        }

        private void OnMainFormEvent(object sender, GameEventArgs e) 
        {
            MainFormEventArgs args= (MainFormEventArgs)e;
            if (args.MainFormTag == MainFormTag.Lock)
                mCanvas.gameObject.SetActive(false);
            else
                mCanvas.gameObject.SetActive(true);
        }
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
        private set;
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