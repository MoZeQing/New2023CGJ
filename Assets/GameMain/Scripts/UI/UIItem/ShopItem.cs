using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace GameMain
{
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Text nameText; 
        [SerializeField] private Text priceText;
        [SerializeField] private Text inventoryText;
        [SerializeField] private Button subBtn;
        [SerializeField] private Button purchaseButton;

        private ShopItemData mShopItemData;
        private Action<ShopItemData> mAction;
        private Action<bool, ShopItemData> mTouchAction;

        void Start()
        {
            purchaseButton.onClick.AddListener(()=>mAction(mShopItemData));
        }

        public void SetData(ShopItemData itemData)
        {
            mShopItemData = itemData;
            nameText.text = itemData.itemName.ToString();
            priceText.text = string.Format("¼Û¸ñ:{0}",itemData.price.ToString());
            if(GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag)!=null)
                inventoryText.text = string.Format("¿â´æ:{0}", GameEntry.Utils.GetPlayerItem(mShopItemData.itemTag).itemNum.ToString()); 
            else
                inventoryText.text = string.Format("¿â´æ:{0}", 0);
            if (GameEntry.Utils.GetPlayerItem(itemData.itemTag) != null)
            {
                if (GameEntry.Utils.GetPlayerItem(itemData.itemTag).itemNum >= itemData.maxNum)
                {
                    purchaseButton.interactable = false;
                }
                else
                {
                    purchaseButton.interactable = true;
                }
            }
            else
            {
                purchaseButton.interactable = true;
            }
        }

        public virtual void SetClick(Action<ShopItemData> action)
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
}
