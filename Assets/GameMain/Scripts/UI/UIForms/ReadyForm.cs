using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class ReadyForm : UIFormLogic
    {
        [SerializeField] private Button m_Ready;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Ready.onClick.AddListener(Ready_OnClick);
        }

        private void Ready_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MapForm, this);
        }


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

