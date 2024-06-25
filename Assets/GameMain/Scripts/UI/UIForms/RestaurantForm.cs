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
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private PurchaseForm purchaseForm;
        [SerializeField] private List<CakeItem> mItems = new List<CakeItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private List<bool> mFires= new List<bool>();
        private DRItem dRItem;
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            mFires.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Cake)
                    continue;
                dRItems.Add(item);
                mFires.Add(false);
            }
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
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
        private void ShowItems()
        {
            leftBtn.interactable = (index != 0);

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetClick(OnClick);
                    mItems[i].GetComponent<Button>().interactable= !mFires[index];
                }
                else
                    mItems[i].Hide();
                index++;
            }

            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
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
        private void OnClick(DRItem itemData)
        {
            dRItem = itemData;
            mFires[dRItems.IndexOf(itemData)] = true;
            if (itemData.Price > GameEntry.Player.Money)
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的资金不足");
            else
                GameEntry.UI.OpenUIForm(UIFormId.OkTips, UpdateItem, "你确定要购买吗？");
        }
        private void UpdateItem()
        {
            GameEntry.Player.Money -= dRItem.Price;
            GameEntry.Cat.Favor += dRItem.Favor;
            GameEntry.Player.Energy += dRItem.Energy;

            index -= mItems.Count;

            ShowItems();
        }
        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
