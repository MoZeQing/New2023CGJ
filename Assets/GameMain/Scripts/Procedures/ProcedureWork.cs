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
    public class ProcedureWork : ProcedureBase
    {
        private GameState mGameState;
        private string sceneAssetName;
        private OrderList mOrderList;
        private WorkForm mWorkForm;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mGameState = GameState.Work;
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);

            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(3);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 3.ToString());
                return;
            }
            //场景加载
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
            GameEntry.Utils.GameState = GameState.Work;
            GameEntry.Dialog.StoryUpdate();
            sceneAssetName = AssetUtility.GetSceneAsset(drScene.AssetName);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);

            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);

            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            GameEntry.Utils.GameState = GameState.Afternoon;
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (GameEntry.Dialog.InDialog)
                return;
            switch (mGameState)
            {
                case GameState.Afternoon:
                    ChangeState<ProcedureMain>(procedureOwner);
                    //切换bgm
                    break;
                case GameState.Work:
                    //切换bgm
                    break;
                case GameState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                default:
                    break;
            }
        }
        private void OnGameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            mGameState = args.GameState;
            if (mGameState == GameState.AfterSpecial)
            {
                WorkData mWorkData=sender as WorkData;
                GameEntry.UI.OpenUIForm(UIFormId.SettleForm, mWorkData);
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs args = (LoadSceneSuccessEventArgs)e;
            if (args.SceneAssetName == sceneAssetName)
            {
                GamePosUtility.Instance.GamePosChange(GamePos.Down);
                mOrderList = GameObject.Find("OrderList").GetComponent<OrderList>();
                mWorkForm = GameObject.Find("WorkForm").GetComponent<WorkForm>();

                mOrderList.IsShowItem = false;
                mWorkForm.OnLevel();
            }
        }
    }
}