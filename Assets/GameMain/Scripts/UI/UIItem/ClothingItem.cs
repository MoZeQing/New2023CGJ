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
            itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
            priceText.text = string.Format("¼Û¸ñ:{0}", itemData.Price.ToString());
            this.GetComponent<Button>().interactable = GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id) == null;
        }
    }
}
