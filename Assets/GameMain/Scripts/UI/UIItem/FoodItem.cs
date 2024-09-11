using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image FoodImg;
    [SerializeField] private Text FoodText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text warningPriceText;
    [SerializeField] private Button okBtn;

    private ShopItemData mShopItemData;
    private Action<bool, ShopItemData> mTouchAction;
    private Action mAction;

    private void Start()
    {
        okBtn.onClick.AddListener(OnClick);
    }
    private void Update()
    {

        if (GameEntry.Player.Money >= mShopItemData.price)
        {
            okBtn.interactable = true;
            warningPriceText.gameObject.SetActive(false);
        }
        if (GameEntry.Player.Money < mShopItemData.price)
        {
            okBtn.interactable = false;
            warningPriceText.gameObject.SetActive(true);
        }
    }

    public void SetData(ShopItemData shopItemData)
    {
        mShopItemData = shopItemData;
        FoodText.text = shopItemData.itemName;
        priceText.text = shopItemData.price.ToString();
    }

    private void OnClick()
    {
        if (GameEntry.Player.Money >= mShopItemData.price)
        {
            mAction?.Invoke();
            GameEntry.Player.Money -= mShopItemData.price;
            GameEntry.Dialog.PlayStory(mShopItemData.itemTag.ToString());
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
