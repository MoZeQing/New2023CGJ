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
        [SerializeField] protected Button itemBtn;
        [SerializeField] protected Text itemText;
        [SerializeField] protected Text priceText;
        [SerializeField] protected Text inventoryText;

        protected DRItem mShopItemData;
        protected Action<DRItem> mAction;
        protected Action<bool, DRItem> mTouchAction;

        public bool Interactable
        {
            get => itemBtn.interactable;
            set=> itemBtn.interactable = value;
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        public virtual void SetData(DRItem itemData)
        {
            mShopItemData = itemData;
            this.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>(itemData.IconPath);
            itemText.text = itemData.Info;
            priceText.text = $"{itemData.Price}";
            if(GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id)!=null)
                itemBtn.interactable = (itemData.MaxNum > GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum);
            else 
                itemBtn.interactable = true;
            //if(GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id)!=null)
            //    inventoryText.text =  GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString(); 
            //else
            //    inventoryText.text = "0";
        }
        public virtual void SetData(DRItem itemData, Action<DRItem> click, Action<bool, DRItem> touch)
        {
            SetData(itemData);
            SetClick(click);
            SetTouch(touch);
        }

        public virtual void SetClick(Action<DRItem> action)
        {
            mAction = action;
            itemBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            itemBtn.GetComponent<Button>().onClick.AddListener(() => mAction(mShopItemData));
        }

        public virtual void SetTouch(Action<bool, DRItem> action)
        {
            mTouchAction = action;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if(mTouchAction!=null)
                mTouchAction(true, mShopItemData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if(mTouchAction!=null)
                mTouchAction(false, mShopItemData);
        }
    }
}
