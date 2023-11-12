using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class MapForm : UIFormLogic
    {
        [SerializeField] private Button mGreengrocerBtn;
        [SerializeField] private Button mGlassBtn;
        [SerializeField] private Button mCinemaBtn;
        [SerializeField] private Button mHospitalBtn;
        [SerializeField] private Button mRestaurantBtn;
        [SerializeField] private Button mBeachBtn;
        [SerializeField] private Button mBakeryBtn;
        [SerializeField] private Button mBookstoreBtn;
        [SerializeField] private Button mBlackMarketBtn;
        [SerializeField] private Button mParkBtn;

        [SerializeField] private Transform mCanvas;

        private MainState mMainState;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mMainState= (MainState)userData;

            mGreengrocerBtn.onClick.AddListener(() => Outing(OutingSceneState.Greengrocer));
            mGlassBtn.onClick.AddListener(() => Outing(OutingSceneState.Glass));
            mRestaurantBtn.onClick.AddListener(() => Outing(OutingSceneState.Restaurant));
            mBeachBtn.onClick.AddListener(() => Outing(OutingSceneState.Beach));
            mParkBtn.onClick.AddListener(() => Outing(OutingSceneState.Park));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void Outing(OutingSceneState outingSceneState)
        {
            GameEntry.Utils.outSceneState=outingSceneState;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);

            Invoke(nameof(ChangeGameState), 1.5f);
        }

        private void ChangeGameState()
        {
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(mMainState));
        }

        private void GamePosEvent(object sender, GameEventArgs args)
        {
            GamePosEventArgs gamePos = (GamePosEventArgs)args;
            switch (gamePos.GamePos)
            {
                case GamePos.Up:
                    mCanvas.transform.DOLocalMove(new Vector3(1920f, 0f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Down:
                    mCanvas.transform.DOLocalMove(new Vector3(1920f, -1080f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Left:
                    mCanvas.transform.DOLocalMove(new Vector3(3840f, 0f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Right:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
            }
        }
    }
}
