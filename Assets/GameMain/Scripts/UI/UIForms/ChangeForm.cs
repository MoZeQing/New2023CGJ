using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class ChangeForm : UIFormLogic
    {
        [SerializeField] private Animator animator;

        private bool mFlag = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mFlag = true;
            animator.SetBool("Into", true);
            mTime = 3f;
        }

        float mTime = 3f;

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            mTime -= Time.deltaTime;
            if (mTime < 0 && mFlag)
            {
                animator.SetBool("Into", false);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void Opened() => GameEntry.Event.FireNow(this,LoadingEventArgs.Create());
        private void Closed() => GameEntry.UI.CloseUIForm(this.UIForm);
    }
}
