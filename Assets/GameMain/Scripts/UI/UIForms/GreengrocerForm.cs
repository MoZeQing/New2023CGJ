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
        [SerializeField] private GameObject moneyTips;

        private Dictionary<string, int> shopItems = new Dictionary<string, int>();
        private Dictionary<ItemData, int> shopItemsData = new Dictionary<ItemData, int>();
        private List<Item> items = new List<Item>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(OnExit);
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

        private void ShowItems(List<ItemData> itemDatas)
        {
            foreach (ItemData itemData in itemDatas)
            {
                GameObject go = Instantiate(shopItemPre, canvas);
                Item item = go.GetComponent<Item>();
                item.SetData(itemData);
                item.SetClick(OnClick);
                item.SetTouch(OnTouch);
            }
        }
        private void OnClick(ItemData itemData)
        {
            if (GameEntry.Utils.Money < itemData.price)
            {
                moneyTips.gameObject.SetActive(true);
            }
            else
            {
                GameEntry.Utils.Money -= itemData.price;

            }
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
            foreach (Item item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
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

