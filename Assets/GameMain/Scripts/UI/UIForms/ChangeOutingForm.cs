using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ChangeOutingForm : UIFormLogic
    {
        private ProcedureMain m_ProcedureMain = null;
        private Animator mFadeAnimator = null;

        private float time;
        private float nowTime;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMain= (ProcedureMain)userData;

            mFadeAnimator= this.GetComponent<Animator>();
            mFadeAnimator.SetBool("Fade", true);

            nowTime= Time.time;
            time = Time.time + 1f;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, LoadScene);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            nowTime += Time.deltaTime;
            if (nowTime >= time)
            {

            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, LoadScene);
        }

        private void LoadScene(object sender, GameEventArgs args)
        { 
            LoadSceneSuccessEventArgs loadScene= (LoadSceneSuccessEventArgs)args;
            if (loadScene.SceneAssetName == AssetUtility.GetSceneAsset("Main"))
            {
                mFadeAnimator.SetBool("ChangeScene", true);
            }
        }

    }

}