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
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        [SerializeField] private PurchaseForm purchaseForm;
        [SerializeField] private List<ShopItem> mItems = new List<ShopItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private ItemData mItemData = new ItemData();
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Materials)
                    continue;
                dRItems.Add(item);
            }

            leftBtn.interactable = false;
            rightBtn.interactable = dRItems.Count > mItems.Count;     
            
            exitBtn.onClick.AddListener(OnExit);
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            index = 0;
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
            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                    mItems[i].SetData(dRItems[index], OnClick, OnTouch);
                else
                    mItems[i].Hide();
                index++;
            }
            leftBtn.interactable = index != 0;
            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
        }
        private void OnClick(DRItem itemData)
        {
            purchaseForm.SetData(itemData);
            purchaseForm.gameObject.SetActive(true);
            purchaseForm.SetClick(UpdateItem);
        }
        private void Right()
        {
            ShowItems();
        }

        private void Left()
        {
            index -= 2 * mItems.Count;
            ShowItems();
        }
        private void UpdateItem()
        {
            index -= mItems.Count;
            ShowItems();
        }
        private void OnTouch(bool flag, DRItem itemData)
        {
            if (flag)
            {
                headerField.text = itemData.Name;
                contentField.text = itemData.Info;
            }
            else
            {
                headerField.text = string.Empty;
                contentField.text = string.Empty;
            }
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

