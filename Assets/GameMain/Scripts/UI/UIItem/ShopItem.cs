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

        void Start()
        {

        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        public virtual void SetData(DRItem itemData)
        {
            mShopItemData = itemData;
            this.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
            priceText.text = string.Format("гд{0}",itemData.Price.ToString());
            if(GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id)!=null)
                inventoryText.text =  GameEntry.Utils.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString(); 
            else
                inventoryText.text = "0";
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
