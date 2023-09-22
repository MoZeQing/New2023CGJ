using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlassItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image glassImg;
    [SerializeField] private Text glassText;
    [SerializeField] private Text priceText;
    [SerializeField] private Button okBtn;

    private ShopItemData mShopItemData;
    private Action<bool, ShopItemData> mTouchAction;
    private Action mAction;

    private void Start()
    {
        okBtn.onClick.AddListener(OnClick);
    }

    public void SetData(ShopItemData shopItemData)
    {
        mShopItemData = shopItemData;
        glassText.text = shopItemData.itemName;
        priceText.text = shopItemData.price.ToString();
        //okBtn.interactable = GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag) == null;
    }

    private void OnClick()
    {
        if (GameEntry.Utils.Money >= mShopItemData.price)
        {
            GameEntry.Utils.Money -= mShopItemData.price;
            GameEntry.Utils.AddPlayerItem(mShopItemData, 1);
        }
    }

    public void SetClick(Action action)
    {
        mAction = action;
    }

    public virtual void SetTouch(Action<bool, ShopItemData> action)
    {
        mTouchAction = action;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        mTouchAction(true, mShopItemData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        mTouchAction(false, mShopItemData);
    }
}
