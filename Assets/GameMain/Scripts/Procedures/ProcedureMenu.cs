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
        private GameState mGameState;

        public void StartGame()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.SaveLoad.InitData();
            //GameEntry.Event.Subscribe(LoadingEventArgs.EventId, OnLoadingEvent);
        }

        private void OnLoadingEvent(object sender, GameEventArgs e)
        {
            LoadingEventArgs args = (LoadingEventArgs)e;

            GameEntry.Event.Unsubscribe(LoadingEventArgs.EventId, OnLoadingEvent);
        }

        public void ExitGame()
        { 
            
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("Menu");
            mGameState = GameState.Menu;
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, GameStateEvent);
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Base.ResetNormalGameSpeed();
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, GameStateEvent);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (mGameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Night:
                    ChangeState<ProcedureMain>(procedureOwner);
                    //ÇÐ»»bgm
                    break;
                case GameState.Guide:
                    ChangeState<ProcedureGuide>(procedureOwner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void GameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            mGameState = args.GameState;
        }
    }
}