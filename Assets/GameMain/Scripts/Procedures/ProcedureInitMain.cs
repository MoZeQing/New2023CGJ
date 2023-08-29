using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
            GameEntry.Base.ResetNormalGameSpeed();
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();          

            InitData();
            ChangeState<ProcedureMain>(procedureOwner);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccess);
        }

        /// <summary>
        /// 初始化游戏（测试）
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
        }

        private void LoadSceneSuccess(object sender, GameEventArgs e)
        { 
            LoadSceneSuccessEventArgs args= (LoadSceneSuccessEventArgs)e;
            m_Initialized = true;
        }
    }
}

