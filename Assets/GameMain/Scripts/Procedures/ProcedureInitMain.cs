using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ProcedureInitMain : ProcedureBase
    {
        private bool m_Initialized = false;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_Initialized= false;
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccess);
            // 还原游戏速度
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (GameEntry.Scene.GetLoadedSceneAssetNames().Length == 0 &&
                GameEntry.Scene.GetLoadingSceneAssetNames().Length == 0)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }

        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccess);
        }

        private void LoadSceneSuccess(object sender, GameEventArgs e)
        { 
            LoadSceneSuccessEventArgs args= (LoadSceneSuccessEventArgs)e;
            m_Initialized = true;
        }
    }
}

