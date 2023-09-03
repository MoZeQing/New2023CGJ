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
        private float mTime;
        private float mDuration;

        private bool mFire=false;

        [SerializeField] private Image m_Image;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mDuration = 2f;
            mTime = 0;
            m_Image.color = new Color(1, 1, 1, 3 * mTime / mDuration);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            mTime += Time.deltaTime;
            if (mTime > 0 && mTime <= mDuration/ 2f)
            {
                m_Image.color = Color.white;
            }
            else if (mTime > mDuration / 2f  && mTime <= mDuration)
            {
                m_Image.color = new Color(1, 1, 1,1-( 3 * mTime / mDuration - 2));
            }
            else if (mTime > mDuration)
            {
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
