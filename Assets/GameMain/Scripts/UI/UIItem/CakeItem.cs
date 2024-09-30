using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CakeItem : ShopItem
    {
        public override void SetData(DRItem itemData)
        {
            mShopItemData = itemData;
            this.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>(itemData.IconPath);
            priceText.text = string.Format("гд{0}", itemData.Price.ToString());
            itemText.text = itemData.Info;
            nameText.text = itemData.ItemName;
        }
    }
}

