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
            GameEntry.SaveLoad.InitData();
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("Menu");
            mGameState = GameState.Menu;
            //客制化组件的初始化在此处开始
            GameEntry.SaveLoad.LoadGame();
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, GameStateEvent);//事件监听切换模式
            //客制化组件的初始化在此处开始
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Base.ResetNormalGameSpeed();
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, GameStateEvent);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (mGameState)
            {
                case GameState.Test:
                    ChangeState<ProcedureTest>(procedureOwner);
                    break;
                case GameState.Night:
                    ChangeState<ProcedureMain>(procedureOwner);
                    //切换bgm
                    break;
                case GameState.Guide:
                    ChangeState<ProcedureGuide>(procedureOwner);
                    break;
                case GameState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    break;
            }
        }
        private void GameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            mGameState = args.GameState;
        }
    }
}