using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class GreengrocerForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject shopItemPre;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        [SerializeField] private PurchaseForm purchaseForm;

        private List<Item> mItems = new List<Item>();
        private List<ShopItemData> mItemDatas = new List<ShopItemData>();
        private ShopItemData mItemData = new ShopItemData();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mItemDatas = GameEntry.Utils.greengrocerItemDatas;
            exitBtn.onClick.AddListener(OnExit);
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

        private void ShowItems(List<ShopItemData> itemDatas)
        {
            foreach (ShopItemData itemData in itemDatas)
            {
                GameObject go = Instantiate(shopItemPre, canvas);
                ShopItem item = go.GetComponent<ShopItem>();
                item.SetData(itemData);
                item.SetClick(OnClick);
                item.SetTouch(OnTouch);
            }
        }
        private void OnClick(ShopItemData itemData)
        {
            purchaseForm.SetData(itemData);
            purchaseForm.gameObject.SetActive(true);
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
            foreach (Item item in mItems)
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
    public int maxNum;
    public int itemNum;
}

