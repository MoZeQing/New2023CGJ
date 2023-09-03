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
    public class ProcedureMenu : ProcedureBase
    {
        private MainMenu m_MenuForm = null;
        private MainState mMainState;

        public void StartGame()
        {           
            InitData();
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Teach));
        }

        public void ExitGame()
        { 
            
        }

        /// <summary>
        /// ≥ı ºªØ”Œœ∑£®≤‚ ‘£©
        /// </summary>
        private void InitData()
        {
            GameEntry.Utils.MaxEnergy = 80;
            GameEntry.Utils.Energy = 80;
            GameEntry.Utils.MaxAp = 6;
            GameEntry.Utils.Ap = 6;
            GameEntry.Utils.Money = 10000;
            GameEntry.Utils.Mood = 20;
            GameEntry.Utils.Favor = 0;
            GameEntry.Dialog.LoadGame();
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("Menu");
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            //–∂‘ÿÀ˘”–≥°æ∞
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
                    //«–ªªbgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //«–ªªbgm
                    break;
                case MainState.Menu:
                    break;
                case MainState.Outing:
                    //«–ªªbgm
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