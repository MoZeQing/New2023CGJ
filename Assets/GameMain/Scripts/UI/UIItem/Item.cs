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
        //mShopItemData = itemData;
        //this.gameObject.SetActive(true);
        //itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        //if (GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id) != null)
        //    inventoryText.text = string.Format("{0}", GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        //else
        //    inventoryText.text = string.Format("{0}", 0);
    }
    public override void SetData(DRItem itemData, Action<DRItem> onClick, Action<bool, DRItem> onTouch)
    {
        //mShopItemData = itemData;
        //this.gameObject.SetActive(true);
        //itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        //if (GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id) != null)
        //    inventoryText.text = string.Format("库存:{0}", GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        //else
        //    inventoryText.text = string.Format("库存:{0}", 0);
        //SetClick(onClick);
        //SetTouch(onTouch);
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
    None=-1,//全体
    Materials=0,//原材料
    Item=1,//道具
    Instrument=2,//器械
    Book=3,//书
    Cake=4,//蛋糕
    Music = 5,//音乐
    Dishes = 6,//菜肴
    Food = 7,//食物
    Clothes=8,//衣服
}
public enum ItemTag
{
    CoffeeBean=0,//咖啡豆
    Water=4,//水
    Milk=6,//牛奶
    Cream=8,//奶油
    ChocolateSyrup=9,//巧克力浆
    Sugar=11,//糖
    Ice=10,//冰
    Salt=12,//盐
    CondensedMilk=13,
    ManualGrinder=101,//手动研磨器
    Extractor=102,//电动萃取
    ElectricGrinder=103,//电动研磨器
    Heater=104,//加热器
    Syphon=105,//虹吸壶
    FrenchPress=106,//法压壶
    Kettle=107,//浸泡壶
    FilterBowl=108,//滤纸式滤杯
    Cup=109,//咖啡杯
    Stirrer=110,//搅拌器
    Food1 = 200,
    Food2 = 201,
    Food3 = 202,
    Food4 = 203,
    Food5 = 204,
    Food6 = 205,
    Food7 = 206,
    Item1 = 500,
    Item2 = 501,
    Item3 = 502,
    Item4 = 503,
    Item5 = 504,
    Item6 = 505,
    Item7 = 506,
    Item8 = 507,
    Item9 = 508,
    Item10 = 509,
    Item11 = 510,
    Item12 = 511,
    Closet1 = 1001,//破旧衣服
    Closet2 = 1002,//常服
    Closet3 = 1003,//女仆装
    Closet4 = 1004,//泳装
    Closet5 = 1005,//运动服
    Closet6 = 1006//晚礼服
}
