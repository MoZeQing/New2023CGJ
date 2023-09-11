using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CupboradForm : UIFormLogic
    {
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private GameObject itemItem;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle allToggle;
        [SerializeField] private Toggle materialsToggle;
        [SerializeField] private Toggle itemToggle;
        [SerializeField] private Toggle onceItemToggle;

        private List<Item> mItems = new List<Item>();
        private List<ItemData> mItemDatas=new List<ItemData>();
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mItemDatas = GameEntry.Utils.PlayerData.items;
            allToggle.isOn = true;
            allToggle.onValueChanged.AddListener(OnAllChange);
            materialsToggle.onValueChanged.AddListener(OnMaterialsChange);
            itemToggle.onValueChanged.AddListener(OnItemChange);
            onceItemToggle.onValueChanged.AddListener(OnOnceItemChange);
            exitBtn.onClick.AddListener(OnExit);
            ShowItems();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            allToggle.onValueChanged.RemoveAllListeners();
            materialsToggle.onValueChanged.RemoveAllListeners();
            itemToggle.onValueChanged.RemoveAllListeners();
            onceItemToggle.onValueChanged.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
            ClearItems();
        }

        public void ShowItems()
        {
            ShowItems(mItemDatas);
        }

        public void ShowItems(List<ItemData> itemDatas)
        {
            foreach (ItemData itemData in itemDatas)
            {
                GameObject go = Instantiate(itemItem, mCanvas);
                Item item =go.GetComponent<Item>();
                item.SetData(itemData);
                item.SetClick(OnClick);
                mItems.Add(item);
            }
        }

        public void ClearItems()
        {
            foreach (Item item in mItems)
            { 
                GameObject.Destroy(item.gameObject);
            }
            mItems.Clear();
        }

        private void OnClick(ItemData itemData)
        { 
            //弹出一个确认弹窗
        }

        private void OnAllChange(bool value)
        {
            OnFilterChange(FilterMode.All);
        }

        private void OnMaterialsChange(bool value) 
        {
            OnFilterChange(FilterMode.Materials);
        }

        private void OnItemChange(bool value)
        {
            OnFilterChange(FilterMode.Item);
        }

        private void OnOnceItemChange(bool value)
        {
            OnFilterChange(FilterMode.OnceItem);
        }

        private void OnFilterChange(FilterMode filterMode)
        { 
            ClearItems();
            List<ItemData> newItems=new List<ItemData>();
            foreach (ItemData itemData in mItemDatas)
            {
                if (itemData.filterMode == filterMode)
                {
                    newItems.Add(itemData);
                    continue;
                }
                if (filterMode==FilterMode.All)
                {
                    newItems.Add(itemData);
                    continue;
                }
            }
            ClearItems();
            ShowItems(newItems);
        }

        public void OnExit()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }

    public enum FilterMode
    { 
        All,
        Materials,
        Item,
        OnceItem
    }
}