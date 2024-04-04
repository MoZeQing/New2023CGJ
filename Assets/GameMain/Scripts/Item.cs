using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameFramework.DataTable;
using GameMain;

[System.Serializable]
public class Item : ShopItem
{
    public override void SetData(DRItem itemData)
    {
        mShopItemData = itemData;
        this.gameObject.SetActive(true);
        itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        if (GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id) != null)
            inventoryText.text = string.Format("{0}", GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        else
            inventoryText.text = string.Format("{0}", 0);
    }
    public override void SetData(DRItem itemData, Action<DRItem> onClick, Action<bool, DRItem> onTouch)
    {
        mShopItemData = itemData;
        this.gameObject.SetActive(true);
        itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        if (GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id) != null)
            inventoryText.text = string.Format("¿â´æ:{0}", GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        else
            inventoryText.text = string.Format("¿â´æ:{0}", 0);
        SetClick(onClick);
        SetTouch(onTouch);
    }
}
[System.Serializable]
public class ItemData
{
    public ItemTag itemTag;
    public ItemKind itemKind;
    public string itemName;
    public string itemImgPath;
    public int price;
    public bool equipable;
    public int family;
    public int hope;
    public int mood;
    public int love;
    public int favor;
    public int ability;
    public int maxNum;
    [TextArea]
    public string itemInfo;

    public ItemData() { }

    public ItemData(DRItem item)
    {
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemKind = (ItemKind)item.Kind;
        itemInfo = item.Info;
        price = item.Price;
        family = item.Family;
        hope = item.Hope;
        mood = item.Mood;
        love = item.Love;
        favor = item.Favor;
        ability = item.Ap;
        maxNum = item.MaxNum;
        equipable = item.Equipable;
        itemImgPath = item.ImagePath;
    }
    public ItemData(ItemTag itemTag)
    {
        this.itemTag = itemTag;
        IDataTable<DRItem> items = GameEntry.DataTable.GetDataTable<DRItem>();
        DRItem item = items.GetDataRow((int)itemTag);
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemKind = (ItemKind)item.Kind;
        itemInfo = item.Info;
        price = item.Price;
        family = item.Family;
        hope = item.Hope;
        mood = item.Mood;
        love = item.Love;
        favor = item.Favor;
        ability = item.Ap;
        maxNum = item.MaxNum;
        equipable = item.Equipable;
    }
}
[System.Serializable]
public class PlayerItemData : ItemData
{
    public int itemNum;
    public bool equiping;

    public PlayerItemData() { }
    public PlayerItemData(ItemTag itemTag, int num)
        :base(itemTag) 
    {
        this.itemNum = num;
    }

    public PlayerItemData(ItemData itemData, int num)
    {
        itemTag = itemData.itemTag;
        itemKind = itemData.itemKind;
        itemName = itemData.itemName;
        price = itemData.price;
        equipable = itemData.equipable;
        itemInfo = itemData.itemInfo;     
        itemNum = num;
        itemImgPath = itemData.itemImgPath;
    }
}

public enum ItemKind
{ 
    None=-1,//È«Ìå
    Materials=0,//Ô­²ÄÁÏ
    Item=1,//µÀ¾ß
    Instrument=2,//Æ÷Ðµ
    Book=3,//Êé
    Cake=4,//µ°¸â
    Music = 5,//ÒôÀÖ
    Dishes = 6,//²ËëÈ
    Food = 7,//Ê³Îï
    Clothes=8,//ÒÂ·þ
}
public enum ItemTag
{
    CoffeeBean=0,//¿§·È¶¹
    Water=4,//Ë®
    Milk=6,//Å£ÄÌ
    Cream=8,//ÄÌÓÍ
    ChocolateSyrup=9,//ÇÉ¿ËÁ¦½¬
    Sugar=11,//ÌÇ
    Ice=10,//±ù
    Salt=12,//ÑÎ
    CondensedMilk=13,
    ManualGrinder=101,//ÊÖ¶¯ÑÐÄ¥Æ÷
    Extractor=102,//µç¶¯ÝÍÈ¡
    ElectricGrinder=103,//µç¶¯ÑÐÄ¥Æ÷
    Heater=104,//¼ÓÈÈÆ÷
    Syphon=105,//ºçÎüºø
    FrenchPress=106,//·¨Ñ¹ºø
    Kettle=107,//½þÅÝºø
    FilterBowl=108,//ÂËÖ½Ê½ÂË±­
    Cup=109,//¿§·È±­
    Stirrer=110,//½Á°èÆ÷
    Book1 =30,
    Book2=31,
    Book3=32,
    Book4=33,
    Book5=34,
    Music1 = 40,
    Music2 = 41,
    Music3 = 42,
    Dishes1 = 50,
    Dishes2 = 51,
    Dishes3 = 52,
    Dishes4 = 53,
    Dishes5 = 54,
    Food1 = 60,
    Food2 = 61,
    Food3 = 62,
    Food4 = 63,
    Food5 = 64,
    Closet1 = 1001,
    Closet2 = 1002,
    Closet3 = 1003,
    Closet4 = 1004,
    Closet5=1005,
}
