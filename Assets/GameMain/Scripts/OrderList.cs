using DG.Tweening;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OrderList : MonoBehaviour
    {
        [SerializeField] private List<Transform> plotsCanvas = new List<Transform>();
        [SerializeField] private List<OrderItem> orders = new List<OrderItem>();
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject orderPre;

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);

            Invoke(nameof(ShowItem), 2f);
            Invoke(nameof(ShowItem), 4f);
            Invoke(nameof(ShowItem), 6f);
            Invoke(nameof(ShowItem), 8f);
            Invoke(nameof(ShowItem), 10f);
        }
        private void ShowItem() => ShowItem(new OrderData());
        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        }

        public void ShowItem(List<OrderData> orderDatas)
        {
            foreach (OrderData orderData in orderDatas) 
            {
                ShowItem(orderData);
            }
        }
        public void ShowItem(OrderData orderData)
        {
            if (orders.Count >= 4)
                return;
            GameEntry.Entity.ShowOrder(new OrderItemData(GameEntry.Entity.GenerateSerialId(), 10011, orderData)
            {
                Position = plotsCanvas[4].position
            });
        }

        public void OnOrderEvent(object sender,GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            OrderItem orderItem = (OrderItem)sender;
            int index = orders.IndexOf(orderItem);
            orders.Remove(orderItem);
            GameEntry.Entity.HideEntity(orderItem.Entity);
            ShowItem(new OrderData());
        }

        private void UpdateList()
        {
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].transform.DOMove(plotsCanvas[i].position, 1f).SetEase(Ease.InOutExpo);
            }
        }

        private void ShowOrderSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            OrderItem orderItem = null;
            if (args.Entity.TryGetComponent<OrderItem>(out orderItem))
            {
                orderItem.transform.position = plotsCanvas[4].position;
                orders.Add(orderItem);
                UpdateList();
            }
        }
    }

}