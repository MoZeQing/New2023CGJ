using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Text priceText;
        [SerializeField] private Text inventoryText;
        [SerializeField] private Button subBtn;
        [SerializeField] private Button PurchaseButton;
        [SerializeField] private GameObject PurchaseForm;

        void Start()
        {
            PurchaseButton.onClick.AddListener(Purchase);
        }

        private void Purchase()
        {
            PurchaseForm.SetActive(true);
        }


    }
}
