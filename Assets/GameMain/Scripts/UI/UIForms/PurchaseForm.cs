using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


        private int purchaseNumber;

        void Start()
        {
            exitPurchaseFormBtn.onClick.AddListener(ExitPurchaseForm);
            plusBtn.onClick.AddListener(Plus);
            superPlusBtn.onClick.AddListener(SuperPlus);
            minusBtn.onClick.AddListener(Minus);
            superMinusPlusBtn.onClick.AddListener(SuperMinus);
            PurchaseFormBuyBtn.onClick.AddListener(PurchaseComfirm); ;
            purchaseNumber = 6;
            purchaseNum.text = purchaseNumber.ToString();
        }
        void Update()
        {
            purchaseNum.text = purchaseNumber.ToString();
        }

        private void ExitPurchaseForm()
        {
            purchaseNum.text = 5.ToString();
            purchaseNumber = 5;
            this.gameObject.SetActive(false);
        }
        private void PurchaseComfirm()
        {
            purchaseNum.text = 5.ToString();
            purchaseNumber = 5;
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
        }


    }
}
