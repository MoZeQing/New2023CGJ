using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using GameFramework.DataTable;
using GameMain;

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
        itemText.text = itemData.itemName.ToString();
        priceText.text = string.Format("¼Û¸ñ:{0}", itemData.price.ToString());
        amountText.text = string.Format("¿â´æ:{0}", itemData.itemNum.ToString());
        if (mItemData.equipable)
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
    public ItemData(ItemTag itemTag)
    {
        this.itemTag = itemTag;
        IDataTable<DRItem> items = GameEntry.DataTable.GetDataTable<DRItem>();
        DRItem item = items.GetDataRow((int)itemTag);
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemInfo = item.Info;
        price = item.Price;
        family = item.Family;
        hope = item.Hope;
        mood = item.Mood;
        love = item.Love;
        favor = item.Favor;
        ability = item.Ap;
        maxNum = item.MaxNum;
        filterMode = (GameMain.FilterMode)item.FilterMode;
        equipable = item.Equipable;
    }
}

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
        itemName = itemData.itemName;
        price = itemData.price;
        filterMode = itemData.filterMode;
        equipable = itemData.equipable;
        itemInfo = itemData.itemInfo;     
        itemNum = num;
    }
}

public enum ItemKind
{ 
    Materials=0,
    Item=1,
    Instrument=2,
    Book=3,
    Cake=4,
    Music = 5,
    Glass = 6,
    Dishes = 7,
    Food = 8,
    Clothes=9,
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
    ManualGrinder=16,//ÊÖ¶¯ÑÐÄ¥Æ÷
    Extractor=17,//µç¶¯ÝÍÈ¡
    ElectricGrinder=18,//µç¶¯ÑÐÄ¥Æ÷
    Heater=19,//¼ÓÈÈÆ÷
    Syphon=20,//ºçÎüºø
    FrenchPress=21,//·¨Ñ¹ºø
    Kettle=22,//½þÅÝºø
    FilterBowl=23,//ÂËÖ½Ê½ÂË±­
    Cup=24,//¿§·È±­
    Stirrer=25,//½Á°èÆ÷
    Espresso,//Å¨Ëõ¿§·È
    HotCafeAmericano,//ÈÈÃÀÊ½
    IceCafeAmericano,//ÀäÃÀÊ½
    HotLatte,//ÈÈÄÃÌú
    IceLatte,//±ùÄÃÌú
    HotMocha,//ÈÈÄ¦¿¨
    IceMocha,//ÀäÄ¦¿¨
    Kapuziner,//¿¨²¼ÆæÅµ
    FlatWhite,//°Ä°×
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
    Closet1 = 101,
    Closet2 = 102,
    Closet3 = 103,
    Closet4 = 104,
}
