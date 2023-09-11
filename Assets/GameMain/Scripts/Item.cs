using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Item : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int index;
    [SerializeField] private Image itemImg;
    [SerializeField] private Image usingImg;
    [SerializeField] private Text itemText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text itemInfoText;

    private ItemData mItemData;

    private Action<ItemData> mAction;
    private Action<bool,ItemData> mTouchAction;

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

    public void SetClick(Action<ItemData> action)
    {
        mAction = action;
    }

    public void SetTouch(Action<bool,ItemData> action)
    {
        mTouchAction = action;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mTouchAction(true, mItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mTouchAction(false, mItemData);
    }

    private void OnClick()
    {
        mAction(mItemData);
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

