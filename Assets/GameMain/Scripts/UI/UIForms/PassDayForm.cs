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
            //mTime += Time.deltaTime;
            //if (mTime > 0 && mTime <= mDuration / 2f)
            //{
            //    m_Image.color = Color.white;
            //}
            //else if (mTime > mDuration / 2f && mTime <= mDuration)
            //{
            //    m_Image.color = new Color(1, 1, 1, 1 - (3 * mTime / mDuration - 2));
            //}
            //else if (mTime > mDuration)
            //{
            //    GameEntry.UI.CloseUIForm(this.UIForm);
            //}
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
