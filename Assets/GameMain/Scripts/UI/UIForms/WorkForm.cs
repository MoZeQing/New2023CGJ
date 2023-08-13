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
    public class WorkForm : UIFormLogic
    {
        [SerializeField] private Transform mCanvas;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(GamePosEventArgs.EventId, GamePosEvent);

            mCanvas.transform.localPosition = new Vector3(0f, -1080f, 0f);
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
                    mCanvas.transform.DOLocalMove(new Vector3(0f, -1080, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Down:
                    mCanvas.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
                case GamePos.Left:
                    mCanvas.transform.DOLocalMove(new Vector3(1920f, -1080, 0f), 1f).SetEase(Ease.InOutExpo);
                    break;
            }
        }
    }
}
