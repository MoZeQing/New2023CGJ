using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace GameMain
{
    /// <summary>
    /// 教学关卡（前3天的剧情）
    /// </summary>
    public class ProcedureGuide : ProcedureBase
    {
        private string sceneAssetName;
        private OrderList mOrderList;
        private WorkForm mWorkForm;

        private LevelData mLevelData;
        private LevelData levelData2;
        private LevelData levelData3;

        private int mIndex;
        private GameState mGameState;

        public bool InGuide { get; set; } = true;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mIndex = 1;
            InGuide = true;
            mGameState = GameState.Guide;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);
            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(4);
            //加载主界面
            Debug.Log(drScene != null);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.",4.ToString());
                return;
            }
            Debug.Log("Start Load Scene");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
            sceneAssetName = AssetUtility.GetSceneAsset(drScene.AssetName);

        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, OnDialogEvent);

            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.CloseAllLoadingUIForms();
            //等待场景加载后手动初始化一下数据
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (InGuide == false)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
            if (mGameState == GameState.Menu)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
        }

        private void OnLoadSceneSuccess(object sender,GameEventArgs e)
        { 
            LoadSceneSuccessEventArgs args= (LoadSceneSuccessEventArgs)e;
            if (args.SceneAssetName == sceneAssetName)
            {
                mOrderList = GameObject.Find("OrderList").GetComponent<OrderList>();
                mWorkForm = GameObject.Find("WorkForm").GetComponent<WorkForm>();

                mOrderList.IsShowItem = false;
                mWorkForm.OnLevel("(0)Guide_1");
                GameEntry.Player.Day++;
                mIndex++;
            }
        }

        private void OnDialogEvent(object sender, GameEventArgs e)
        { 
            DialogEventArgs args= (DialogEventArgs)e;
            mWorkForm.IsNext = !args.InDialog;
        }
        private void OnGameStateEvent(object sender, GameEventArgs e)
        { 
            GameStateEventArgs args= (GameStateEventArgs)e;
            mGameState = args.GameState;
            if (args.GameState == GameState.ForeSpecial)
            { 
                
            }
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.OpenUIForm(UIFormId.GuideForm, mIndex-2);

            }
            if (args.GameState == GameState.AfterSpecial)
            {
                mOrderList.IsShowItem = false;
                //mWorkForm.IsNext = false;
                if (mIndex == 1)
                {
                    GameEntry.Utils.PlayerData.guideID = 0;
                    mWorkForm.OnLevel("(0)Guide_1");
                    mIndex++;
                    GameEntry.Player.Day++;
                    Debug.Log(GameEntry.Player.Day);
                }
                else if (mIndex == 2)
                {
                    GameEntry.Utils.PlayerData.guideID = 1;
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    mWorkForm.OnLevel("(1)Guide_2");
                    mIndex++;
                    GameEntry.Player.Day++;
                    Debug.Log(GameEntry.Player.Day);
                }
                else if (mIndex == 3)
                {
                    GameEntry.Utils.PlayerData.guideID = 2;
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    mWorkForm.OnLevel("(2)Guide_3");
                    mIndex++;

                    Debug.Log(GameEntry.Player.Day);
                }
                else
                {
                    GameEntry.Utils.PlayerData.guideID = 3;
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    InGuide = false;
                    GameEntry.Sound.PlaySound(102);
                }
            }
        }
    }
}
