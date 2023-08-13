using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class ProcedureTeach : ProcedureBase
    {
        private MainState mMainState;
        private DialogForm mDialogForm;
        private TeachingForm mTeachingForm;
        private ChangeForm mChangeForm;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void DialogEvent(object sender, GameEventArgs e)
        {
            DialogEventArgs dialog = (DialogEventArgs)e;
            switch (mMainState)
            {
                case MainState.Dialog:
                    mMainState = MainState.Teach;
                    break;
            }
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            switch (mMainState)
            {
                case MainState.Teach:
                    Teach();
                    break;
                case MainState.Change:
                    Change();
                    break;
                case MainState.Dialog:
                    Dialog();
                    break;
            }
        }

        private void Teach()
        {
            GameEntry.UI.OpenUIForm(UIFormId.TeachForm, this);
        }

        private void Change() 
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
        }

        private void Dialog()
        {
            GameEntry.UI.OpenUIForm(UIFormId.DialogForm, this);
        }
    }
}
