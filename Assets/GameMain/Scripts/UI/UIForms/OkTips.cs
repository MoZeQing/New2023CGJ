using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;

namespace GameMain
{
    public class OkTips : BaseForm
    {
        public UIFormId UIFormId { get; private set; } = UIFormId.OkTips;
        [SerializeField] private Text titleText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;

        private Action mAction;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mAction = BaseFormData.Action;
            titleText.text = BaseFormData.UserData.ToString();
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
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
