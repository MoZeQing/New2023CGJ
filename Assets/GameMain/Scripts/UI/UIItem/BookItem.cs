using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image bookImg;
    [SerializeField] private Text bookText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text warningPriceText;
    [SerializeField] private Text warningNumText;
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
        if (GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag) != null)
        {
            if (GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag).itemNum >= mShopItemData.maxNum)
            {
                okBtn.interactable = false;
                warningNumText.gameObject.SetActive(true);
            }
        }
        if (warningNumText.gameObject.activeSelf == false)
        {
            if (GameEntry.Utils.Money >= mShopItemData.price)
            {
                okBtn.interactable = true;
                warningPriceText.gameObject.SetActive(false);
            }
            if (GameEntry.Utils.Money < mShopItemData.price)
            {
                okBtn.interactable = false;
                warningPriceText.gameObject.SetActive(true);
            }
        }
    }

    public void SetData(ShopItemData shopItemData)
    { 
        mShopItemData= shopItemData;
        bookText.text = shopItemData.itemName;
        priceText.text = shopItemData.price.ToString();
        okBtn.interactable=!(GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag) == null);
    }

    private void OnClick()
    {
        if (GameEntry.Utils.Money >= mShopItemData.price)
        {
            mAction();
            GameEntry.Utils.Money -= mShopItemData.price;
            GameEntry.Utils.Favor += mShopItemData.favor;
            GameEntry.Utils.Love += mShopItemData.love;
            GameEntry.Utils.Family += mShopItemData.family;
            GameEntry.Utils.Mood += mShopItemData.mood;
            GameEntry.Utils.Hope += mShopItemData.hope;
            GameEntry.Utils.Ability += mShopItemData.ability;
            GameEntry.Utils.AddPlayerItem(mShopItemData,1);
            GameEntry.Dialog.PlayStory(mShopItemData.itemTag.ToString());
        }
    }

    public void SetClick(Action action)
    { 
        mAction= action;
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
