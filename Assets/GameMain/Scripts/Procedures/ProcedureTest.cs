using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;

namespace GameMain
{
    public class ProcedureTest : ProcedureBase
    {
        private GameState mGameState;
        private string sceneAssetName;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
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
            foreach (DRRecipe recipe in GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows())
            {
                GameEntry.Player.AddRecipe(recipe.Id);
            }
            sceneAssetName = AssetUtility.GetSceneAsset(drScene.AssetName);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Input.GetKey(KeyCode.Escape))
                GameEntry.UI.OpenUIForm(UIFormId.OptionForm);
            switch (mGameState)
            {
                case GameState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    //切换bgm
                    break;
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);

            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);

            GameEntry.Player.ClearRecipes();

            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
        }

        private void OnGameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            mGameState = args.GameState;
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs args = (LoadSceneSuccessEventArgs)e;
            if (args.SceneAssetName == sceneAssetName)
            {
                GamePosUtility.Instance.GamePosChange(GamePos.Down);
            }
        }
    }

}