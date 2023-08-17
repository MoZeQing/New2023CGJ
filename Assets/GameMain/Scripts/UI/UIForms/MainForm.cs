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
        [SerializeField] private Transform mCanvas;

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int m_RandomValue;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            upButton.onClick.AddListener(() => GameEntry.Event.FireNow(this, GamePosEventArgs.Create(GamePos.Up)));
            downButton.onClick.AddListener(() => GameEntry.Event.FireNow(this, GamePosEventArgs.Create(GamePos.Down)));
            leftButton.onClick.AddListener(() => GameEntry.Event.FireNow(this, GamePosEventArgs.Create(GamePos.Left)));
            rightButton.onClick.AddListener(() => GameEntry.Event.FireNow(this, GamePosEventArgs.Create(GamePos.Right)));

            GameEntry.Event.Subscribe(GamePosEventArgs.EventId, GamePosEvent);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(GamePosEventArgs.EventId, GamePosEvent);
        }

        private void GamePosEvent(object sender, GameEventArgs args)
        {
            GamePosEventArgs gamePos = (GamePosEventArgs)args;
            switch (gamePos.GamePos)
            {
                case GamePos.Up:
                    Camera.main.transform.DOMove(new Vector3(0f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Down:
                    Camera.main.transform.DOMove(new Vector3(0f, -6f, -8f), 1f).SetEase(Ease.InOutExpo);
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
}