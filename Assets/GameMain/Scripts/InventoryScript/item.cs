using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item:MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Image itemImg;
    [SerializeField] private Text itemText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text itemInfoText;

    public ItemData itemData;

    private Action<ItemData> mAction;

    public void SetData(ItemData itemData)
    { 
        //itemText.text= itemData.itemName.ToString();
        priceText.text= itemData.price.ToString();
        //amountText.text=itemData.itemNum.ToString();
        itemInfoText.text=itemData.itemInfo.ToString();
    }

    public void SetClick(Action<ItemData> action)
    { 
        mAction= action;
    }
}
[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemImage;
    public int itemNum;
    public int price;
    public GameMain.FilterMode filterMode;
    [TextArea]
    public string itemInfo;
}
