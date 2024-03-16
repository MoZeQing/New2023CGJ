using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditor;
using System.Reflection;

namespace GameMain
{
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected Text nameText;
        [SerializeField] protected Text priceText;
        [SerializeField] protected Text inventoryText;

        protected ItemData mShopItemData;
        protected Action<ItemData> mAction;
        protected Action<bool, ItemData> mTouchAction;

        void Start()
        {

        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        public virtual void SetData(DRItem itemData)
        {
            this.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
            priceText.text = string.Format("¼Û¸ñ:{0}",itemData.Price.ToString());
            if(GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id)!=null)
                inventoryText.text = string.Format("¿â´æ:{0}", GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString()); 
            else
                inventoryText.text = string.Format("¿â´æ:{0}", 0);
        }

        public virtual void SetClick(Action<ItemData> action)
        {
            mAction = action;
            this.GetComponent<Button>().onClick.AddListener(() => mAction(mShopItemData));
        }

        public virtual void SetTouch(Action<bool, ItemData> action)
        {
            mTouchAction = action;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            //mTouchAction(true, mShopItemData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            //mTouchAction(false, mShopItemData);
        }
    }
}
