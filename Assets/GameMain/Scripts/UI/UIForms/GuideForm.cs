using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace GameMain
{
    public class GuideForm : UIFormLogic
    {
        [SerializeField] private Text text;
        [SerializeField] private List<string> dialogs;

        private int index;
        private ProcedureGuide mProcedureGuide;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mProcedureGuide= (ProcedureGuide)userData;
            text.text = dialogs[0];
            index = 1;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(0))
            {
                if (index >= dialogs.Count)
                {
                    GameEntry.UI.CloseUIForm(this.UIForm);
                    mProcedureGuide.OnComptele();
                    return;
                }
                text.text = dialogs[index];
                index++;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
