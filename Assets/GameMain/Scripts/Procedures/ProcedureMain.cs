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
        private MainState mMainState;
        private bool mDialog = false;  

        public GamePos GamePos
        {
            get;
            private set;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            //GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            //GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
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
                    //切换bgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //切换bgm
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    ChangeState<ProcedureOuting>(procedureOwner);
                    //切换bgm
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        //private void LoadCatSuccess(object sender, GameEventArgs e)
        //{
        //    ShowEntitySuccessEventArgs showEntitySuccess= (ShowEntitySuccessEventArgs)e;
        //    Cat cat = null;
        //    if (showEntitySuccess.Entity.TryGetComponent<Cat>(out cat))
        //    { 
        //        Cat= cat;
        //    }
        //    Cat.HideCat();
        //}
        private void MainStateEvent(object sender, GameEventArgs e)
        { 
            MainStateEventArgs args= (MainStateEventArgs)e;
            switch (args.MainState)
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
                    break;
                case MainState.Outing:
                    //切换bgm
                    GameEntry.Dialog.StoryUpdate();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mMainState = args.MainState;
        }
        private void GamePosEvent(object sender, GameEventArgs args)
        {
            GamePosEventArgs gamePos = (GamePosEventArgs)args;
            GamePos = gamePos.GamePos;
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
        Dialog
    }

    public enum OutingSceneState
    {
        Home,//家
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