using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private ItemData mItemData;

    private Action mAction;

    public void SetData(ItemData itemData)
    {
        mItemData = itemData;
        //itemText.text= itemData.itemName.ToString();
        priceText.text = itemData.price.ToString();
        //amountText.text=itemData.itemNum.ToString();
        itemInfoText.text = itemData.itemInfo.ToString();
        if(mItemData.equipable)
            usingImg.gameObject.SetActive(mItemData.equiping);
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetClick(Action action)
    {
        mAction = action;
    }

    private void OnClick()
    {
        mAction();
        if (mItemData.equipable)
        {
            usingImg.gameObject.SetActive(!usingImg.gameObject.activeSelf);
            mItemData.equiping = usingImg.gameObject.activeSelf;
        }
        //·¢ËÍÏûÏ¢
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
    public bool equiping;
    public bool equipable;
    [TextArea]
    public string itemInfo;
}

