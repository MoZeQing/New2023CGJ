using GameFramework.DataTable;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameFramework.Event;

namespace GameMain
{
    public class ShopForm : BaseForm
    {
        [Header("购买区域")]
        [SerializeField] protected Button exitBtn;
        [SerializeField] protected Button leftBtn;
        [SerializeField] protected Button rightBtn;
        [SerializeField] protected Text moneyText;
        [SerializeField] protected Text pageText;
        [SerializeField] protected List<ShopItem> mItems = new List<ShopItem>();

        protected List<DRItem> dRItems = new List<DRItem>();
        protected int index = 0;

        /// <summary>
        /// 初始化数值
        /// </summary>
        /// <param name="userData"></param>
        protected virtual void OnInitValue(object userData)
        { 
            
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            leftBtn.interactable = false;
            rightBtn.interactable = dRItems.Count > mItems.Count;

            exitBtn?.onClick.AddListener(OnExit);
            leftBtn?.onClick.AddListener(Left);
            rightBtn?.onClick.AddListener(Right);

            OnInitValue(userData);
            UpdateItem();
            index = 0;
            ShowItems();

            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, OnDialogEvent);
        }
        private void OnDialogEvent(object sender, GameEventArgs e)
        {
            DialogEventArgs args = (DialogEventArgs)e;
            if (!args.InDialog)
            {
                DRUIForms dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);

                if (dRUIForms.OpenSound != 0)
                {
                    GameEntry.Sound.PlaySound(dRUIForms.OpenSound);
                }
            }
        }
        protected virtual void ShowItems()
        {
            leftBtn.interactable = index != 0;

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetClick(OnClick);
                }
                else
                    mItems[i].Hide();
                index++;
            }
            rightBtn.interactable = index < dRItems.Count;
            if (pageText != null)
                pageText.text = (index / mItems.Count).ToString();
        }
        protected virtual void OnClick(DRItem itemData)
        {
            if (GameEntry.Player.Money < itemData.Price)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的金钱不足");
                return;
            }
            GameEntry.UI.OpenUIForm(UIFormId.OkTips,()=> OnConfirm(itemData), "你确定要购买吗？");
        }
        protected virtual void Right()
        {
            ShowItems();
        }

        protected virtual void Left()
        {
            index -= 2 * mItems.Count;
            ShowItems();
        }
        protected virtual void OnConfirm(DRItem itemData)
        {
            GameEntry.Player.Money -= itemData.Price;
            GameEntry.Player.AddPlayerItem((ItemTag)itemData.Id, 1);
            GameEntry.Utils.RunEvent(itemData.EventData);
            UpdateItem();
        }
        protected virtual void UpdateItem()
        {
            moneyText.text = $"{GameEntry.Player.Money}";
            index = 0;
            ShowItems();
        }

        protected virtual void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.MapForm);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.Event.FireNow(this, OutEventArgs.Create(OutingSceneState.Home));
            GameEntry.UI.CloseUIForm(this.UIForm);
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
            : base(itemTag)
        {
            IDataTable<DRItem> items = GameMain.GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem item = items.GetDataRow((int)itemTag);
            this.maxNum = item.MaxNum;
        }
        public ShopItemData(ItemTag itemTag, int maxNum)
            : base(itemTag)
        {
            this.maxNum = maxNum;
        }
    }
}
