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
        [SerializeField] private Animator animator;
        [SerializeField,Range(0f,2f)] private float speed=1f;

        private Action mAction;
        private ValueData valueData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            valueData = BaseFormData.UserData as ValueData;
            mAction = BaseFormData.Action;
            progressImg.fillAmount = 0;
            canvas.transform.localPosition = Vector3.up * 1080f;
            canvas.transform.DOLocalMoveY(0f, speed).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                DoProgress();
            });
        }
        private void DoProgress()
        {
            progressImg.DOFillAmount(1f, speed * 2).OnComplete(() =>
            {
                DoComplete();
            });
        }
        private void DoComplete()
        {
            GameEntry.Sound.PlaySound(43);
            GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, valueData);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(0))
            {
                if (DOTween.IsTweening(canvas))
                {
                    canvas.DOKill();
                    canvas.localPosition = Vector3.zero;
                    DoProgress();
                    return;
                }
                if (DOTween.IsTweening(progressImg))
                {
                    progressImg.DOKill();
                    progressImg.fillAmount = 1f;
                    DoComplete();
                    return;
                }
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnExit()
        {
            mAction?.Invoke();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }

    public enum ActionTag
    { 
        Read,
        Augur,
        Run,
        Cake,
        Work
    }
}
