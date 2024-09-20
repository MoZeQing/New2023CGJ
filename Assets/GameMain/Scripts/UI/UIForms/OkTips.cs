using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;
using DG.Tweening;

namespace GameMain
{
    public class OkTips : BaseForm
    {
        public UIFormId UIFormId { get; private set; } = UIFormId.OkTips;
        [SerializeField] private Text titleText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;
        [SerializeField] private Transform canvas;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.down * 100f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            titleText.text = BaseFormData.UserData.ToString();
            titleText.text= titleText.text.Replace("\\n", "\n");
            okBtn.onClick.AddListener(OnClick);
            cancelBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            okBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            BaseFormData.Action?.Invoke();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
