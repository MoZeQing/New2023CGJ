using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlassItem : ShopItem
{
    public override void SetData(DRItem itemData)
    {
        this.gameObject.SetActive(true);
        itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        priceText.text = string.Format("¼Û¸ñ:\n{0}", itemData.Price.ToString());
        priceText.text = priceText.text.Replace("\\n", "\n");
        itemText.text = itemData.Name;
        inventoryText.text = itemData.Info;
        inventoryText.text=inventoryText.text.Replace("\\n", "\n");
        this.GetComponent<Button>().interactable = GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id) == null;
    }
}
