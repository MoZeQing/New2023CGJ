using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace GameMain
{
    public class ActionForm : BaseForm
    {
        [SerializeField] private Image progressImg;
        [SerializeField] private Image catImg;
        [SerializeField] private Transform canvas;

        private Action mAction;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Dictionary<ValueTag, int> pairs = BaseFormData.UserData as Dictionary<ValueTag, int>;
            mAction = BaseFormData.Action;
            progressImg.fillAmount = 0;
            canvas.transform.localPosition = Vector3.up * 1080f;
            canvas.transform.DOLocalMoveY(0, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                progressImg.DOFillAmount(1f, 5f).OnComplete(() =>
                {
                    GameEntry.Sound.PlaySound(43);
                    GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, pairs);
                });
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnExit()
        {
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
