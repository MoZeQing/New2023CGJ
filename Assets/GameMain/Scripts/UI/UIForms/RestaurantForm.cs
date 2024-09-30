using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class RestaurantForm : ShopForm
    {
        [Header("购买区域的东西")]
        [SerializeField] private ItemKind kind;
        [Header("优惠时间")]
        [SerializeField] private CakeItem cakeItem;
        [SerializeField] private int salePrice;
        [Header("特殊菜单")]
        [SerializeField] private ItemTag[] sales=new ItemTag[7];

        protected override void OnInitValue(object userData)
        {
            base.OnInitValue(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != kind)
                    continue;
                dRItems.Add(item);
            }

            //cakeItem.Interactable= salePrice > GameEntry.Player.Money;
            //DRItem dRItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow((int)sales[(int)GameEntry.Player.Week]);
            //cakeItem.SetData(dRItem);
            //cakeItem.SetClick(CakeItem_OnClick);
        }
        protected override void ShowItems()
        {
            leftBtn.interactable = index != 0;

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetClick(OnClick);
                    mItems[i].Interactable = !GameEntry.Utils.CheckDayPassFlag(((ItemTag)dRItems[index].Id).ToString());
                }
                else
                    mItems[i].Hide();
                index++;
            }
            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
        }
        protected override void OnConfirm(DRItem itemData)
        {
            GameEntry.Utils.AddFlag(((ItemTag)itemData.Id).ToString());
            base.OnConfirm(itemData);
        }
        protected override void UpdateItem()
        {
            //cakeItem.Interactable = salePrice < GameEntry.Player.Money && !GameEntry.Utils.CheckDayPassFlag(sales[(int)GameEntry.Player.Week].ToString());

            base.UpdateItem();
        }
        private void CakeItem_OnClick(DRItem dRItem)
        {
            cakeItem.Interactable = false;
            GameEntry.UI.OpenUIForm(UIFormId.OkTips,() => CakeItem_OnConfirm(dRItem),$"确定要购买{dRItem.Name}蛋糕吗？");
        }

        private void CakeItem_OnConfirm(DRItem dRItem)
        {
            GameEntry.Player.AddPlayerItem((ItemTag)dRItem.Id, 1);
            GameEntry.Utils.AddDayPassFlag(((ItemTag)dRItem.Id).ToString());
            GameEntry.Utils.RunEvent(dRItem.EventData);
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
