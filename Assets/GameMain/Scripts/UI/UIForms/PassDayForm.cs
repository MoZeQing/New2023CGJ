using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class PassDayForm : UIFormLogic
    {
        [SerializeField] private Animator mAnimator;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Debug.Log("Ö´ÐÐ");
            Invoke(nameof(OnComplete), 4f);
        }
        private void OnComplete() => GameEntry.UI.CloseUIForm(this.UIForm);
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
