using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class PopTip : BaseForm
    {
        [SerializeField] private Text text;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            text.text=BaseFormData.UserData.ToString();
            Invoke(nameof(OnExit), 2f);
        }

        private void OnExit() => GameEntry.UI.CloseUIForm(this.UIForm);

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

