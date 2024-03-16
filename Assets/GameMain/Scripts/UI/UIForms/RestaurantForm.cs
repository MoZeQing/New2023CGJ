using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class RestaurantForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private PurchaseForm purchaseForm;
        [SerializeField] private List<ShopItem> mItems = new List<ShopItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Cake)
                    continue;
                dRItems.Add(item);
            }
            exitBtn.onClick.AddListener(OnExit);

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
                    mItems[i].SetData(dRItems[index]);
                else
                    mItems[i].Hide();
                index++;
            }
        }
        private void OnClick(DRItem itemData)
        {
            //purchaseForm.SetData(itemData);
            purchaseForm.gameObject.SetActive(true);
            purchaseForm.SetClick(UpdateItem);
        }
        private void UpdateItem()
        {
            index -= mItems.Count;
            ShowItems();
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
