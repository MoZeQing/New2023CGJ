using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class ChangeForm : UIFormLogic
    {
        private float mTime;
        private float mDuration;

        [SerializeField] private Image m_Image;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            int index = (int)userData;
            m_Image.sprite = GameEntry.Utils.changeSprites[index];

            mDuration = 3f;
            mTime = 0;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            mTime += Time.deltaTime;
            if (mTime <= mDuration / 3f)
            {
                m_Image.color = new Color(1, 1, 1, 3 * mTime / mDuration);
            }
            else if (mTime > mDuration / 3f && mTime <= mDuration * 2f / 3f)
            {
                m_Image.color = Color.white;
            }
            else if (mTime > mDuration * 2f / 3f && mTime <= mDuration)
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
