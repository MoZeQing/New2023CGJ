using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System;
using UnityEditor.SceneManagement;

namespace GameMain
{
    public class ProcedureWork : ProcedureBase
    {
        private MainState mMainState;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUIForm(UIFormId.MainForm);
            GameEntry.Event.FireNow(this, GamePosEventArgs.Create(GamePos.Down));

            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (GameEntry.Dialog.InDialog)
                return;
            switch (mMainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    ChangeState<ProcedureMain>(procedureOwner);
                    //ÇÐ»»bgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //ÇÐ»»bgm
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    ChangeState<ProcedureOuting>(procedureOwner);
                    //ÇÐ»»bgm
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void MainStateEvent(object sender, GameEventArgs e)
        {
            MainStateEventArgs args = (MainStateEventArgs)e;
            switch (args.MainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    //ÇÐ»»bgm
                    GameEntry.Utils.TimeTag = TimeTag.AfterWork;
                    GameEntry.Dialog.StoryUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mMainState = args.MainState;
        }
    }
}