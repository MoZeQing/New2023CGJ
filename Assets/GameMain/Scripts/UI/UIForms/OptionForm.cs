using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace GameMain
{
    public class OptionForm : BaseForm
    {
        [SerializeField] private Button exit;
        [SerializeField] private Button main;
        [SerializeField] private Transform canvas;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exit.onClick.AddListener(()=> GameEntry.UI.CloseUIForm(this.UIForm));
            main.onClick.AddListener(() => GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu)));
    }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exit.onClick.RemoveAllListeners();
            main.onClick.RemoveAllListeners();
        }
    }
}

