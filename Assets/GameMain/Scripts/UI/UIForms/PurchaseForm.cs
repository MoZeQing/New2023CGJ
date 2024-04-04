using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameMain
{
    public class PurchaseForm : MonoBehaviour
    {
        [SerializeField] private Text purchaseNum;
        [SerializeField] private Button plusBtn;
        [SerializeField] private Button superPlusBtn;
        [SerializeField] private Button minusBtn;
        [SerializeField] private Button superMinusPlusBtn;
        [SerializeField] private Button exitPurchaseFormBtn;
        [SerializeField] private Button PurchaseFormBuyBtn;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject tips;

        private DRItem mShopItemData;
        private Action mAction;
        private int purchaseNumber;

        void Start()
        {
            exitPurchaseFormBtn.onClick.AddListener(ExitPurchaseForm);
            plusBtn.onClick.AddListener(Plus);
            superPlusBtn.onClick.AddListener(SuperPlus);
            minusBtn.onClick.AddListener(Minus);
            superMinusPlusBtn.onClick.AddListener(SuperMinus);
            PurchaseFormBuyBtn.onClick.AddListener(PurchaseComfirm); ;
            purchaseNumber = 1;
            purchaseNum.text = purchaseNumber.ToString();
        }
        void Update()
        {
            purchaseNum.text = purchaseNumber.ToString();
        }
        public void SetData(DRItem shopItemData)
        {
            mShopItemData = shopItemData;
            if (GameEntry.Utils.Money < mShopItemData.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(true);
                PurchaseFormBuyBtn.interactable = false;
                iconImage.sprite = Resources.Load<Sprite>(shopItemData.IconPath);
            }
        }
        public void SetClick(Action action)
        {
            mAction = action;
        }

        private void ExitPurchaseForm()
        {
            purchaseNum.text = 1.ToString();
            purchaseNumber = 1;
            this.gameObject.SetActive(false);
        }
        private void PurchaseComfirm()
        {
            if (GameEntry.Utils.Money >= mShopItemData.Price * purchaseNumber)
            {
                GameEntry.Utils.AddPlayerItem(new ShopItemData(mShopItemData), purchaseNumber);
                GameEntry.Utils.Money -= mShopItemData.Price * purchaseNumber;
            }
            purchaseNum.text = 1.ToString();
            purchaseNumber = 1;
            mAction();
            this.gameObject.SetActive(false);
        }
        private void Plus()
        {
            if (purchaseNumber >= 0 && purchaseNumber <= 98)
            {
                purchaseNumber++;
            }
            if (purchaseNumber > 99)
            {
                purchaseNumber = 99;
            }
            if (GameEntry.Utils.Money < mShopItemData.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(true);
                PurchaseFormBuyBtn.interactable = false;
            }
        }
        private void SuperPlus()
        {
            if (purchaseNumber >= 0 && purchaseNumber <= 98)
            {
                purchaseNumber = purchaseNumber + 5;
            }
            if (purchaseNumber > 99)
            {
                purchaseNumber = 99;
            }
            if (GameEntry.Utils.Money < mShopItemData.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(true);
                PurchaseFormBuyBtn.interactable = false;
            }
        }
        private void Minus()
        {
            if (purchaseNumber > 0)
            {
                purchaseNumber--;
            }
            if (purchaseNumber < 0)
            {
                purchaseNumber = 0;
            }
            if (GameEntry.Utils.Money >= mShopItemData.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(false);
                PurchaseFormBuyBtn.interactable = true;
            }
        }
        private void SuperMinus()
        {
            if (purchaseNumber > 0)
            {
                purchaseNumber = purchaseNumber - 5;
            }
            if (purchaseNumber < 0)
            {
                purchaseNumber = 0;
            }
            if (GameEntry.Utils.Money >= mShopItemData.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(false);
                PurchaseFormBuyBtn.interactable = true;
            }
        }


    }
}
