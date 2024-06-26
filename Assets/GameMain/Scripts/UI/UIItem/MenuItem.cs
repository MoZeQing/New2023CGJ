using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MenuItem : MonoBehaviour
    {
        [SerializeField] private Image[] stars;
        [SerializeField] private Image[] tags;
        [SerializeField] private Text coffeeNameText;
        [SerializeField] private Text demandText;

        public void SetData(DRCoffee dRCoffee)
        {
            coffeeNameText.text = dRCoffee.CoffeeName;
            demandText.text = $"¿§·ÈµÄÐèÇó£º{dRCoffee.Demand}";
        }
    }
}
