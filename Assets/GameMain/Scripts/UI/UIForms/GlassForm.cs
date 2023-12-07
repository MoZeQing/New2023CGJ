using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class GlassForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject itemPre;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;

        private List<ShopItem> mItems = new List<ShopItem>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);           
            exitBtn.onClick.AddListener(OnExit);
            ClearItems();
            ShowItems();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void ShowItems()
        {
            DRItem[] items = GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows();
            foreach (DRItem item in items)
            {
                ItemData itemData = new ItemData(item);
                if (itemData.itemKind != ItemKind.Instrument)
                    continue;
                GameObject go = Instantiate(itemPre, canvas);
                ShopItem shopItem = go.GetComponent<ShopItem>();
                shopItem.SetData(itemData);
                shopItem.SetTouch(OnTouch);
                mItems.Add(shopItem);
            }
        }
        private void OnTouch(bool flag, ItemData itemData)
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
