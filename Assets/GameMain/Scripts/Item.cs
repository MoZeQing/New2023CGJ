using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Image itemImg;
    [SerializeField] private Image usingImg;
    [SerializeField] private Text itemText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text itemInfoText;

    private PlayerItemData mItemData;
    private Action<PlayerItemData> mAction;

    public void SetData(PlayerItemData itemData)
    {
        mItemData = itemData;
        //itemText.text= itemData.itemName.ToString();
        priceText.text = itemData.price.ToString();
        amountText.text = itemData.itemNum.ToString();
        itemInfoText.text = itemData.itemInfo.ToString();
        if(mItemData.equipable)
            //usingImg.gameObject.SetActive(mItemData.equiping);
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetClick(Action<ItemData> action)
    {
        mAction = action;
    }

    private void OnClick()
    {
        mAction(mItemData);
        if (mItemData.equipable)
        {
            usingImg.gameObject.SetActive(!usingImg.gameObject.activeSelf);
            //mItemData.equiping = usingImg.gameObject.activeSelf;
        }
        //·¢ËÍÏûÏ¢
    }
}
[System.Serializable]
public class ItemData
{
    public ItemTag itemTag;
    public ItemKind itemKind;
    public string itemName;
    public int price;
    public GameMain.FilterMode filterMode;
    public bool equipable;
    [TextArea]
    public string itemInfo;

    public ItemData() { }
    public ItemData(ItemTag itemTag)
    {
        this.itemTag = itemTag;
    }
}

public class PlayerItemData : ItemData
{
    public int itemNum;
    public bool equiping;

    public PlayerItemData() { }
    public PlayerItemData(ItemData itemData, int num)
    {
        this.itemTag = itemData.itemTag;
        this.itemName = itemData.itemName;
        this.price = itemData.price;
        this.filterMode = itemData.filterMode;
        this.equipable = itemData.equipable;
        this.itemInfo = itemData.itemInfo;
        this.itemNum = num;
    }
}
public enum ItemKind
{ 
    Materials=0,
    Item=1,
    Instrument=2,
    Book=3,
    Cake=4,
    Music = 5
}
public enum ItemTag
{
    CoffeeBean=0,//¿§·È¶¹
    Water=4,//Ë®
    Milk=3,//Å£ÄÌ
    Cream=4,//ÄÌÓÍ
    ChocolateSyrup=5,//ÇÉ¿ËÁ¦½¬
    Sugar=6,//ÌÇ
    Ice=7,//±ù
    Salt=8,//ÑÎ
    Book1=10,
    Book2=11,
    Book3=12,
    Book4=13,
    Book5=14,
    Music1 = 15,
    Music2 = 16,
    Music3 = 17,
    Closet1=101,
    Closet2=102,
    Closet3=103,
}
