using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System;
using DG.Tweening;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private GameState mGameState;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Player.AddRecipe(1);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            mGameState = GameState.Afternoon;
            GameEntry.Utils.GameState = GameState.Afternoon;
            GameEntry.Dialog.StoryUpdate();
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            mGameState = GameState.Night;
            GameEntry.Utils.GameState = GameState.Night;
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, GameStateEvent);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            //场景加载
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), 0, this);
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, GameStateEvent);
            GameEntry.UI.CloseUIGroup("Default");
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
            if (GameEntry.Dialog.InDialog)
                return;
            switch (mGameState)
            {
                case GameState.None:
                    break;
                case GameState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //切换bgm
                    break;
                case GameState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                default:
                    break;
            }
        }
        private void GameStateEvent(object sender, GameEventArgs e)
        { 
            GameStateEventArgs args= (GameStateEventArgs)e;
            mGameState = args.GameState;
        }
    }
    /// <summary>
    /// 目前所处的主游戏状态
    /// </summary>
    public enum MainState
    {
        Undefined,
        Work,
        Teach,
        Menu,
        Outing,
        Dialog,
        Change,
        Guide
    }

    public enum TimeTag
    { 
        None,
        Morning,
        ForeWork,
        ForeSpecial,
        AfterSpecial,
        Afternoon,
        Evening,
        Night,
    }

    public enum Week
    { 
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public enum OutingSceneState
    {
        Main=-1,
        Home=0,//家
        Market=1,//市场
        Glass=2,//玻璃仪器店
        Gym=3,//体育馆
        Restaurant=4,//餐馆
        Beach=5,//海滩
        Clothing=6,//服装店
        Library=7//图书馆
    }
}