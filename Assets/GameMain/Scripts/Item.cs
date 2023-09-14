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
    public string itemName;
    public int price;
    public GameMain.FilterMode filterMode;
    public bool equipable;
    [TextArea]
    public string itemInfo;
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

public enum ItemTag
{ 

}
