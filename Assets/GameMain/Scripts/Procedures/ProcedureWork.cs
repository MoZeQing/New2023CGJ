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
        private MainState mMainState;
        private WorkData workData;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mMainState = MainState.Work;
            workData = new WorkData();
            GamePosUtility.Instance.GamePosChange(GamePos.Down);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(3);
            //º”‘ÿ÷˜ΩÁ√Ê
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 3.ToString());
                return;
            }
            //≥°æ∞º”‘ÿ
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);

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
            GameEntry.Utils.TimeTag = TimeTag.Afternoon;
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
                    //«–ªªbgm
                    break;
                case MainState.Work:
                    //«–ªªbgm
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    ChangeState<ProcedureOuting>(procedureOwner);
                    //«–ªªbgm
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
                    //«–ªªbgm
                    break;
            }
            mMainState = args.MainState;
        }
        private void OnOrderEvent(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            //if (mLevelData.orderData == args.OrderData)
            //{
            //    GamePosUtility.Instance.GamePosChange(GamePos.Up);
            //    dialogBox.SetDialog(mLevelData.afterWork);
            //    dialogBox.SetComplete(OnAfterWorkComplete);
            //}
            if (args.Income == 0)
                return;
            workData.orderDatas.Add(args.OrderData);
            workData.Income += args.Income;
        }
        private void OnGameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.AfterSpecial)
                GameEntry.UI.OpenUIForm(UIFormId.SettleForm, workData);
        }
    }
}