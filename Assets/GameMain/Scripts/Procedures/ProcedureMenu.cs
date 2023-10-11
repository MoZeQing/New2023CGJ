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

namespace GameMain
{
    public class ProcedureMenu : ProcedureBase
    {
        private MainMenu m_MenuForm = null;
        private MainState mMainState;

        public void StartGame()
        {           
            GameEntry.SaveLoad.InitData();
        }

        public void ExitGame()
        { 
            
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("Menu");
            mMainState = MainState.Menu;
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Base.ResetNormalGameSpeed();
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
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
                    break;
                case MainState.Outing:
                    //ÇÐ»»bgm
                    break;
                case MainState.Change:
                    ChangeState<ProcedureInitMain>(procedureOwner);
                    break;
                case MainState.Guide:
                    ChangeState<ProcedureGuide>(procedureOwner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void MainStateEvent(object sender, GameEventArgs e)
        {
            MainStateEventArgs args = (MainStateEventArgs)e;
            mMainState = args.MainState;
        }
    }
}