using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;
using UnityEngine.UI;
using GameFramework.Event;


namespace GameMain
{
    public class OrderManager : MonoBehaviour
    {
        private List<DROrder> orders = new List<DROrder>();
        private OrderData mOrderData = new OrderData();
        public OrderData OrderData
        {
            get
            {
                return mOrderData;
            }
            private set
            {
                mOrderData = value;
            }
        }

        private void Start()
        {
            //if (OrderData.Check())
            //{
            //    GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
            //    ProcedureMain main = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
            //    main.Level(this);
            //}
        }

        public void SetOrder(int index)
        {
            //IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();
            //DROrder drOrder = dtOrder.GetDataRow(index);
            //OrderData = new OrderData(drOrder);
            //OrderData.NodeTag = NodeTag.None;
            //GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//用于更新UI信息的事件，需要保证线程安全
        }

        public void SetOrder(OrderData order)
        {
            //OrderData = order;
            //OrderData.NodeTag = NodeTag.None;
            //GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//用于更新UI信息的事件，需要保证线程安全
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Entity baseCompenent = null;
            //if (collision.TryGetComponent<Entity>(out baseCompenent))
            //{
            //    if (baseCompenent.transform.parent.GetComponent<BaseNode>() == null)
            //    {
            //        Debug.LogWarning("错误，没有该脚本");
            //        return; 
            //    }
            //    if (baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData == null)
            //        return;
            //    NodeData nodeData = baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData;
            //    switch (nodeData.NodeTag)
            //    {
            //        case NodeTag.Espresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.IceEspresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.SweetEspresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.ConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.IceConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.SweetConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.Mocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.IceMocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.SweetMocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.WhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.IceWhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.SweetWhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.CafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.IceCafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.SweetCafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.Latte:
            //            OrderData.Latte -= 1;
            //            break;
            //        case NodeTag.IceLatte:
            //            OrderData.Latte -= 1;
            //            break;
            //        case NodeTag.SweetLatte:
            //            OrderData.Latte -= 1;
            //            break;
            //        default:
            //            return;
            //    }
            //    GameEntry.Entity.HideEntity(nodeData.Id);
            //    OrderData.NodeTag= nodeData.NodeTag;
            //    GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
            //}
        }
    }

    public class MaterialData
    { 
        public int Water
        {
            get;
            set;
        }

        public int CoffeeBean
        {
            get;
            set;
        }

        public int Milk
        {
            get;
            set;
        }

        public int ChocolateSyrup
        {
            get;
            set;
        }

        public int Cream
        {
            get;
            set;
        }
        public int Ice
        {
            get;
            set;
        }
        public int Sugar
        {
            get;
            set;
        }
    }
    [System.Serializable]
    public class OrderData
    {
        //咖啡种类
        public NodeTag NodeTag;
        public LevelTag LevelTag;
        public string NodeName;
        //订单生成的时间
        public float OrderTime;
        public bool Grind;//粗细度
        public bool Urgent=false;//加急订单
        public bool Bad = false;//糟糕的订单

        public OrderData() { }

        public OrderData(NodeTag nodeTag,LevelTag leveltag,int orderTime,bool isCoarse,bool notCoarse)
        {
            LevelTag= leveltag;
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
            this.NodeTag = nodeTag;
            this.NodeName = dRNode.AssetName;
            switch (leveltag)
            {
                case LevelTag.Urgent:
                    if (orderTime != 0)
                    {
                        this.Urgent = true;
                        this.OrderTime = orderTime;
                    }
                    else
                    {
                        this.OrderTime = -2;
                    }
                    break;
                case LevelTag.Bad:
                    Bad= Random.Range(0, 2) == 1;
                    break;
            }
            if (Urgent)
            {
                this.OrderTime = dRNode.Time;
            }
            if (isCoarse)
                Grind = true;
            else if(notCoarse)
                Grind = false;
            else
                Grind = Random.Range(0, 2) == 1;
        }
    }
}
