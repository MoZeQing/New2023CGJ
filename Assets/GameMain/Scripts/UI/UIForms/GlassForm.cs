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
        [SerializeField] private GameObject glassItemPre;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;

        private List<ShopItemData> mShopItemDatas = new List<ShopItemData>();
        private List<GlassItem> mItems = new List<GlassItem>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mShopItemDatas = GameEntry.Utils.glassItemDatas;
            exitBtn.onClick.AddListener(OnExit);
            ClearItems();
            ShowItems(mShopItemDatas);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void ShowItems(List<ShopItemData> itemDatas)
        {
            foreach (ShopItemData itemData in itemDatas)
            {
                GameObject go = Instantiate(glassItemPre, canvas);
                GlassItem item = go.GetComponent<GlassItem>();
                item.SetData(itemData);
                item.SetTouch(OnTouch);
                mItems.Add(item);
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
            foreach (GlassItem item in mItems)
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
