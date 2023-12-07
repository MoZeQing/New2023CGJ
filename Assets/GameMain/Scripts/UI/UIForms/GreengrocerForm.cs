using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework.DataTable;
using GameMain;

namespace GameMain
{
    public class GreengrocerForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject itemPre;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        [SerializeField] private PurchaseForm purchaseForm;

        private List<ShopItem> mItems = new List<ShopItem>();
        private ItemData mItemData = new ItemData();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(OnExit);
            ShowItems();
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

        private void ShowItems()
        {
            ClearItems();
            DRItem[] items = GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows();
            foreach (DRItem item in items)
            {
                ItemData itemData = new ItemData(item);
                if (itemData.itemKind != ItemKind.Materials)
                    continue;
                GameObject go = Instantiate(itemPre, canvas);
                ShopItem shopItem = go.GetComponent<ShopItem>();
                shopItem.SetData(itemData);
                shopItem.SetTouch(OnTouch);
                mItems.Add(shopItem);
            }
        }
        private void OnClick(ShopItemData itemData)
        {
            purchaseForm.SetData(itemData);
            purchaseForm.gameObject.SetActive(true);
            purchaseForm.SetClick(UpdateItem);
        }
        private void UpdateItem()
        {
            ClearItems();
            ShowItems();
        }
        private void OnTouch(bool flag,ItemData itemData)
        {
            if (flag)
            {
                headerField.text = itemData.itemName;
                contentField.text = itemData.itemInfo;
            }
            else
            {
                headerField.text = string.Empty;
                contentField.text = string.Empty;
            }
        }
        private void ClearItems()
        {
            foreach (ShopItem item in mItems)
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
}
public class ShopItemData : ItemData
{
    
    public int itemNum;

    public ShopItemData() { }

    public ShopItemData(DRItem item)
        : base(item)
    {
        this.maxNum = item.MaxNum;
    }
    public ShopItemData(ItemTag itemTag)
        :base(itemTag)
    {
        IDataTable<DRItem> items = GameMain.GameEntry.DataTable.GetDataTable<DRItem>();
        DRItem item = items.GetDataRow((int)itemTag);
        this.maxNum = item.MaxNum;
    }
    public ShopItemData(ItemTag itemTag,int maxNum)
        : base(itemTag)
    {
        this.maxNum = maxNum;
    }
}

