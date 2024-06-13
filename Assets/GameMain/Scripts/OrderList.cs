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

        private List<OrderData> finishOrders= new List<OrderData>();

        private bool mIsShowItem = true;
        private int mOrderNumber = 0;
        public int OrderMax { get; set; } = 0;

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
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, OnLevelEvent);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, OnLevelEvent);
        }

        //protected override void OnInit(object userData)
        //{
        //    base.OnInit(userData);
        //}

        //protected override void OnShow(object userData)
        //{
        //    base.OnShow(userData);
        //    GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
        //    GameEntry.Event.Subscribe(OrderEventArgs.EventId, OnOrderEvent);
        //    GameEntry.Event.Subscribe(LevelEventArgs.EventId, OnLevelEvent);
        //}

        //protected override void OnHide(bool isShutdown, object userData)
        //{
        //    base.OnHide(isShutdown, userData);
        //    GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowOrderSuccess);
        //    GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OnOrderEvent);
        //    GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, OnLevelEvent);
        //}

        private float nowTime=5f;
        private float rateTime=10f;

        private void Update()
        {
            nowTime -= Time.deltaTime;
            if (!mIsShowItem) 
                return;
            if (mOrderNumber >= OrderMax)
                return;
            if (nowTime <= 0)
            {
                ShowItem();
                nowTime = UnityEngine.Random.Range(12f, 25f) * (100 - GameEntry.Utils.OrderTime) / 100;
            }
        }
        public void ShowItem()
        {
            mOrderNumber++;
            OrderData orderData = new OrderData();
            List<NodeTag> coffees = new List<NodeTag>();
            foreach (DRNode node in GameEntry.DataTable.GetDataTable<DRNode>().GetAllDataRows())
            {
                if (node.Coffee)
                    if(GameEntry.Player.HasCoffeeRecipe((NodeTag)node.Id))
                        coffees.Add((NodeTag)node.Id);
            }
            orderData.NodeTag = coffees[Random.Range(0, coffees.Count - 1)];
            //生成好友订单
            int friendRand = Random.Range(0, 12);
            if (friendRand < 6)
            {
                List<string> keys = new List<string>(GameEntry.Utils.friends.Keys);
                orderData.friendName = keys[friendRand];
                orderData.friendFavor = 2;
            }
            //生成好友订单
            orderData.Grind = Random.Range(0, 2) == 1;
            orderData.OrderTime = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)orderData.NodeTag).Time*(100-GameEntry.Utils.OrderTime)/100f;
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
                Position = plotsCanvas[4].position
            });
        }

        public void ClearItems()
        {
            foreach (OrderItem orderItem in orders)
            {
                GameEntry.Entity.HideEntity(orderItem.Entity);
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

        public void OnOrderEvent(object sender,GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            OrderItem orderItem = (OrderItem)sender;
            int index = orders.IndexOf(orderItem);
            orders.Remove(orderItem);
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

        private void OnLevelEvent(object sender, GameEventArgs e)
        { 
            LevelEventArgs args= (LevelEventArgs)e;
            IsShowItem = false;
            ClearItems();
        }
    }

}