using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework.DataTable;

namespace GameMain
{
    public class MusicHallForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject shopItemPre;

        private List<MusicItem> mItems = new List<MusicItem>();
        private List<MusicItemData> mItemDatas = new List<MusicItemData>();
        private MusicItemData mItemData = new MusicItemData();
        private int itemId;
        private int changeNum=0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(OnExit);
            ClearItems();
            /*if (GameEntry.Utils.Week == Week.Monday&&flag==true)
            {
                DrawLots();
                flag =false;
            }
            if(GameEntry.Utils.Week != Week.Monday)
            {
                flag = true;
            }*/
                ShowItems(mItemDatas);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }

        private void ShowItems(List<MusicItemData> itemDatas)
        {
            foreach (MusicItemData itemData in itemDatas)
            {
                if((int)itemData.itemTag== GameEntry.Utils.musicHallItemID)
                {
                    GameObject go = Instantiate(shopItemPre, canvas);
                    MusicItem item = go.GetComponent<MusicItem>();
                    item.SetData(itemData);
                    mItems.Add(item);
                }
               
            }
        }

        private void ClearItems()
        {
            foreach (MusicItem item in mItems)
            {
                Destroy(item.gameObject);
            }
            mItems.Clear();
        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.outingBefore = false;
            GameEntry.Dialog.StoryUpdate();
            OnGameStateChange();
        }

        private void OnGameStateChange()
        {
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
    public class MusicItemData : ItemData
    {
        public Sprite Poster;
        public string AbilityModifier;

        public MusicItemData(ItemTag itemTag)
        {
            this.itemTag = itemTag;
            IDataTable<DRItem> items = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem item = items.GetDataRow((int)itemTag);
            itemName = item.Name;
            itemTag = (ItemTag)item.Id;
            itemInfo = item.Info;
            price = item.Price;
            family = item.Family;
            hope = item.Hope;
            mood = item.Mood;
            love = item.Love;
            favor = item.Favor;
            equipable = item.Equipable;
            AbilityModifier = item.AMInfo;

        }
        public MusicItemData()
        {

        }
    }
}



