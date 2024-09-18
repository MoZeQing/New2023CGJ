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

        private bool mIsShowItem = true;

        /// <summary>
        /// 是否自动随机生成订单
        /// </summary>
        public bool IsShowItem
        {
            get
            {
                return mIsShowItem;
            }
            set
            {
                if (value)
                    nowTime = -1;
                else
                    nowTime = 9999999f;
                mIsShowItem = value;
            }
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowOrderSuccess);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, OnLevelEvent);
            GameEntry.Event.Subscribe(EventEventArgs.EventId, OnEventEvent);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowOrderSuccess);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, OnLevelEvent);
            GameEntry.Event.Unsubscribe(EventEventArgs.EventId, OnEventEvent);
        }

        private float nowTime=5f;
        private float rateTime=10f;

        private void Update()
        {
            if (!mIsShowItem) 
                return;
            nowTime -= Time.deltaTime;
            if (nowTime <= 0)
            {
                ShowItem();
                nowTime = UnityEngine.Random.Range(12f, 25f);
            }
        }
        public void ShowItem(int id)
        {
            OrderData orderData = new OrderData();
            orderData.NodeTag = (NodeTag)id;
            orderData.Grind = Random.Range(0, 2) == 1;
            orderData.OrderTime = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)orderData.NodeTag).Time;
            orderData.NodeName = orderData.NodeTag.ToString();
            ShowItem(orderData);
        }
        public void ShowItem()
        {
            OrderData orderData = new OrderData();
            List<NodeTag> coffees = new List<NodeTag>();
            foreach (DRNode node in GameEntry.DataTable.GetDataTable<DRNode>().GetAllDataRows())
            {
                if (node.Coffee)
                    if(GameEntry.Player.HasCoffeeRecipe((NodeTag)node.Id))
                        coffees.Add((NodeTag)node.Id);
            }
            orderData.NodeTag = coffees[Random.Range(0, coffees.Count - 1)];
            orderData.Grind = Random.Range(0, 2) == 1;
            orderData.OrderTime = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)orderData.NodeTag).Time;
            orderData.NodeName = orderData.NodeTag.ToString();
            ShowItem(orderData);
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
            if (!IsShowItem)
                return;
            if (orders.Count >= 4)
                return;
            GameEntry.Entity.ShowOrder(new OrderItemData(GameEntry.Entity.GenerateSerialId(), 10011, orderData)
            {
                Position = plotsCanvas[plotsCanvas.Count-1].position
            });
        }

        public void ClearItems()
        {
            OrderItem[] orderItems= new OrderItem[orders.Count];
            for (int i=0;i<orderItems.Length;i++)
            {
                orderItems[i] = orders[i];
            }
            for (int i = 0; i < orderItems.Length; i++)
            {
                orderItems[i].OnExit();
            }
            orders.Clear();
        }

        private void UpdateList()
        {
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].transform.DOMove(plotsCanvas[i].position, 1f).SetEase(Ease.InOutExpo);
            }
        }

        public void OnEventEvent(object sender, GameEventArgs e)
        { 
            EventEventArgs args= (EventEventArgs)e;
            string[] values = (string[])sender;
            IsShowItem = true;
            ShowItem(int.Parse(values[1]));
            IsShowItem=false;
        }

        public void OnOrderEvent(object sender,GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            OrderItem orderItem = (OrderItem)sender;
            int index = orders.IndexOf(orderItem);
            orders.Remove(orderItem);
        }

        private void OnShowOrderSuccess(object sender, GameEventArgs e)
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

        private void OnLevelEvent(object sender, GameEventArgs e)
        { 
            LevelEventArgs args= (LevelEventArgs)e;
            IsShowItem = false;
            ClearItems();
        }
    }

}