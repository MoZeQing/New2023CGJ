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
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Text timeText;
        [SerializeField] private Transform mCanvas;
        [Header("主控")]
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button optionBtn;
        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int m_RandomValue;
        private GamePos mGamePos=GamePos.Up;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            leftButton.onClick.AddListener(TurnLeft);
            rightButton.onClick.AddListener(TurnRight);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            saveBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));

            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
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

            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
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

        private void OnPlayerDataEvent(object sender, GameEventArgs e)
        {
            PlayerDataEventArgs args = (PlayerDataEventArgs)e;
            PlayerData playerData= args.PlayerData;
            timeText.text = string.Format("第{0}天", playerData.day);
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
                Camera.main.transform.DOMove(new Vector3(0f, -4.2f, -8f), 1f).SetEase(Ease.InOutExpo);
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