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
using UnityEditor.SceneManagement;

namespace GameMain
{
    public class ProcedureMenu : ProcedureBase
    {
        private MainMenu m_MenuForm = null;
        private MainState mMainState;

        public void StartGame()
        {           
            InitData();
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Teach));
        }

        public void ExitGame()
        { 
            
        }

        /// <summary>
        /// ≥ı ºªØ”Œœ∑£®≤‚ ‘£©
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
            GameEntry.Utils.Love = 0;
            GameEntry.Utils.Family= 0;
            GameEntry.Utils.Day = 0;
            GameEntry.Utils.closet = 101;
            GameEntry.Utils.ClearFlag();
            GameEntry.Dialog.LoadGame();
            IDataTable<DRItem> items= GameEntry.DataTable.GetDataTable<DRItem>();
            foreach (DRItem item in items) 
            {
                //if (item.Kind != (int)ItemKind.Materials)
                //continue;
                PlayerItemData itemData = new PlayerItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price= item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable=item.Equipable;
                GameEntry.Utils.PlayerData.items.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Materials)
                    continue;
                ShopItemData itemData = new ShopItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.maxNum = item.MaxNum;
                GameEntry.Utils.greengrocerItemDatas.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Book)
                    continue;
                ShopItemData itemData = new ShopItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.maxNum = item.MaxNum;
                GameEntry.Utils.bookstoreItemDatas.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Music)
                    continue;
                MusicItemData itemData = new MusicItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.AbilityModifier = item.AMInfo;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.Favor = item.Favor;
                GameEntry.Utils.musicHallItemDatas.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Glass)
                    continue;
                ShopItemData itemData = new ShopItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.maxNum = item.MaxNum;
                GameEntry.Utils.glassItemDatas.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Dishes)
                    continue;
                ShopItemData itemData = new ShopItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.maxNum = item.MaxNum;
                GameEntry.Utils.restaurantItemDatas.Add(itemData);
            }
            foreach (DRItem item in items)
            {
                if (item.Kind != (int)ItemKind.Food)
                    continue;
                ShopItemData itemData = new ShopItemData();
                itemData.itemName = item.Name;
                itemData.itemTag = (ItemTag)item.Id;
                itemData.itemInfo = item.Info;
                itemData.price = item.Price;
                itemData.filterMode = (GameMain.FilterMode)item.FilterMode;
                itemData.equipable = item.Equipable;
                itemData.maxNum = item.MaxNum;
                GameEntry.Utils.bakeryItemDatas.Add(itemData);
            }
            GameEntry.Utils.AddPlayerItem(new PlayerItemData(new ItemData(ItemTag.Closet1), 1), 1);
            GameEntry.Utils.AddPlayerItem(new PlayerItemData(new ItemData(ItemTag.Closet2), 1), 1);
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("Menu");
            mMainState = MainState.Menu;
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Base.ResetNormalGameSpeed();
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (mMainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    ChangeState<ProcedureMain>(procedureOwner);
                    //«–ªªbgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //«–ªªbgm
                    break;
                case MainState.Menu:
                    break;
                case MainState.Outing:
                    //«–ªªbgm
                    break;
                case MainState.Change:
                    ChangeState<ProcedureInitMain>(procedureOwner);
                    break;
                case MainState.Guide:
                    ChangeState<ProcedureGuide>(procedureOwner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void MainStateEvent(object sender, GameEventArgs e)
        {
            MainStateEventArgs args = (MainStateEventArgs)e;
            mMainState = args.MainState;
        }
    }
}