using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameMain
{
    public class PurchaseForm : BaseForm
    {
        [SerializeField] private Text purchaseNum;
        [SerializeField] private Text purchaseTotal;
        [SerializeField] private Button plusBtn;
        [SerializeField] private Button superPlusBtn;
        [SerializeField] private Button minusBtn;
        [SerializeField] private Button superMinusPlusBtn;
        [SerializeField] private Button exitPurchaseFormBtn;
        [SerializeField] private Button PurchaseFormBuyBtn;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject tips;

        private DRItem mDRItem;
        private Action mAction;
        private int purchaseNumber;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitPurchaseFormBtn.onClick.AddListener(ExitPurchaseForm);
            plusBtn.onClick.AddListener(Plus);
            superPlusBtn.onClick.AddListener(SuperPlus);
            minusBtn.onClick.AddListener(Minus);
            superMinusPlusBtn.onClick.AddListener(SuperMinus);
            PurchaseFormBuyBtn.onClick.AddListener(PurchaseComfirm); ;
            purchaseNumber = 1;
            purchaseNum.text = purchaseNumber.ToString();
            mDRItem = userData as DRItem;
            Check();
            mAction = BaseFormData.Action;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            purchaseNum.text = "X" + purchaseNumber.ToString();
            purchaseTotal.text = string.Format("»¨Ïú£º{0}", purchaseNumber * mDRItem.Price);
        }

        private void ExitPurchaseForm()
        {
            purchaseNum.text = 1.ToString();
            purchaseNumber = 1;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
        private void Check()
        {
            if (GameEntry.Player.Money < mDRItem.Price * purchaseNumber)
            {
                tips.gameObject.SetActive(true);
                PurchaseFormBuyBtn.interactable = false;
            }
        }
        private void PurchaseComfirm()
        {
            if (GameEntry.Player.Money >= mDRItem.Price * purchaseNumber)
            {
                GameEntry.Player.AddPlayerItem(new ShopItemData(mDRItem), purchaseNumber);
                GameEntry.Player.Money -= mDRItem.Price * purchaseNumber;
            }
            mAction?.Invoke();
            GameEntry.UI.CloseUIForm(this.UIForm);
            Check();
        }
        private void Plus()
        {
            purchaseNumber++;
            purchaseNumber = Mathf.Min(purchaseNumber,99);

        }
        private void SuperPlus()
        {
            purchaseNumber += 5;
            purchaseNumber = Mathf.Min(purchaseNumber, 99);
            Check();
        }
        private void Minus()
        {
            purchaseNumber--;
            purchaseNumber = Mathf.Max(purchaseNumber, 0);
            Check();
        }
        private void SuperMinus()
        {
            purchaseNumber -= 5;
            purchaseNumber = Mathf.Max(purchaseNumber, 0);
            Check();
        }
    }
}
