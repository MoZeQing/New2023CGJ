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
    Food = 8
}
public enum ItemTag
{
    CoffeeBean=0,//¿§·È¶¹
    Water=4,//Ë®
    Milk=3,//Å£ÄÌ
    Cream=9,//ÄÌÓÍ
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
    Glass1 = 18,
    Glass2 = 19,
    Glass3 = 20,
    Dishes1 = 27,
    Dishes2 = 28,
    Dishes3 = 29,
    Dishes4 = 30,
    Dishes5 = 31,
    Food1 = 32,
    Food2 = 33,
    Food3 = 34,
    Food4 = 35,
    Food5 = 36,
    Closet1 =101,
    Closet2=102,
    Closet3=103,
}
