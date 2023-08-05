using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        public Text moneyText;
        public Text favourText;
        public Text APText;
        public Text moodText;

        public Button touchBtn;
        public Button dialogueBtn;
        public Button playBtn;
        public Button storyBtn;
        public Button cleanBtn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(MainFormEventArgs.EventId, MainEvent);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(MainFormEventArgs.EventId, MainEvent);
        }

        private void MainEvent(object sender, GameEventArgs args)
        {
            MainFormEventArgs mainFormEvent = (MainFormEventArgs)args;
            MainFormTag tag = mainFormEvent.MainFormTag;
            switch (tag)
            {
                case MainFormTag.Up:
                    this.transform.DOLocalMove(new Vector3(1920f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Down:
                    this.transform.DOLocalMove(new Vector3(1920, 800f, 0f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Left:
                    this.transform.DOLocalMove(new Vector3(3840f, 800f, 0f), 1f).SetEase(Ease.OutExpo);
                    break;
                case MainFormTag.Right:
                    this.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
                    break;
            }
        }
    }
}