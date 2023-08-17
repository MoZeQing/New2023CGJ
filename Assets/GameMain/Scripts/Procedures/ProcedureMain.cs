using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private MainState mMainState;
        public Cat Cat
        {
            get;
            set;
        } = null;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            //初始化信息
            InitMain();     
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (mMainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    //切换bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                case MainState.Work:
                    //切换bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    //切换bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 初始化游戏（测试）
        /// </summary>
        private void InitMain()
        {
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            //场景加载
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.TeachForm, this);
            //初始化角色
            GameEntry.Entity.ShowCat(new CatData(GameEntry.Entity.GenerateSerialId(), 10008)
            {
                Position = new Vector3(0f, 4.6f)
            });

            InitData();
        }
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

        private void LoadCatSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs showEntitySuccess= (ShowEntitySuccessEventArgs)e;
            Cat cat = null;
            if (showEntitySuccess.Entity.TryGetComponent<Cat>(out cat))
            { 
                Cat= cat;
            }
            Cat.HideCat();
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
        Outing
    }

    public enum OutingSceneState
    {
        Greengrocer,//果蔬商
        Glass,//玻璃仪器店
        Cinema,//电影院
        Hospital,//医院
        Restaurant,//餐馆
        Beach,//海滩
        Bakery,//烘培店
        Bookstore,//书店
        BlackMarket,//黑市
        Park//公园
    }
}