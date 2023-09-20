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
        private MainState mMainState;

        public bool InGuide { get; set; } = true;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mIndex = 1;
            InGuide = true;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, OnMainStateEvent);
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
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, OnMainStateEvent);

            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            //等待场景加载后手动初始化一下数据
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (InGuide == false)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
            if (mMainState == MainState.Menu)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
        }

        public void OnComptele()
        {

        }

        public void OnMainStateEvent(object sender, GameEventArgs e)
        {
            MainStateEventArgs args = (MainStateEventArgs)e;
            mMainState = args.MainState;
        }

        private void OnLoadSceneSuccess(object sender,GameEventArgs e)
        { 
            LoadSceneSuccessEventArgs args= (LoadSceneSuccessEventArgs)e;
            if (args.SceneAssetName == sceneAssetName)
            {
                mOrderList = GameObject.Find("OrderList").GetComponent<OrderList>();
                mWorkForm = GameObject.Find("WorkForm").GetComponent<WorkForm>();

                mOrderList.IsShowItem = false;
                mWorkForm.IsGuide= true;
                mWorkForm.IsNext = false;
                mWorkForm.OnLevel("Guide_1");
                GameEntry.Dialog.PlayStory("Guide_1");//播放剧情
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
            if (args.GameState == GameState.ForeSpecial)
            { 
                
            }
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.OpenUIForm(UIFormId.GuideForm, this);

            }
            if (args.GameState == GameState.AfterSpecial)
            {
                mOrderList.IsShowItem = false;
                mWorkForm.IsGuide = true;
                mWorkForm.IsNext = false;
                if (mIndex == 1)
                {
                    mWorkForm.OnLevel("Guide_1");
                    GameEntry.Dialog.PlayStory("Guide_1");//播放剧情
                    mIndex++;
                    GameEntry.Utils.Day++;
                }
                else if (mIndex == 2)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    GameEntry.Dialog.PlayStory("Guide_2");//播放剧情
                    mWorkForm.OnLevel("Guide_2");
                    //GameEntry.Dialog.PlayStory("Guide_2");//播放剧情
                    mIndex++;
                    GameEntry.Utils.Day++;
                }
                else if (mIndex == 3)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    GameEntry.Dialog.PlayStory("Guide_2");//播放剧情
                    GameEntry.Dialog.PlayStory("Guide_2");//播放剧情
                    mIndex++;
                    GameEntry.Utils.Day++;
                }
                else
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
                    InGuide = false;
                }
            }
        }
    }
}
