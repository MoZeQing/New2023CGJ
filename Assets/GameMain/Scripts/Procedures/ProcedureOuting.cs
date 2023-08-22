using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using UnityEditor;
using System;

namespace GameMain
{
    public class ProcedureOuting : ProcedureBase
    {
        private MainState mMainState;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mMainState = MainState.Outing;
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.OpenUIForm((UIFormId)(12 + (int)GameEntry.Utils.outSceneState), this);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.UI.CloseUIGroup("Default");
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
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
                    if (GameEntry.Dialog.StoryUpdate())
                        return;
                    break;
                case MainState.Work:
                    //ÇÐ»»bgm
                    if (GameEntry.Dialog.StoryUpdate())
                        return;
                    break;
                case MainState.Menu:
                    break;
                case MainState.Outing:
                    //ÇÐ»»bgm
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mMainState = args.MainState;
        }
    }
}