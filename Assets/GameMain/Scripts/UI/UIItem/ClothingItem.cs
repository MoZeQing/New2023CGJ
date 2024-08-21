using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMain
{
    public class ClothingItem : ShopItem
    {
        public override void SetData(DRItem itemData)
        {
            this.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>(itemData.ClothingPath);
            mShopItemData = itemData;
            priceText.text = string.Format("¼Û¸ñ:{0}", itemData.Price.ToString());
            itemText.text = itemData.Info;
            itemText.text = itemText.text.Replace("\\n", "\n");
            this.GetComponent<Button>().interactable = GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id) == null;
        }
    }
}
