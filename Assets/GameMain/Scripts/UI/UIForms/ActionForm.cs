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
        [SerializeField] private Transform TextCanvas;
        [SerializeField] private Image iconImg;
        [SerializeField] private Text textText;
        [SerializeField,Range(0f,2f)] private float speed=0.5f;

        private Action mAction;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Tuple<ValueTag, int> tuple = BaseFormData.UserData as Tuple<ValueTag, int>;
            //iconImg.sprite = Resources.Load<Sprite>($"Image/ValueTagIcon/{tuple.Item1}");
            textText.text = tuple.Item2 > 0 ? $"+{tuple.Item2}" : $"{tuple.Item2}";
            TextCanvas.localPosition = Vector3.down * 120f;
            mAction = BaseFormData.Action;
            progressImg.fillAmount = 0;
            canvas.transform.localPosition = Vector3.up * 1080f;
            canvas.transform.DOLocalMoveY(100f, speed).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                progressImg.DOFillAmount(1f, speed).OnComplete(() =>
                {
                    GameEntry.Sound.PlaySound(43);
                    TextCanvas.DOLocalMoveY(0f, speed).SetEase(Ease.OutExpo);
                });
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(0))
            {
                if (!DOTween.IsTweening(canvas) &&
                !DOTween.IsTweening(progressImg) &&
                !DOTween.IsTweening(TextCanvas))
                {
                    OnExit();
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
