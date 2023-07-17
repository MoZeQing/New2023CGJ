using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class WorkForm : MonoBehaviour
    {
        [SerializeField] private Text EspressoText;
        [SerializeField] private Text ConPannaText;
        [SerializeField] private Text MochaText;
        [SerializeField] private Text WhiteCoffeeText;
        [SerializeField] private Text CafeAmericanoText;
        [SerializeField] private Text LatteText;
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, UpdateOrder);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, UpdateOrder);
        }

        private void UpdateOrder(object sender,GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            OrderData orderData = args.OrderData;
            EspressoText.text = orderData.Espresso.ToString();
            ConPannaText.text = orderData.ConPanna.ToString();
            MochaText.text = orderData.Mocha.ToString();
            WhiteCoffeeText.text = orderData.WhiteCoffee.ToString();
            CafeAmericanoText.text = orderData.CafeAmericano.ToString();
            LatteText.text = orderData.Latte.ToString();
        }
    }
}
